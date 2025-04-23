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
    public partial class MCVisitHistoryPage : System.Web.UI.Page
    {
        const string pageName = "MCVisitHistoryPage";

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
                // parameter in request query string is visit_id
                if (!IsPostBack)
                {
                    btnClose.Attributes.Add("onclick", "return closeWindow();");
                                        
                    setLanguage();
                }

                InitializeHdrGrid();
                InitializeDtlGrid();                    
                InitializeSelectedRecord();

                btnClose.Focus();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitHistoryPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitHistoryPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);

                lblVisitDtls.Text = rm.GetString("lblVisitDtls", culture);                
                lblTitle.Text = rm.GetString("lblVisitHist", culture);

                btnClose.Text = rm.GetString("btnClose", culture);                
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);
                
                if (selectedKeys.Value.Trim().Equals(""))
                {
                    // set to initial values
                    dtlGrid.DataSource = GetVisits(new DataTable());
                    dtlGrid.DataBind();                    
                }
                else
                {
                    // set to last selected record values                    
                    uint recID = 0;
                    // remove last separator, and get last selected row
                    string selectedValues = selectedKeys.Value.Substring(0, selectedKeys.Value.Length - 1);
                    if (uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID) && recID > 0)
                    {
                        // take dtl rekords                        
                        dtlGrid.DataSource = GetVisits(new MedicalCheckVisitDtlHist(Session[Constants.sessionConnection]).SearchMedicalCheckVisitDetailsHistory(recID.ToString()));
                        dtlGrid.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeHdrGrid()
        {
            try
            {
                // get column width
                double width = (hdrGrid.Width.Value - 20) / 6;
                
                InitializeHdrGridHeader(width);

                hdrGrid.Columns.Add(CreateBoundColumn("rec_id", 0, width));
                hdrGrid.Columns.Add(CreateBoundColumn("schedule_date", 1, width));
                hdrGrid.Columns.Add(CreateBoundColumn("point", 2, width));
                hdrGrid.Columns.Add(CreateBoundColumn("status", 3, width));
                hdrGrid.Columns.Add(CreateBoundColumn("user_name", 4, width));
                hdrGrid.Columns.Add(CreateBoundColumn("mod_time", 5, width));

                hdrGrid.ShowHeader = false;

                hdrGrid.DataSource = GetVisitsHdrs();
                hdrGrid.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeDtlGrid()
        {
            try
            {
                // get column width
                double width = dtlGrid.Width.Value / 4;

                InitializeDtlGridHeader(width);                
                
                dtlGrid.Columns.Add(CreateBoundColumn("type", 0, width));
                dtlGrid.Columns.Add(CreateBoundColumn("risk", 1, width));
                dtlGrid.Columns.Add(CreateBoundColumn("result", 2, width));
                dtlGrid.Columns.Add(CreateBoundColumn("date", 3, width));

                dtlGrid.ShowHeader = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeHdrGridHeader(double width)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);

                hdrRow.Cells.Clear();                
                
                List<string> colNames = new List<string>();
                colNames.Add(rm.GetString("hdrID", culture));
                colNames.Add(rm.GetString("hdrDateScheduled", culture));
                colNames.Add(rm.GetString("hdrAmbulance", culture));
                colNames.Add(rm.GetString("hdrStatus", culture));
                colNames.Add(rm.GetString("hdrModifyBy", culture));
                colNames.Add(rm.GetString("hdrModifiedTime", culture));

                foreach (string col in colNames)
                {
                    // create link button to be header field
                    Label hdrLbl = new Label();
                    hdrLbl.CssClass = "hdrListTitle";
                    hdrLbl.Text = col.Trim();
                    hdrLbl.Width = Unit.Parse((width - 5).ToString());

                    // create table cell for header field
                    TableCell hdrCell = new TableCell();
                    hdrCell.Width = new Unit(width);
                    hdrCell.CssClass = "hdrListCell";
                    hdrCell.Controls.Add(hdrLbl);

                    hdrRow.Cells.Add(hdrCell);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeDtlGridHeader(double width)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);
                
                dtlRow.Cells.Clear();

                List<string> colNames = new List<string>();
                colNames.Add(rm.GetString("hdrType", culture));
                colNames.Add(rm.GetString("hdrRiskVaccine", culture));
                colNames.Add(rm.GetString("hdrResult", culture));
                colNames.Add(rm.GetString("hdrTimePerformed", culture));
                
                foreach (string col in colNames)
                {
                    // create link button to be header field
                    Label dtlLbl = new Label();
                    dtlLbl.CssClass = "hdrListTitle";
                    dtlLbl.Text = col.Trim();
                    dtlLbl.Width = Unit.Parse((width - 5).ToString());                    

                    // create table cell for header field
                    TableCell dtlCell = new TableCell();
                    dtlCell.Width = new Unit(width);
                    dtlCell.CssClass = "hdrListCell";
                    dtlCell.Controls.Add(dtlLbl);

                    dtlRow.Cells.Add(dtlCell);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void hdrGrid_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            try
            {
                List<string> selKeys = new List<string>();
                
                // first try to get selection from page hidden field                
                selKeys = getSelectionValues(selectedKeys);                

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.Cells.Count > 1)
                    {
                        HtmlInputCheckBox chb = (HtmlInputCheckBox)e.Item.Cells[0].FindControl("chbSel");

                        chb.Value = e.Item.Cells[1].Text.Trim();

                        if (selKeys.Contains(chb.Value))
                        {
                            chb.Checked = true;
                            e.Item.ForeColor = e.Item.BackColor;
                            e.Item.BackColor = ColorTranslator.FromHtml(Constants.selItemColor);
                        }
                        else
                            chb.Checked = false;
                    }                    
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCVisitHistoryPage.hdrGrid_ItemDataBound(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCVisitHistoryPage.aspx&Header=" + Constants.falseValue.Trim(), false);                                        
                }
                catch (System.Threading.ThreadAbortException) { }
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

        private BoundColumn CreateBoundColumn(string dataFieldValue, int colIndex, double width)
        {
            try
            {
                // create data grid column
                // I could not find how to change grid lines color
                // put border line arround each column item and it would look like grid lines are of that color
                BoundColumn column = new BoundColumn();
                column.DataField = dataFieldValue;
                column.ItemStyle.Width = new Unit(width - 2 * Constants.colBorderWidth);
                column.ItemStyle.BorderColor = ColorTranslator.FromHtml(Constants.colBorderColor);
                column.ItemStyle.BorderStyle = BorderStyle.Solid;
                column.ItemStyle.BorderWidth = Unit.Parse(Constants.colBorderWidth.ToString().Trim());
                column.Visible = true;
                column.ItemStyle.Wrap = true;

                return column;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetVisitsHdrs()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);

                DataTable table = new MedicalCheckVisitHdrHist(Session[Constants.sessionConnection]).SearchMedicalCheckVisitHeadersHistory(getVisitID());

                DataTable resultTable = new DataTable();
                
                resultTable.Columns.Add("rec_id", typeof(string));
                resultTable.Columns.Add("schedule_date", typeof(string));
                resultTable.Columns.Add("point", typeof(string));
                resultTable.Columns.Add("status", typeof(string));
                resultTable.Columns.Add("user_name", typeof(string));
                resultTable.Columns.Add("mod_time", typeof(string));

                if (table.Rows.Count > 0)
                {                    
                    foreach (DataRow row in table.Rows)
                    {
                        DataRow resultRow = resultTable.NewRow();
                                                
                        if (row["rec_id"] != DBNull.Value)
                        {
                            resultRow["rec_id"] = row["rec_id"].ToString();
                        }
                        if (row["schedule_date"] != DBNull.Value)
                        {
                            resultRow["schedule_date"] = DateTime.Parse(row["schedule_date"].ToString().Trim()).ToString(Constants.dateFormat + " " + Constants.timeFormat);
                        }
                        if (row["point"] != DBNull.Value)
                        {
                            resultRow["point"] = row["point"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            resultRow["status"] = row["status"].ToString().Trim();
                        }
                        if (row["user_name"] != DBNull.Value)
                        {
                            resultRow["user_name"] = row["user_name"].ToString().Trim();
                        }
                        if (row["mod_time"] != DBNull.Value)
                        {
                            resultRow["mod_time"] = DateTime.Parse(row["mod_time"].ToString().Trim()).ToString(Constants.dateFormat + " " + Constants.timeFormat);
                        }

                        resultTable.Rows.Add(resultRow);
                    }
                }

                return resultTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetVisits(DataTable table)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCVisitHistoryPage).Assembly);

                DataTable resultTable = new DataTable();
                                
                resultTable.Columns.Add("type", typeof(string));
                resultTable.Columns.Add("risk", typeof(string));
                resultTable.Columns.Add("result", typeof(string));
                resultTable.Columns.Add("date", typeof(string));

                if (table.Rows.Count > 0)
                {
                    Dictionary<int, RiskTO> riskDict = new Risk(Session[Constants.sessionConnection]).SearchRisksDictionary();
                    Dictionary<int, VaccineTO> vacDict = new Vaccine(Session[Constants.sessionConnection]).SearchVaccinesDictionary();

                    foreach (DataRow row in table.Rows)
                    {
                        DataRow resultRow = resultTable.NewRow();

                        string type = "";
                        if (row["type"] != DBNull.Value)
                        {
                            type = row["type"].ToString().Trim().ToUpper();
                            resultRow["type"] = rm.GetString(type, culture);
                        }
                        if (row["check_id"] != DBNull.Value)
                        {
                            int id = Int32.Parse(row["check_id"].ToString().Trim());

                            if (type == Constants.VisitType.R.ToString() && riskDict.ContainsKey(id))
                                resultRow["risk"] = riskDict[id].RiskCode.Trim();
                            else if (type == Constants.VisitType.V.ToString() && vacDict.ContainsKey(id))
                                resultRow["risk"] = vacDict[id].VaccineType.Trim();
                        }
                        if (row["result"] != DBNull.Value)
                        {
                            resultRow["result"] = row["result"].ToString().Trim();
                        }
                        if (row["date_performed"] != DBNull.Value)
                        {
                            resultRow["date"] = DateTime.Parse(row["date_performed"].ToString().Trim()).ToString(Constants.dateFormat + " " + Constants.timeFormat);
                        }

                        resultTable.Rows.Add(resultRow);
                    }
                }

                return resultTable;
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "MCVisitHistoryPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "MCVisitHistoryPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private uint getVisitID()
        {
            try
            {                
                uint id = 0;

                if (Request.QueryString["visitID"] != null)
                {
                    if (!uint.TryParse(Request.QueryString["visitID"], out id))
                        id = 0;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

