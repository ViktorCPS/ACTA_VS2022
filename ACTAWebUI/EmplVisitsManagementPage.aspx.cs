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
    public partial class EmplVisitsManagementPage : System.Web.UI.Page
    {
        const string pageName = "EmplVisitsManagementPage";

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
                // parameter in request query string is emplID (employee_id)
                if (!IsPostBack)
                {
                    btnDatePerformed.Attributes.Add("onclick", "return calendarPicker('tbDatePerformed', 'false');");                    
                    btnUpdate.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnDelete.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnDatePerformed.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnDatePerformed.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    
                    EmployeeTO Empl = getEmployee();

                    setLanguage();

                    InitializeSQLParameters(Empl);

                    GetVisits(Empl);

                    populateResult();                    

                    if (Empl.EmployeeID != -1)
                    {
                        lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();
                    }
                    else
                        tableSaveUpdate.Visible = lblNewEntry.Visible = false;

                    string resultParameter = "";
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckSupervisor
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckWCDR
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                        {
                            tableSaveUpdate.Visible = lblNewEntry.Visible = false;
                            resultParameter = "&showSelection=false";
                        }
                    }

                    InitializeSelectedRecord();

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=290" + resultParameter;
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVisitsManagementPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVisitsManagementPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                lblDatePerformed.Text = rm.GetString("lblDate", culture);
                lblResult.Text = rm.GetString("lblResult", culture);
                lblRisk.Text = rm.GetString("lblRisk", culture);
                lblScheduledDate.Text = rm.GetString("lblScheduledDate", culture);
                lblNewEntry.Text = rm.GetString("updateEntry", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblTermin.Text = rm.GetString("lblTermin", culture);
                lblTimePerformed.Text = rm.GetString("lblTime", culture);
                lblType.Text = rm.GetString("lblType", culture);

                btnUpdate.Text = rm.GetString("btnUpdate", culture);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                lblError.Text = "";

                if (SelBox.Value.Trim().Equals(""))
                {
                    // set to initial values
                    tbDatePerformed.Text = tbType.Text = tbRisk.Text = tbScheduledDate.Text = tbStatus.Text = tbTermin.Text = tbTimePerformed.Text = "";                    
                    if (cbResult.Items.Count > 0)
                        cbResult.SelectedIndex = 0;                    
                }
                else
                {
                    // set to last selected record values                    
                    uint recID = 0;
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID) && recID > 0)
                    {
                        // take dtl rekord
                        MedicalCheckVisitDtl dtl = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]);
                        dtl.VisitDtlTO.RecID = recID;
                        List<MedicalCheckVisitDtlTO> listDtl = dtl.SearchMedicalCheckVisitDetails();
                        
                        if (listDtl.Count > 0)
                        {
                            // take hdr rekord
                            MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);
                            hdr.VisitHdrTO.VisitID = listDtl[0].VisitID;
                            List<MedicalCheckVisitHdrTO> listHdr = hdr.SearchMedicalCheckVisitHeaders();

                            if (listDtl[0].DatePerformed.Equals(new DateTime()))
                            {
                                tbDatePerformed.Text = DateTime.Now.Date.ToString(Constants.dateFormat);
                                tbTimePerformed.Text = DateTime.Now.ToString(Constants.timeFormat);
                            }
                            else
                            {
                                tbDatePerformed.Text = listDtl[0].DatePerformed.Date.ToString(Constants.dateFormat);
                                tbTimePerformed.Text = listDtl[0].DatePerformed.ToString(Constants.timeFormat);
                            }

                            tbType.Text = rm.GetString(listDtl[0].Type.Trim(), culture);

                            bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                            if (listDtl[0].Type == Constants.VisitType.R.ToString())
                            {
                                // get risk code
                                Risk risk = new Risk(Session[Constants.sessionConnection]);
                                risk.RiskTO.RiskID = listDtl[0].CheckID;
                                List<RiskTO> riskList = risk.SearchRisks();

                                if (riskList.Count > 0)
                                {
                                    if (!isAltLang)
                                        tbRisk.Text = riskList[0].RiskCodeDescSR.Trim();
                                    else
                                        tbRisk.Text = riskList[0].RiskCodeDescEN.Trim();
                                }
                            }
                            else if (listDtl[0].Type == Constants.VisitType.V.ToString())
                            {
                                // get vaccine type
                                Vaccine vac = new Vaccine(Session[Constants.sessionConnection]);
                                vac.VaccineTO.VaccineID = listDtl[0].CheckID;
                                List<VaccineTO> vacList = vac.SearchVaccines();

                                if (vacList.Count > 0)
                                {
                                    if (!isAltLang)
                                        tbRisk.Text = vacList[0].VaccineTypeDescSR.Trim();
                                    else
                                        tbRisk.Text = vacList[0].VaccineTypeDescEN.Trim();
                                }
                            }                             

                            if (cbResult.Items.Count > 0)
                            {
                                if (listDtl[0].Result.Trim().Equals(""))
                                    cbResult.SelectedIndex = 0;
                                else
                                    cbResult.SelectedValue = listDtl[0].Result.Trim();
                            }
                                                        
                            if (listHdr.Count > 0)
                            {
                                tbStatus.Text = rm.GetString(listHdr[0].Status.Trim(), culture);
                                tbScheduledDate.Text = listHdr[0].ScheduleDate.Date.ToString(Constants.dateFormat);
                                tbTermin.Text = listHdr[0].ScheduleDate.ToString(Constants.timeFormat);
                            }
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrID", culture) + "," + rm.GetString("hdrTimePerformed", culture) + "," + rm.GetString("hdrStatus", culture)
                    + "," + rm.GetString("hdrType", culture) + "," + rm.GetString("hdrRiskVaccine", culture) + "," + rm.GetString("hdrResult", culture)
                    + "," + rm.GetString("hdrTermin", culture) + "," + rm.GetString("hdrDateScheduled", culture);
                Session[Constants.sessionFields] = "id, time_performed, status, type, risk, result, termin, date_scheduled";
                Session[Constants.sessionColTypes] = null;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateTimeFormat);
                formating.Add(6, (int)Constants.FormatTypes.TimeFormat);
                formating.Add(7, (int)Constants.FormatTypes.DateFormat);
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

        private void GetVisits(EmployeeTO empl)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                ClearSessionValues();

                List<List<object>> resultTable = new List<List<object>>();

                // create list of columns for list preview of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("id", typeof(uint)));
                resultColumns.Add(new DataColumn("time_performed", typeof(DateTime)));
                resultColumns.Add(new DataColumn("status", typeof(string)));
                resultColumns.Add(new DataColumn("type", typeof(string)));
                resultColumns.Add(new DataColumn("risk", typeof(string)));
                resultColumns.Add(new DataColumn("result", typeof(string)));
                resultColumns.Add(new DataColumn("termin", typeof(DateTime)));
                resultColumns.Add(new DataColumn("date_scheduled", typeof(DateTime)));
                
                Dictionary<uint, MedicalCheckVisitHdrTO> visits = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]).SearchMedicalCheckVisits(empl.EmployeeID.ToString(), "", "", "", "", new DateTime(), new DateTime());
                Dictionary<int, RiskTO> riskDict = new Risk(Session[Constants.sessionConnection]).SearchRisksDictionary();
                Dictionary<int, VaccineTO> vacDict = new Vaccine(Session[Constants.sessionConnection]).SearchVaccinesDictionary();
                

                foreach (uint id in visits.Keys)
                {
                    MedicalCheckVisitHdrTO hdrTO = visits[id];

                    // skip deleted visits
                    if (hdrTO.Status.Trim().ToUpper().Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString().Trim().ToUpper()))
                        continue;

                    // each visit detail is one row
                    foreach (MedicalCheckVisitDtlTO dtlTO in hdrTO.VisitDetails)
                    {
                        // create result row
                        List<object> resultRow = new List<object>();

                        resultRow.Add(dtlTO.RecID);
                        resultRow.Add(dtlTO.DatePerformed);
                        resultRow.Add(rm.GetString(hdrTO.Status, culture));
                        resultRow.Add(rm.GetString(dtlTO.Type, culture));
                        if (dtlTO.Type == Constants.VisitType.R.ToString() && riskDict.ContainsKey(dtlTO.CheckID))
                            resultRow.Add(riskDict[dtlTO.CheckID].RiskCode.Trim());
                        else if (dtlTO.Type == Constants.VisitType.V.ToString() && vacDict.ContainsKey(dtlTO.CheckID))
                            resultRow.Add(vacDict[dtlTO.CheckID].VaccineType.Trim());
                        else
                            resultRow.Add("");
                        resultRow.Add(dtlTO.Result.Trim());
                        resultRow.Add(hdrTO.ScheduleDate);
                        resultRow.Add(hdrTO.ScheduleDate);

                        resultTable.Add(resultRow);
                    }
                }

                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionDataTableList] = resultTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void populateResult()
        {
            try
            {
                List<string> results = new List<string>();

                results.Add(Constants.VisitResult.OK.ToString().Trim());
                results.Add(Constants.VisitResult.NOK.ToString().Trim());
                
                cbResult.DataSource = results;
                cbResult.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                lblError.Text = "";

                if (SelBox.Value.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noRecordToUpdate", culture);
                    return;
                }

                // create record to save / update
                MedicalCheckVisitDtl dtl = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]);

                // get rec_id
                uint recID = 0;
                if (!SelBox.Value.Trim().Equals(""))
                {
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (!uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID))
                        recID = 0;
                }

                // check entered date
                DateTime date = CommonWeb.Misc.createDate(tbDatePerformed.Text.Trim());
                if (date.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noDate", culture);
                    setSelection();
                    return;
                }

                // check entered time
                DateTime time = CommonWeb.Misc.createTime(tbTimePerformed.Text.Trim());
                if (time.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noTime", culture);
                    setSelection();
                    return;
                }

                dtl.VisitDtlTO.RecID = recID;
                List<MedicalCheckVisitDtlTO> list = dtl.SearchMedicalCheckVisitDetails();
                if (list.Count > 0)
                {
                    string user = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                    // get header record
                    MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);                    
                    List<MedicalCheckVisitHdrTO> hdrList = hdr.SearchMedicalCheckVisits(list[0].VisitID.ToString().Trim());

                    MedicalCheckVisitHdrHist hdrHist = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]);
                    if (hdrList.Count > 0)
                    {
                        hdrHist.VisitHdrHistTO = new MedicalCheckVisitHdrHistTO(hdrList[0]);                        
                        hdrHist.VisitHdrHistTO.ModifiedBy = user;
                        hdrHist.VisitHdrHistTO.ModifiedTime = new DateTime();

                        foreach (MedicalCheckVisitDtlHistTO dtlTO in hdrHist.VisitHdrHistTO.VisitDetails)
                        {
                            dtlTO.ModifiedBy = user;
                            dtlTO.ModifiedTime = new DateTime();
                        }
                    }

                    dtl.VisitDtlTO = list[0];
                    bool doneVisitChange = !dtl.VisitDtlTO.DatePerformed.Equals(new DateTime());
                    dtl.VisitDtlTO.DatePerformed = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                    dtl.VisitDtlTO.Result = cbResult.SelectedItem.Text.Trim();
                    
                    // if hdr status is not DONE, update hdr status to DONE if all dtls are confirmed
                    bool updateHdrStatus = false;
                    if (hdrList[0].Status.Trim().ToUpper() != Constants.MedicalCheckVisitStatus.DONE.ToString().Trim().ToUpper())
                    {
                        // get all details for this header
                        MedicalCheckVisitDtl detail = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]);
                        detail.VisitDtlTO.VisitID = list[0].VisitID;
                        List<MedicalCheckVisitDtlTO> dtlList = detail.SearchMedicalCheckVisitDetails();

                        bool completed = true;
                        foreach (MedicalCheckVisitDtlTO dtlTO in dtlList)
                        {
                            if (dtlTO.RecID != list[0].RecID && (dtlTO.DatePerformed.Equals(new DateTime()) || dtlTO.Result.Trim().Equals("")))
                            {
                                completed = false;
                                break;
                            }
                        }

                        if (completed)
                            updateHdrStatus = true;
                    }

                    // if it is risk visit, find risk updated from changing visit and update according to visit data
                    List<EmployeeXRiskTO> riskUpdateList = new List<EmployeeXRiskTO>();
                    if (dtl.VisitDtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.R.ToString().Trim().ToUpper()))
                    {
                        EmployeeXRisk emplRisk = new EmployeeXRisk(Session[Constants.sessionConnection]);
                        if (doneVisitChange)
                        {
                            emplRisk.EmplXRiskTO.LastVisitRecID = dtl.VisitDtlTO.RecID;
                            List<EmployeeXRiskTO> emplRiskList = emplRisk.SearchEmployeeXRisks();

                            foreach (EmployeeXRiskTO riskTO in emplRiskList)
                            {
                                riskTO.LastDatePerformed = dtl.VisitDtlTO.DatePerformed.Date;
                                riskTO.LastScheduleDate = new DateTime();
                                riskTO.ModifiedBy = user;
                                riskTO.ModifiedTime = new DateTime();
                                riskUpdateList.Add(riskTO);
                            }
                        }
                        else
                        {
                            // find risk to update                            
                            emplRisk.EmplXRiskTO.EmployeeID = hdr.VisitHdrTO.EmployeeID;
                            emplRisk.EmplXRiskTO.RiskID = dtl.VisitDtlTO.CheckID;
                            
                            List<EmployeeXRiskTO> emplRiskList = emplRisk.SearchEmployeeXRisks();

                            // update risk last performed date for active risks                            
                            foreach (EmployeeXRiskTO riskTO in emplRiskList)
                            {
                                if (!riskTO.DateEnd.Equals(new DateTime()) && riskTO.DateEnd.Date < DateTime.Now.Date)
                                    continue;

                                riskTO.LastDatePerformed = dtl.VisitDtlTO.DatePerformed.Date;
                                riskTO.LastScheduleDate = new DateTime();
                                riskTO.LastVisitRecID = dtl.VisitDtlTO.RecID;
                                riskTO.ModifiedBy = user;
                                riskTO.ModifiedTime = new DateTime();
                                riskUpdateList.Add(riskTO);
                            }
                        }
                    }

                    // update record and save history
                    dtl.VisitDtlTO.ModifiedBy = user;
                    dtl.VisitDtlTO.ModifiedTime = new DateTime();
                    if (dtl.BeginTransaction())
                    {
                        try
                        {
                            bool saved = true;
                            hdrHist.SetTransaction(dtl.GetTransaction());
                            saved = saved && hdrHist.Save(false);

                            if (saved)
                                saved = saved && dtl.Update(false);

                            if (saved && updateHdrStatus)
                            {
                                hdr.SetTransaction(dtl.GetTransaction());
                                hdr.VisitHdrTO = hdrList[0];
                                hdr.VisitHdrTO.Status = Constants.MedicalCheckVisitStatus.DONE.ToString();
                                hdr.VisitHdrTO.ModifiedBy = user;
                                hdr.VisitHdrTO.ModifiedTime = new DateTime();

                                saved = saved && hdr.Update(false);
                            }

                            if (saved && riskUpdateList.Count > 0)
                            {
                                EmployeeXRisk emplRisk = new EmployeeXRisk(Session[Constants.sessionConnection]);
                                emplRisk.SetTransaction(dtl.GetTransaction());

                                foreach (EmployeeXRiskTO riskTO in riskUpdateList)
                                {
                                    emplRisk.EmplXRiskTO = riskTO;
                                    saved = saved && emplRisk.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (saved)
                            {
                                dtl.CommitTransaction();

                                SelBox.Value = "";

                                GetVisits(getEmployee());

                                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=290";

                                InitializeSelectedRecord();

                                lblError.Text = rm.GetString("recordUpdated", culture);
                            }
                            else
                            {
                                if (dtl.GetTransaction() != null)
                                    dtl.RollbackTransaction();

                                lblError.Text = rm.GetString("recordNotUpdated", culture);
                                setSelection();
                            }
                        }
                        catch
                        {
                            if (dtl.GetTransaction() != null)
                                dtl.RollbackTransaction();

                            lblError.Text = rm.GetString("recordNotUpdated", culture);
                            setSelection();
                        }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVisitsManagementPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVisitsManagementPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVisitsManagementPage).Assembly);

                lblError.Text = "";

                // get ids of records for deletion
                if (SelBox.Value.Trim().Equals(""))
                    lblError.Text = rm.GetString("noRecordToDelete", culture);
                else
                {
                    string recIDs = SelBox.Value.Replace(Constants.delimiter, ',');
                    recIDs = recIDs.Substring(0, recIDs.Length - 1);

                    MedicalCheckVisitDtl dtl = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]);

                    // check if there is completed details that should not be deleted
                    List<MedicalCheckVisitDtlTO> dtlList = dtl.SearchMedicalCheckVisitDetails(recIDs);

                    bool completedFound = false;
                    List<uint> idList = new List<uint>();
                    string visitIDs = "";
                    foreach (MedicalCheckVisitDtlTO visitDtlTO in dtlList)
                    {
                        if (!visitDtlTO.DatePerformed.Equals(new DateTime()) || !visitDtlTO.Result.Trim().Equals(""))
                        {
                            completedFound = true;
                            break;
                        }

                        if (!idList.Contains(visitDtlTO.VisitID))
                       {
                            idList.Add(visitDtlTO.VisitID);
                            visitIDs += visitDtlTO.VisitID.ToString().Trim() + ",";
                        }
                    }

                    if (completedFound)
                    {
                        lblError.Text = rm.GetString("completedVisitsDtlFound", culture);
                        setSelection();
                        return;
                    }

                    if (visitIDs.Length > 0)
                        visitIDs = visitIDs.Substring(0, visitIDs.Length - 1);

                    List<MedicalCheckVisitHdrTO> hdrList = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]).SearchMedicalCheckVisits(visitIDs);
                    List<MedicalCheckVisitHdrHistTO> hdrHistList = new List<MedicalCheckVisitHdrHistTO>();
                    MedicalCheckVisitHdrHist hdrHist = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]);
                    string user = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                    // put all visits to history
                    foreach (MedicalCheckVisitHdrTO hdrTO in hdrList)
                    {
                        MedicalCheckVisitHdrHistTO hdrHistTO = new MedicalCheckVisitHdrHistTO(hdrTO);
                        hdrHistTO.ModifiedBy = user;
                        hdrHistTO.ModifiedTime = new DateTime();

                        foreach (MedicalCheckVisitDtlHistTO dtlTO in hdrHistTO.VisitDetails)
                        {
                            dtlTO.ModifiedBy = user;
                            dtlTO.ModifiedTime = new DateTime();
                        }

                        hdrHistList.Add(hdrHistTO);
                    }                    

                    if (dtl.BeginTransaction())
                    {
                        try
                        {
                            bool deleted = true;

                            hdrHist.SetTransaction(dtl.GetTransaction());
                            foreach (MedicalCheckVisitHdrHistTO histTO in hdrHistList)
                            {
                                hdrHist.VisitHdrHistTO = histTO;
                                deleted = deleted && hdrHist.Save(false);

                                if (!deleted)
                                    break;
                            }

                            if (deleted)
                                deleted = deleted && dtl.Delete(recIDs, false);

                            if (deleted)
                            {
                                MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);
                                hdr.SetTransaction(dtl.GetTransaction());
                                List<MedicalCheckVisitHdrTO> emptyHdrList = hdr.SearchEmptyVisits(visitIDs);

                                foreach (MedicalCheckVisitHdrTO hdrTO in emptyHdrList)
                                {
                                    hdrTO.Status = Constants.MedicalCheckVisitStatus.DELETED.ToString();
                                    if (hdrTO.FlagEmail == Constants.noInt)
                                        hdrTO.FlagEmail = Constants.yesInt;
                                    else
                                    {
                                        hdrTO.FlagEmail = Constants.noInt;
                                        hdrTO.FlagEmailCratedTime = new DateTime();
                                    }
                                    hdrTO.ModifiedBy = user;
                                    hdrTO.ModifiedTime = new DateTime();

                                    hdr.VisitHdrTO = hdrTO;

                                    deleted = deleted && hdr.Update(false);

                                    if (!deleted)
                                        break;
                                }
                            }

                            if (deleted)
                            {
                                dtl.CommitTransaction();

                                SelBox.Value = "";

                                GetVisits(getEmployee());

                                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=290";

                                InitializeSelectedRecord();

                                lblError.Text = rm.GetString("recordsDeleted", culture);
                            }
                            else
                            {
                                if (dtl.GetTransaction() != null)
                                    dtl.RollbackTransaction();

                                lblError.Text = rm.GetString("recordsNotDeleted", culture);
                                setSelection();
                            }
                        }
                        catch
                        {
                            if (dtl.GetTransaction() != null)
                                dtl.RollbackTransaction();

                            lblError.Text = rm.GetString("recordsNotDeleted", culture);
                            setSelection();
                        }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVisitsManagementPage.btnDelete_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVisitsManagementPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVisitsManagementPage.btnPostBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVisitsManagementPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplVisitsManagementPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplVisitsManagementPage.", filterState);
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

                GetVisits(getEmployee());
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

