using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;

using Common;
using TransferObjects;
using Util;
using ReportsWeb;
using System.Net;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;


namespace ACTAWebUI
{
    public partial class WCReportsPage : System.Web.UI.Page
    {
        const string pageName = "WCReportsPage";

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
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                ClearSessionValues();

                if (!IsPostBack)
                {
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    setLanguage();

                    Empl = getEmployee();

                    InitializeData(Empl);

                    InitializeSQLParameters();
                    rbPayS.Enabled = false;
                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();

                        InitializeFillDataSet(DateTime.Now);
                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                    }
                    else // reload selected filter state
                    {
                        LoadState();
                        InitializeFillDataSet(CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text));
                        if (!rbCounter.Checked)
                            resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                        else
                        {
                            Session[Constants.sessionCountersEmployees] = Empl.EmployeeID.ToString().Trim();
                            resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/CountersPage.aspx";
                        }
                        setLabel();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCReportsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLabel()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);
                foreach (KeyValuePair<int, string> pair in getMonths())
                {
                    lbMonths.Items.Add(pair.Value);
                }

                DateTime current = DateTime.Now;
                lbMonths.SelectedIndex = current.Month - 1;
                tbYear.Text = current.Year + ".";
                foreach (KeyValuePair<int, string> pair in getMonths())
                {
                    if (pair.Key == current.Month)
                    {
                        lblGraphPeriod.Text = rm.GetString("lblDataForMonth", culture) + pair.Value + " " + current.Year;
                    }
                }

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

