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
    public partial class EmplRiskManagementDataPage : System.Web.UI.Page
    {
        const string pageName = "EmplRiskManagementDataPage";

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
                // parameter in request query string is emplID (employee_id)
                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnDelete.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");                    

                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    
                    EmployeeTO Empl = getEmployee();

                    setLanguage();

                    InitializeSQLParameters(Empl);

                    GetRisks(Empl);

                    populateRisks(Empl);
                    populateRotation();

                    if (Empl.EmployeeID != -1)
                    {
                        lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();
                    }
                    else
                        tableSaveUpdate.Visible = lblNewEntry.Visible = false;

                    string resultParameter = "";
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                        {
                            tableSaveUpdate.Visible = lblNewEntry.Visible = false;
                            resultParameter = "&showSelection=false";
                        }
                    }

                    InitializeSelectedRecord();
                    
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=320" + resultParameter;                    
                }

                if (!SelRecord.Value.Trim().Equals(""))
                {
                    setSelection();
                    InitializeSelectedRecord();
                    SelRecord.Value = "";
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);

                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblRisk.Text = rm.GetString("lblRisk", culture);
                lblRotation.Text = rm.GetString("lblRotation", culture);
                lblNewEntry.Text = rm.GetString("newEntry", culture);

                btnSave.Text = rm.GetString("btnSave", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSelectedRecord()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);

                lblError.Text = "";
                
                if (SelBox.Value.Trim().Equals(""))
                {
                    lblNewEntry.Text = rm.GetString("newEntry", culture);
                    btnSave.Text = rm.GetString("btnSave", culture);

                    // set to initial values
                    tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat);
                    tbToDate.Text = "";
                    if (cbRisk.Items.Count > 0)
                        cbRisk.SelectedIndex = 0;
                    if (cbRotation.Items.Count > 0)
                        cbRotation.SelectedIndex = 0;

                    cbRisk.Enabled = true;
                    cbRisk_SelectedIndexChanged(this, new EventArgs());
                }
                else
                {
                    lblNewEntry.Text = rm.GetString("updateEntry", culture);
                    btnSave.Text = rm.GetString("btnUpdate", culture);

                    // set to last selected record values                    
                    uint recID = 0;
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID) && recID > 0)
                    {
                        EmployeeXRisk emplRisk = new EmployeeXRisk(Session[Constants.sessionConnection]);
                        emplRisk.EmplXRiskTO.RecID = recID;
                        List<EmployeeXRiskTO> list = emplRisk.SearchEmployeeXRisks();

                        if (list.Count > 0)
                        {
                            tbFromDate.Text = list[0].DateStart.ToString(Constants.dateFormat);

                            if (list[0].DateEnd.Equals(new DateTime()))
                                tbToDate.Text = "";
                            else
                                tbToDate.Text = list[0].DateEnd.ToString(Constants.dateFormat);

                            cbRisk.SelectedValue = list[0].RiskID.ToString().Trim();
                            cbRotation.SelectedValue = list[0].Rotation.ToString().Trim();

                            cbRisk.Enabled = false;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParameters(EmployeeTO empl)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);
                
                Session[Constants.sessionHeader] = rm.GetString("hdrID", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture) 
                    + "," + rm.GetString("hdrEmplRisk", culture) + "," + rm.GetString("hdrRotation", culture) + "," + rm.GetString("hdrTimePerformed", culture)
                    + "," + rm.GetString("hdrResult", culture);
                Session[Constants.sessionFields] = "id, date_start, date_end, risk_code, rotation, time_performed, result";
                Session[Constants.sessionColTypes] = null;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateFormat);
                formating.Add(2, (int)Constants.FormatTypes.DateFormat);
                formating.Add(5, (int)Constants.FormatTypes.DateTimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = "id";
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

        private void populateRisks(EmployeeTO empl)
        {
            try
            {
                List<RiskTO> riskList = new List<RiskTO>();
                if (empl.EmployeeID != -1)
                {
                    int company = -1;

                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(empl.EmployeeID.ToString().Trim());

                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        company = ascoDict[empl.EmployeeID].IntegerValue4;

                    Risk risk = new Risk(Session[Constants.sessionConnection]);
                    risk.RiskTO.WorkingUnitID = company;

                    riskList = risk.SearchRisks();
                }

                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                cbRisk.DataSource = riskList;
                if (!isAltLang)
                    cbRisk.DataTextField = "RiskCodeDescSR";
                else
                    cbRisk.DataTextField = "RiskCodeDescEN";
                cbRisk.DataValueField = "RiskID";
                cbRisk.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateRotation()
        {
            try
            {
                List<string> rotation = new List<string>();

                for (int i = 1; i < 25; i++)
                {
                    rotation.Add(i.ToString().Trim());
                }
                                
                cbRotation.DataSource = rotation;
                cbRotation.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetRisks(EmployeeTO empl)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);

                ClearSessionValues();

                List<List<object>> resultTable = new List<List<object>>();
                
                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("id", typeof(uint)));
                resultColumns.Add(new DataColumn("date_start", typeof(DateTime)));
                resultColumns.Add(new DataColumn("date_end", typeof(DateTime)));
                resultColumns.Add(new DataColumn("risk_code", typeof(string)));
                resultColumns.Add(new DataColumn("rotation", typeof(int)));
                resultColumns.Add(new DataColumn("time_performed", typeof(DateTime)));
                resultColumns.Add(new DataColumn("result", typeof(string)));

                Dictionary<int, RiskTO> riskDict = new Risk(Session[Constants.sessionConnection]).SearchRisksDictionary();
                EmployeeXRisk risk = new EmployeeXRisk(Session[Constants.sessionConnection]);
                risk.EmplXRiskTO.EmployeeID = empl.EmployeeID;
                List<EmployeeXRiskTO> riskList = risk.SearchEmployeeXRisks();

                Dictionary<int, EmployeeAsco4TO> ascoList = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(empl.EmployeeID.ToString());

                List<int> posRisks = new List<int>();
                if (ascoList.ContainsKey(empl.EmployeeID) && ascoList[empl.EmployeeID].IntegerValue6 != -1)
                {
                    List<RiskTO> risks = new EmployeePositionXRisk(Session[Constants.sessionConnection]).SearchRisks(ascoList[empl.EmployeeID].IntegerValue6.ToString());

                    foreach (RiskTO riskTO in risks)
                    {
                        if (!posRisks.Contains(riskTO.RiskID))
                            posRisks.Add(riskTO.RiskID);
                    }
                }
                                
                Dictionary<uint, MedicalCheckVisitDtlTO> dtlDict = new Dictionary<uint,MedicalCheckVisitDtlTO>();
                string recIDs = "";
                foreach (EmployeeXRiskTO riskTO in riskList)
                {
                    recIDs += riskTO.LastVisitRecID.ToString().Trim() + ",";
                }

                if (recIDs.Length > 0)
                {
                    recIDs = recIDs.Substring(0, recIDs.Length - 1);

                    List<MedicalCheckVisitDtlTO> dtlList = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]).SearchMedicalCheckVisitDetails(recIDs);

                    foreach (MedicalCheckVisitDtlTO dtlTO in dtlList)
                    {
                        if (!dtlDict.ContainsKey(dtlTO.RecID))
                            dtlDict.Add(dtlTO.RecID, dtlTO);
                    }
                }

                Dictionary<string, Color> itemColors = new Dictionary<string, Color>();

                foreach (EmployeeXRiskTO riskTO in riskList)
                {
                    if (!posRisks.Contains(riskTO.RiskID) && !itemColors.ContainsKey(riskTO.RecID.ToString().Trim()))                        
                        itemColors.Add(riskTO.RecID.ToString().Trim(), Color.Pink);

                    // create result row
                    List<object> resultRow = new List<object>();

                    resultRow.Add(riskTO.RecID);
                    resultRow.Add(riskTO.DateStart);
                    resultRow.Add(riskTO.DateEnd);
                    if (riskDict.ContainsKey(riskTO.RiskID))
                        resultRow.Add(riskDict[riskTO.RiskID].RiskCode);
                    else
                        resultRow.Add("");
                    resultRow.Add(riskTO.Rotation);
                    resultRow.Add(riskTO.LastDatePerformed);
                    if (dtlDict.ContainsKey(riskTO.LastVisitRecID))
                        resultRow.Add(dtlDict[riskTO.LastVisitRecID].Result.Trim());
                    else
                        resultRow.Add("");

                    resultTable.Add(resultRow);
                }

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionItemsColors] = itemColors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);

                lblError.Text = "";

                // create record to save / update
                EmployeeXRiskTO emplRiskTO = new EmployeeXRiskTO();
                
                // get rec_id
                uint recID = 0;
                if (!SelBox.Value.Trim().Equals(""))
                {
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (!uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID))
                        recID = 0;
                }

                emplRiskTO.EmployeeID = getEmployee().EmployeeID;
                if (emplRiskTO.EmployeeID == -1)
                {
                    lblError.Text = rm.GetString("noEmplSelected", culture);
                    setSelection();
                    return;
                }
                                
                int riskID = -1;
                if (cbRisk.SelectedItem != null && int.TryParse(cbRisk.SelectedItem.Value.Trim(), out riskID))
                    emplRiskTO.RiskID = riskID;
                if (emplRiskTO.RiskID == -1)
                {
                    lblError.Text = rm.GetString("noRiskSelected", culture);
                    setSelection();
                    return;
                }

                DateTime start = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                if (start.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noDateStart", culture);
                    setSelection();
                    return;
                }

                DateTime end = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                if (!tbToDate.Text.Trim().Equals("") && end.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noDateEnd", culture);
                    setSelection();
                    return;
                }

                if (!end.Equals(new DateTime()) && start.Date > end.Date)
                {
                    lblError.Text = rm.GetString("startAfterEnd", culture);
                    setSelection();
                    return;
                }

                // check if defined risk already exists for employee
                EmployeeXRisk emplRisk = new EmployeeXRisk(Session[Constants.sessionConnection]);
                emplRisk.EmplXRiskTO = emplRiskTO;

                // check if new risk overlap existing risk
                List<EmployeeXRiskTO> riskList = emplRisk.SearchEmployeeXRisks();
                
                bool riskExists = false;
                foreach (EmployeeXRiskTO riskTO in riskList)
                {
                    // skip updating risk from check
                    if (recID > 0 && riskTO.RecID == recID)
                        continue;

                    if (end.Equals(new DateTime()))
                    {
                        if (riskTO.DateEnd.Equals(new DateTime()) || riskTO.DateEnd.Date >= start.Date)                        
                            riskExists = true;
                    }
                    else
                    {
                        if (riskTO.DateEnd.Equals(new DateTime()))
                        {
                            if (end.Date >= riskTO.DateStart.Date)
                                riskExists = true;
                        }
                        else
                        {
                            if ((start.Date < riskTO.DateStart.Date && end.Date >= riskTO.DateStart.Date) || (start.Date >= riskTO.DateStart.Date && start.Date <= riskTO.DateEnd.Date))
                                riskExists = true;                                
                        }
                    }

                    if (riskExists)
                        break;
                }

                if (riskExists)
                {
                    lblError.Text = rm.GetString("riskDefinedExist", culture);
                    setSelection();
                    return;
                }                

                emplRisk.EmplXRiskTO.DateStart = start;
                emplRiskTO.DateEnd = end;
                emplRisk.EmplXRiskTO.RecID = recID;                

                int rotation = -1;
                if (cbRotation.SelectedItem != null && int.TryParse(cbRotation.SelectedItem.Text.Trim(), out rotation))
                    emplRiskTO.Rotation = rotation;
                if (emplRiskTO.Rotation == -1)
                {
                    lblError.Text = rm.GetString("noRotationSelected", culture);
                    setSelection();
                    return;
                }

                emplRiskTO.LastScheduleDate = new DateTime();

                emplRisk.EmplXRiskTO = emplRiskTO;
                if (emplRiskTO.RecID == 0)
                {
                    // save new record
                    emplRisk.EmplXRiskTO.LastDatePerformed = DateTime.Now;
                    emplRisk.EmplXRiskTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    if (emplRisk.Save(true) > 0)
                    {
                        SelBox.Value = "";

                        GetRisks(getEmployee());
                                                
                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=320";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordSaved", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordNotSaved", culture);
                        setSelection();
                    }
                }
                else
                {
                    // update record
                    emplRisk.EmplXRiskTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    emplRisk.EmplXRiskTO.ModifiedTime = new DateTime();
                    if (emplRisk.Update(true))
                    {
                        SelBox.Value = "";

                        GetRisks(getEmployee());

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=320";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordUpdated", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordNotUpdated", culture);
                        setSelection();
                    }
                }                

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplRiskManagementDataPage).Assembly);

                lblError.Text = "";

                // get ids of records for deletion
                if (SelBox.Value.Trim().Equals(""))
                    lblError.Text = rm.GetString("noRecordToDelete", culture);
                else
                {
                    string recIDs = SelBox.Value.Replace(Constants.delimiter, ',');
                    recIDs = recIDs.Substring(0, recIDs.Length - 1);
                    if (new EmployeeXRisk(Session[Constants.sessionConnection]).Delete(recIDs, true))
                    {
                        SelBox.Value = "";

                        GetRisks(getEmployee());

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=320";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordsDeleted", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordsNotDeleted", culture);
                        setSelection();
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.btnDelete_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPostBack_Click(object sender, EventArgs e)
        {
            try
            {
                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.btnPostBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbRisk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int riskID = -1;
                if (cbRisk.SelectedValue != null && int.TryParse(cbRisk.SelectedValue.Trim(), out riskID))
                {
                    Risk risk = new Risk(Session[Constants.sessionConnection]);
                    risk.RiskTO.RiskID = riskID;

                    List<RiskTO> list = risk.SearchRisks();
                    if (list.Count > 0)
                        cbRotation.SelectedValue = list[0].DefaultRotation.ToString().Trim();
                }

                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.cbRisk_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplRiskManagementDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplRiskManagementDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setSelection()
        {
            try
            {
                // put selection in session
                string[] selectedList = SelBox.Value.Trim().Split(Constants.delimiter);
                List<string> selKeys = new List<string>();
                foreach (string key in selectedList)
                {
                    if (!key.Equals(""))
                        selKeys.Add(key);
                }
                Session[Constants.sessionSelectedKeys] = selKeys;
                Session[Constants.sessionSamePage] = true;

                GetRisks(getEmployee());
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
