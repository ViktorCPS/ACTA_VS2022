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
    public partial class EmplAnagraphicalDataPage : System.Web.UI.Page
    {
        const string pageName = "EmplAnagraphicalDataPage";

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
                // parameter in request query string is emplID (employee_id) and Back if Back button should be visble and functional
                if (!IsPostBack)
                {
                    setLanguage();

                    EmployeeTO Empl = getEmployee();

                    InitializeData(Empl);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplAnagraphicalDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplAnagraphicalDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplAnagraphicalDataPage).Assembly);

                lblCompany.Text = rm.GetString("lblComp", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblEmplID.Text = rm.GetString("lblEmployeeID", culture);
                lblStringone.Text = rm.GetString("lblStringone", culture);
                lblShiftGroup.Text = rm.GetString("lblShiftGroup", culture);
                lblBirthDate.Text = rm.GetString("lblBirthDate", culture);
                lblCity.Text = rm.GetString("lblCity", culture);
                lblAddress.Text = rm.GetString("lblAddress", culture);
                lblPhone1.Text = rm.GetString("lblPhone1", culture);
                lblPhone2.Text = rm.GetString("lblPhone2", culture);
                lblHiringDate.Text = rm.GetString("lblHiringDate", culture);
                lblTerminationDate.Text = rm.GetString("lblTerminationDate", culture);
                lblPosition.Text = rm.GetString("lblPosition", culture);
                lblPositionRisks.Text = rm.GetString("lblPositionRisks", culture);
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
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplAnagraphicalDataPage).Assembly);

                    lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();

                    // get employee working unit - UTE
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    tbUTE.Text = wu.FindWU(Empl.WorkingUnitID).Code.Trim();

                    // get organizational unit
                    tbOUnit.Text = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(Empl.OrgUnitID).Code.Trim();

                    tbFirstName.Text = Empl.FirstName.Trim();
                    tbLastName.Text = Empl.LastName.Trim();
                    tbEmplID.Text = Empl.EmployeeID.ToString();

                    // get additional employee data
                    EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    asco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();
                    EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                    if (ascoList.Count == 1)
                        ascoTO = ascoList[0];

                    tbStringone.Text = ascoTO.NVarcharValue2.Trim();

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

                    tbCompany.Text = wu.FindWU(ascoTO.IntegerValue4).Code.Trim();

                    // get shift group
                    tbShiftGroup.Text = new WorkingGroup(Session[Constants.sessionConnection]).Find(Empl.WorkingGroupID).GroupName.Trim();

                    if (!ascoTO.DatetimeValue5.Equals(new DateTime()) && !ascoTO.DatetimeValue5.Equals(Constants.dateTimeNullValue()))
                        tbBirthDate.Text = ascoTO.DatetimeValue5.ToString(Constants.dateFormat);

                    // get city and address (address is in format Street, house number, post code + city delimited with '|' E.G. Marka Karaljvica|28|11000 Beograd)
                    if (ascoTO.NVarcharValue8.LastIndexOf('|') >= 0)
                    {
                        string city = ascoTO.NVarcharValue8.Substring(ascoTO.NVarcharValue8.LastIndexOf('|') + 1);                        
                        tbCity.Text = city.Substring(city.IndexOf(" ") + 1);                        
                        tbAddress.Text = ascoTO.NVarcharValue8.Substring(0, ascoTO.NVarcharValue8.LastIndexOf('|'));
                    }

                    tbPhone1.Text = ascoTO.NVarcharValue9.Trim();
                    tbPhone2.Text = ascoTO.NVarcharValue10.Trim();

                    if (!ascoTO.DatetimeValue2.Equals(new DateTime()) && !ascoTO.DatetimeValue2.Equals(Constants.dateTimeNullValue()))
                        tbHiringDate.Text = ascoTO.DatetimeValue2.ToString(Constants.dateFormat);

                    if (!ascoTO.DatetimeValue3.Equals(new DateTime()) && !ascoTO.DatetimeValue3.Equals(Constants.dateTimeNullValue()))
                        tbTerminationDate.Text = ascoTO.DatetimeValue3.ToString(Constants.dateFormat);

                    if (ascoTO.IntegerValue6 != -1)
                    {
                        EmployeePosition pos = new EmployeePosition(Session[Constants.sessionConnection]);
                        pos.EmplPositionTO.PositionID = ascoTO.IntegerValue6;

                        List<EmployeePositionTO> posList = pos.SearchEmployeePositions();

                        bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                        if (posList.Count > 0)
                        {                            
                            if (!isAltLang)
                            {
                                tbPosition.Text = posList[0].PositionCodeTitleSR.Trim();
                                tbPosition.ToolTip = posList[0].DescSR.Trim();
                            }
                            else
                            {
                                tbPosition.Text = posList[0].PositionCodeTitleEN.Trim();
                                tbPosition.ToolTip = posList[0].DescEN.Trim();
                            }
                        }

                        EmployeePositionXRisk posXrisk = new EmployeePositionXRisk(Session[Constants.sessionConnection]);
                        posXrisk.EmplPositionXRiskTO.PositionID = ascoTO.IntegerValue6;

                        List<EmployeePositionXRiskTO> list = posXrisk.SearchEmployeePositionXRisks();

                        Dictionary<int, RiskTO> riskDict = new Dictionary<int, RiskTO>();
                        if (list.Count > 0)
                            riskDict = new Risk(Session[Constants.sessionConnection]).SearchRisksDictionary();
                        
                        List<RiskTO> riskList = new List<RiskTO>();
                        foreach (EmployeePositionXRiskTO risk in list)
                        {
                            if (riskDict.ContainsKey(risk.RiskID))
                                riskList.Add(riskDict[risk.RiskID]);
                        }

                        lbPosRisks.DataSource = riskList;
                        if (!isAltLang)
                        {
                            lbPosRisks.DataTextField = "RiskCodeDescSR";
                            lbPosRisks.DataValueField = "DescSR";
                        }
                        else
                        {
                            lbPosRisks.DataTextField = "RiskCodeDescEN";
                            lbPosRisks.DataValueField = "DescEN";
                        }
                        lbPosRisks.DataBind();

                        foreach (ListItem item in lbPosRisks.Items)
                        {
                            item.Attributes.Add("title", item.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbPosRisks_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbPosRisks.Items)
                {
                    item.Attributes.Add("title", item.Value);
                }
            }
            catch { }
        }

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplAnagraphicalDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplAnagraphicalDataPage.", filterState);
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
    }
}