                Session[Constants.sessionEmplCounters] = null;
                Session[Constants.sessionDayPairs] = null;
                Session[Constants.sessionCountersEmployees] = null;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);

                lblEmplData.Text = rm.GetString("lblEmplData", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblStringone.Text = rm.GetString("lblEmployeeType", culture);
                lblPlant.Text = rm.GetString("lblPlant", culture);
                lblCostCentar.Text = rm.GetString("lblCostCentar", culture);
                lblWorkgroup.Text = rm.GetString("lblWorkgroup", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblBranch.Text = rm.GetString("lblBranch", culture);
                lblWUnit.Text = rm.GetString("lblWUnit", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblPeriod.Text = rm.GetString("lblMonth", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnShow.Text = rm.GetString("btnShow", culture);
                rbSummary.Text = rm.GetString("rbMonthly", culture);
                rbCounter.Text = rm.GetString("rbCounter", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);
                rbPayS.Text = rm.GetString("rbPayslip", culture);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCReportsPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCReportsPage.", filterState);
                }
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

        private Dictionary<int, string> getMonths()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);

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

        private List<IOPairProcessedTO> FindIOPairsForEmployee(DateTime month, EmployeeTO Empl)
        {
            try
            {
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                List<DateTime> datesList = new List<DateTime>();
                DateTime day = month;
                while (day < month.AddMonths(1))
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }

                if (Empl.EmployeeID != -1)
                {
                    IOPairList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(Empl.EmployeeID.ToString().Trim(), datesList, "");
                }

                return IOPairList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetValuesForDataRow(DataTable tableDays, DataTable tableMonthly, DateTime month, List<IOPairProcessedTO> IOPairList, EmployeeTO Empl)
        {
            try
            {
                string paymentCode = "";
                int NumDay = -1;
                string description = "";
                string day = "";
                string timeStringStart = "";
                string timeStringEnd = "";
                string startmin = "00";
                string endmin = "00";
                int presence = -1;
                int currentIndex = 0;
                description = "";
                int pass_type_id = -1;

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);


                if (Empl.EmployeeID != -1)
                {

                    Dictionary<int, PassTypeTO> PassTypes = getTypes();
                    Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                    Dictionary<DateTime, List<IOPairProcessedTO>> pairsForDay = new Dictionary<DateTime, List<IOPairProcessedTO>>();

                    foreach (IOPairProcessedTO pair in IOPairList)
                    {
                        if (!pairsForDay.ContainsKey(pair.IOPairDate.Date))
                            pairsForDay.Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        pairsForDay[pair.IOPairDate.Date].Add(pair);
                    }

                    DateTime currDate = new DateTime(month.Year, month.Month, 1, 0, 0, 0); // start with first day in month and go through all month days

                    DateTime nextMonthDate = currDate.AddMonths(1);

                    Dictionary<int, PassTypeTO> passTypes = getTypes();

                    while (currDate.Date < nextMonthDate.Date)
                    {
                        List<IOPairProcessedTO> ioPairsForDay = new List<IOPairProcessedTO>();

                        if (pairsForDay.ContainsKey(currDate.Date))
                            ioPairsForDay = pairsForDay[currDate.Date];

                        //set name of day
                        switch (currDate.DayOfWeek)
                        {
                            case DayOfWeek.Saturday:
                                day = rm.GetString("sat", culture).Remove(1).ToUpper();
                                break;
                            case DayOfWeek.Sunday:
                                day = rm.GetString("sun", culture).Remove(1).ToUpper();
                                break;
                            default:
                                day = "";
                                break;
                        }

                        //number of day in month
                        NumDay = currDate.Day;

                        //foreach iopair set start_time and end_time and payment_code
                        foreach (IOPairProcessedTO iopair in ioPairsForDay)
                        {
                            if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                foreach (KeyValuePair<int, PassTypeTO> passTypePair in passTypes)
                                {
                                    if (iopair.PassTypeID == passTypePair.Value.PassTypeID)
                                    {

                                        timeStringStart = iopair.StartTime.ToString("H:mm");
                                        timeStringEnd = iopair.EndTime.ToString("H:mm");

                                        string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                                        int cost = 0;
                                        bool costum = int.TryParse(costumer, out cost);
                                        bool isNikolaTesla = (cost == (int)Constants.Customers.NikolaTesla);
                                        if (!isNikolaTesla)
                                        {
                                            paymentCode = passTypePair.Value.PaymentCode;
                                        }
                                        else
                                        {
                                            paymentCode = "";
                                            TimeSpan value = iopair.EndTime.Subtract(iopair.StartTime);
                                            paymentCode = value.ToString();
                                        }
                                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                        {
                                            description = passTypePair.Value.Description;
                                        }
                                        else
                                        {
                                            description = passTypePair.Value.DescAlt;
                                        }

                                        presence = passTypePair.Value.IsPass;

                                        DataRow rowDays = tableDays.NewRow();

                                        DataRow row = tableMonthly.NewRow();
                                        rowDays["numDay"] = NumDay;
                                        rowDays["day"] = day;

                                        row["numDay"] = NumDay;
                                        row["day"] = day;

                                        int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                                        if (company != -1)
                                        {
                                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                                            rule.RuleTO.WorkingUnitID = company;
                                            rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                                            rule.RuleTO.RuleType = Constants.RuleCompanyRegularWork;

                                            List<RuleTO> rules = rule.Search();

                                            if (rules.Count == 1)
                                            {
                                                pass_type_id = rules[0].RuleValue;
                                            }
                                        }
                                        if (passTypePair.Value.PassTypeID == pass_type_id || presence == Constants.overtimePassType)
                                        {
                                            row["presenceComment"] = description;
                                            row["presenceStartTime"] = timeStringStart;
                                            row["presenceEndTime"] = timeStringEnd;
                                            row["presencePC"] = paymentCode;
                                        }
                                        else
                                        {
                                            row["absenceReason"] = description;
                                            row["absenceStartTime"] = timeStringStart;
                                            row["absenceEndTime"] = timeStringEnd;
                                            row["absencePC"] = paymentCode;
                                        }

                                        tableDays.Rows.Add(rowDays);
                                        tableDays.AcceptChanges();
                                        tableMonthly.Rows.Add(row);
                                        tableMonthly.AcceptChanges();
                                    }
                                }
                            }
                        }
                        if (ioPairsForDay.Count == 0)
                        {
                            DataRow rowDays = tableDays.NewRow();
                            DataRow row = tableMonthly.NewRow();

                            rowDays["numDay"] = NumDay;
                            rowDays["day"] = day;
                            row["numDay"] = NumDay;
                            row["day"] = day;
                            row["absencePC"] = row["absenceEndTime"] = row["absenceStartTime"] = row["presenceComment"] = row["presenceStartTime"] = row["presenceEndTime"] = row["presencePC"] = row["absenceReason"] = "";

                            tableDays.Rows.Add(rowDays);
                            tableDays.AcceptChanges();
                            tableMonthly.Rows.Add(row);
                            tableMonthly.AcceptChanges();
                        }
                        currentIndex++;
                        currDate = currDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                Empl = getEmployee();
                RadioButton rb = sender as RadioButton;
                if (rb == rbSummary)
                {

                    rbCounter.Checked = rbPayS.Checked = !rbSummary.Checked;
                }

                else if (rb == rbCounter)
                {

                    rbSummary.Checked = rbPayS.Checked = !rbCounter.Checked;

                }
                else if (rb == rbPayS)
                {

                    rbSummary.Checked = rbCounter.Checked = !rbPayS.Checked;
                }

                lbMonths.Enabled = tbYear.Enabled = !rbCounter.Checked;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCReportsPage.rb_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void FillCounters(DataTable tableCounters, EmployeeTO Empl)
        {
            try
            {
                if (Empl.EmployeeID != -1)
                {
                    DataRow row = tableCounters.NewRow();
                    row["annualLeave"] = row["prevYearLeave"] = row["thisYearLeave"] = row["paidLeaveCounter"] = row["bankHoursCounter"] = row["overtimeCounter"] = row["stopWorkingCounter"] = "0";
                    EmployeeCounterValue emplCValue = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                    emplCValue.ValueTO.EmplID = Empl.EmployeeID;
                    List<EmployeeCounterValueTO> counterValues = emplCValue.Search();

                    int prevYearLeave = 0;
                    int thisYearLeave = 0;
                    int annualLeaveUsed = 0;

                    foreach (EmployeeCounterValueTO val in counterValues)
                    {
                        switch (val.EmplCounterTypeID)
                        {
                            case (int)Constants.EmplCounterTypes.AnnualLeaveCounter:
                                thisYearLeave = val.Value;
                                break;
                            case (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter:
                                prevYearLeave = val.Value;
                                break;
                            case (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter:
                                annualLeaveUsed = val.Value;
                                break;
                            case (int)Constants.EmplCounterTypes.PaidLeaveCounter:
                                row["paidLeaveCounter"] = val.Value.ToString().Trim();
                                break;
                            case (int)Constants.EmplCounterTypes.BankHoursCounter:
                                row["bankHoursCounter"] = CommonWeb.Misc.createHoursFromMinutes(val.Value);
                                break;
                            case (int)Constants.EmplCounterTypes.OvertimeCounter:
                                row["overtimeCounter"] = CommonWeb.Misc.createHoursFromMinutes(val.Value);
                                break;
                            case (int)Constants.EmplCounterTypes.StopWorkingCounter:
                                row["stopWorkingCounter"] = CommonWeb.Misc.createHoursFromMinutes(val.Value);
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
                    row["thisYearLeave"] = thisYearLeave.ToString().Trim();
                    row["prevYearLeave"] = prevYearLeave.ToString().Trim();
                    row["annualLeave"] = (thisYearLeave + prevYearLeave).ToString().Trim();
                    tableCounters.Rows.Add(row);
                    tableCounters.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HoursCalcForSubReport(DataTable tablePayment, List<IOPairProcessedTO> IOPairList)
        {
            try
            {
                int numberRow = 1;
                Dictionary<string, TimeSpan> typesCounter = new Dictionary<string, TimeSpan>();
                Dictionary<int, PassTypeTO> passTypes = getTypes();
                foreach (IOPairProcessedTO iopair in IOPairList)
                {
                    if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                    {
                        foreach (KeyValuePair<int, PassTypeTO> pair in passTypes)
                        {
                            if (iopair.PassTypeID == pair.Value.PassTypeID)
                            {

                                TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                                if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                    duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                                if (typesCounter.ContainsKey(pair.Value.PaymentCode))
                                    typesCounter[pair.Value.PaymentCode] = typesCounter[pair.Value.PaymentCode].Add(duration);
                                else
                                    typesCounter.Add(pair.Value.PaymentCode, duration);
                            }
                        }
                    }
                }

                foreach (string ptID in typesCounter.Keys)
                {
                    DataRow row = tablePayment.NewRow();
                    string hours = ((int)typesCounter[ptID].TotalHours).ToString().Trim();
                    string minutes = ((((int)typesCounter[ptID].TotalMinutes - (int)typesCounter[ptID].TotalHours * 60) * 100) / 60).ToString();
                    if (minutes == "0")
                    {
                        row["hours"] = double.Parse(hours);
                    }
                    else if (minutes.EndsWith("0"))
                    {
                        minutes = minutes.Remove(minutes.LastIndexOf('0'));

                        row["hours"] = double.Parse(hours + "." + minutes);
                    }
                    else
                    {
                        row["hours"] = double.Parse(hours + "." + minutes);
                    }
                    row["payment"] = ptID;
                    row["numberRow"] = numberRow;
                    tablePayment.Rows.Add(row);
                    tablePayment.AcceptChanges();
                    numberRow++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InitializeFillDataSet(DateTime month)
        {
            try
            {
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionDataTableList] = null;

                DataSet dataSet = new DataSet();
                DataTable tableMonthly = new DataTable("wc_io_pairs");
                DataTable tableDays = new DataTable("wc_days");
                DataTable tablePayment = new DataTable("wc_payment");
                DataTable tableCounters = new DataTable("wc_counters");


                tableCounters.Columns.Add("thisYearLeave", typeof(System.String));
                tableCounters.Columns.Add("prevYearLeave", typeof(System.String));
                tableCounters.Columns.Add("annualLeave", typeof(System.String));
                tableCounters.Columns.Add("paidLeaveCounter", typeof(System.String));
                tableCounters.Columns.Add("bankHoursCounter", typeof(System.String));
                tableCounters.Columns.Add("overtimeCounter", typeof(System.String));
                tableCounters.Columns.Add("stopWorkingCounter", typeof(System.String));

                //table monthly
                tableMonthly.Columns.Add("numDay", typeof(System.Int32));
                tableMonthly.Columns.Add("day", typeof(System.String));
                tableMonthly.Columns.Add("absenceReason", typeof(System.String));
                tableMonthly.Columns.Add("absenceStartTime", typeof(System.String));
                tableMonthly.Columns.Add("absenceEndTime", typeof(System.String));
                tableMonthly.Columns.Add("absencePC", typeof(System.String));
                tableMonthly.Columns.Add("presenceComment", typeof(System.String));
                tableMonthly.Columns.Add("presenceStartTime", typeof(System.String));
                tableMonthly.Columns.Add("presenceEndTime", typeof(System.String));
                tableMonthly.Columns.Add("presencePC", typeof(System.String));

                //table days
                tableDays.Columns.Add("numDay", typeof(System.Int32));
                tableDays.Columns.Add("day", typeof(System.String));

                //table summary payment
                tablePayment.Columns.Add("payment", typeof(System.String));
                tablePayment.Columns.Add("hours", typeof(System.Double));
                tablePayment.Columns.Add("numberRow", typeof(System.Int32));

                dataSet.Tables.Add(tableMonthly);
                dataSet.Tables.Add(tableDays);
                dataSet.Tables.Add(tablePayment);
                dataSet.Tables.Add(tableCounters);

                EmployeeTO Empl = getEmployee();

                List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(month, Empl);

                SetValuesForDataRow(tableDays, tableMonthly, month, IOPairList, Empl);
                HoursCalcForSubReport(tablePayment, IOPairList);

                //fill session with dataset for report
                Session["WCReportsPage.dataSet"] = dataSet;

                //data for resultPage
                string filter = "ip.pass_type_id=pt.pass_type_id and ip.employee_id=e.employee_idAND e.employee_id IN (" + Empl.EmployeeID.ToString() + ")";
                filter += " AND MONTH(ip.io_pair_date)=" + month.Month + " AND YEAR(ip.io_pair_date)=" + month.Year;

                List<DataColumn> resultColumns = new List<DataColumn>();

                resultColumns.Add(new DataColumn("numDay", typeof(Int32)));
                resultColumns.Add(new DataColumn("day", typeof(string)));
                resultColumns.Add(new DataColumn("absenceReason", typeof(string)));
                resultColumns.Add(new DataColumn("absenceStartTime", typeof(string)));
                resultColumns.Add(new DataColumn("absenceEndTime", typeof(string)));
                resultColumns.Add(new DataColumn("absencePC", typeof(string)));
                resultColumns.Add(new DataColumn("presenceComment", typeof(string)));
                resultColumns.Add(new DataColumn("presenceStartTime", typeof(string)));
                resultColumns.Add(new DataColumn("presenceEndTime", typeof(string)));
                resultColumns.Add(new DataColumn("presencePC", typeof(string)));

                List<List<object>> resultTable = new List<List<object>>();

                foreach (DataRow row in tableMonthly.Rows)
                {
                    List<object> resultRow = new List<object>();
                    resultRow.Add(row["numDay"]);
                    resultRow.Add(row["day"]);
                    resultRow.Add(row["absenceReason"]);
                    resultRow.Add(row["absenceStartTime"]);
                    resultRow.Add(row["absenceEndTime"]);
                    resultRow.Add(row["absencePC"]);
                    resultRow.Add(row["presenceComment"]);
                    resultRow.Add(row["presenceStartTime"]);
                    resultRow.Add(row["presenceEndTime"]);
                    resultRow.Add(row["presencePC"]);
                    resultTable.Add(resultRow);

                }

                Session[Constants.sessionFilter] = filter;
                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionSortCol] = "absenceStartTime,presenceStartTime";
                Session[Constants.sessionSortDir] = Constants.sortAsc;
                FillCounters(tableCounters, Empl);

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
                Empl = getEmployee();
                SaveState();
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);


                DateTime month = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                DateTime nextMonth = month.AddMonths(1);
                if (rbCounter.Checked && Session[Constants.sessionCounters] == null)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                int noOfDays = 0;
                while (month < nextMonth)
                {
                    month = month.AddDays(1);
                    noOfDays++;
                }

                month = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                DateTime toDate = CommonWeb.Misc.createDate(noOfDays + "." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                EmployeeAsco4 emplAsco4 = new EmployeeAsco4(Session[Constants.sessionConnection]);
                emplAsco4.EmplAsco4TO.EmployeeID = Empl.EmployeeID; ;

                List<EmployeeAsco4TO> emplAscoList = emplAsco4.Search();

                if (emplAscoList.Count == 1)
                {
                    Session["WCReportsPage.JMBG"] = emplAscoList[0].NVarcharValue4;
                }

                Session["WCReportsPage.Name"] = Empl.FirstAndLastName;
                Session["WCReportsPage.From"] = month.ToString("dd.MM.yyyy");
                Session["WCReportsPage.To"] = toDate.ToString("dd.MM.yyyy");
                Session["WCReportsPage.WU"] = Empl.WorkingUnitID;
                string reportURL = "";
                DataSet dataSet = (DataSet)Session["WCReportsPage.dataSet"];
                string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                bool isNikolaTesla = (cost == (int)Constants.Customers.NikolaTesla);
                string pc="";

                if (!isNikolaTesla)
                    pc = rm.GetString("pc", culture);
                else pc = rm.GetString("total", culture);
                Session["WCReportsPage.pc"] = pc;
                if (rbCounter.Checked)
                {

                    if (dataSet.Tables[3].Rows.Count == 0)
                    {
                        lblError.Text = rm.GetString("noReportData", culture);

                    }
                    else
                    {
                        Session["WCReportsPage.reportType"] = "counter";

                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/WCReportsReport_sr.aspx";

                        else reportURL = "/ACTAWeb/ReportsWeb/en/WCReportsReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Report=" + reportURL.Trim(), false);
                    }
                }
                else
                {

                    if (dataSet.Tables[1].Rows.Count == 0)
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                    }
                    else
                    {
                        Session["WCReportsPage.reportType"] = "monthly";
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/WCReportsReport_sr.aspx";

                        else reportURL = "/ACTAWeb/ReportsWeb/en/WCReportsReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Report=" + reportURL.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCReportsPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                ClearSessionValues();
                Empl = getEmployee();
                Session[Constants.sessionCounters] = null;

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);
                lblError.Text = "";
                lblGraphPeriod.Text = "";

                if (tbYear.Text == "")
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);
                }
                else
                {
                    if (rbSummary.Checked)
                    {
                        DateTime current = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                        if (current == new DateTime())
                        {
                            lblError.Text = rm.GetString("invalidDate", culture);
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
                            InitializeFillDataSet(current);
                            SaveState();
                            resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                        }
                    }
                    else if (rbCounter.Checked)
                    {
                        lblGraphPeriod.Text = "";

                        Session[Constants.sessionCountersEmployees] = getEmployee().EmployeeID.ToString();
                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/CountersPage.aspx";
                        SaveState();
                    }
                    else
                    {

                        lblGraphPeriod.Text = "";
                        SaveState();

                        EmployeeTO Employee = getEmployee();
                        Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                        int company = Common.Misc.getRootWorkingUnit(Employee.WorkingUnitID, WUnits);

                        //only for wc, for now this part is not in use, if not wc try to find his payslip and show if found

                        //string employeeType = "";
                        //EmployeeType emplType = new EmployeeType(Session[Constants.sessionConnection]);
                        //EmployeeTypeTO emplTypeTO = new EmployeeTypeTO();
                        //emplTypeTO.WorkingUnitID = company;
                        //emplTypeTO.EmployeeTypeID = Employee.EmployeeTypeID;
                        //emplType.EmployeeTypeTO = emplTypeTO;
                        //List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                        //listEmplTypes = emplType.Search();

                        //if (listEmplTypes.Count == 1)
                        //    employeeType = listEmplTypes[0].EmployeeTypeName;


                        //if (employeeType == Enum.GetName(typeof(Constants.EmployeeTypesFIAT), 2))
                        //{
                        DateTime current = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                        string name = Employee.EmployeeID + "_" + current.ToString("yyyy_MM") + "_";

                        Dictionary<int, string> dictionaryComaniesNames = new Dictionary<int, string>();
                        foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                        {

                            string companyName = Enum.GetName(typeof(Constants.FiatCompanies), comp);
                            dictionaryComaniesNames.Add(comp, companyName);

                        }
                        name = Constants.SharedAreaFiat + dictionaryComaniesNames[company] + "\\Payslips\\" + name;
                        
                        if (test.Value == "")
                        {
                            if (File.Exists(name))
                            {
                                ReadPdfFile(name);
                            }
                            else
                            {
                                lblError.Text = rm.GetString("payslipNotFound", culture);
                            }

                        }
                        else
                        {
                            if (File.Exists(name))
                            {
                                ReadPdfFileAttach(name);
                            }
                            else
                            {
                                lblError.Text = rm.GetString("payslipNotFound", culture);
                            }
                        }
                        //}
                        //else { return; }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCReportsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID, false);
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
        private void ReadPdfFileAttach(string path)
        {

            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment;Filename=test.pdf");
                Response.TransmitFile(path);
                Response.End();

            }

        }


        private void InitializeSQLParameters()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);
                string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                bool isNikolaTesla = (cost == (int)Constants.Customers.NikolaTesla);
                if (!isNikolaTesla)
                {
                    Session[Constants.sessionHeader] = rm.GetString("hdrDay", culture) + "," + "Ozn" + "," + rm.GetString("hdrReason", culture) + ","
                        + rm.GetString("hdrFrom", culture) + "," + rm.GetString("hdrTo", culture) + "," + rm.GetString("hdrPaymentCode", culture) + "," + rm.GetString("hdrComment", culture) + "," + rm.GetString("hdrFrom", culture) + "," + rm.GetString("hdrTo", culture) + "," + rm.GetString("hdrPaymentCode", culture);
                }
                else
                {
                    Session[Constants.sessionHeader] = rm.GetString("hdrDay", culture) + "," + "Ozn" + "," + rm.GetString("hdrReason", culture) + ","
                          + rm.GetString("hdrFrom", culture) + "," + rm.GetString("hdrTo", culture) + "," + rm.GetString("hdrTotal", culture) + "," + rm.GetString("hdrComment", culture) + "," + rm.GetString("hdrFrom", culture) + "," + rm.GetString("hdrTo", culture) + "," + rm.GetString("hdrTotal", culture);
                }
                Session[Constants.sessionFields] = "numDay,day,absenceReason,absenceStartTime,absenceEndTime,absencePC,presenceComment,presenceStartTime,presenceEndTime,presencePC";
                Session[Constants.sessionFieldsFormating] = null;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = "io_pairs_processed ip,pass_types pt,employees e";
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
    }
}
