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

using CommonWeb;
using Common;
using Util;
using TransferObjects;

namespace ACTAWebUI
{
    public partial class PassesSearch : System.Web.UI.Page
    {
        private static CultureInfo culture;
        private static ResourceManager rm;
        private static string dateTimeFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ACTAWebUI.Resource", typeof(PassesSearch).Assembly);
                    DateTimeFormatInfo dateTimeformat = new CultureInfo("en-US", true).DateTimeFormat;
                    dateTimeFormat = dateTimeformat.SortableDateTimePattern;

                    lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFrom');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbTo');");
                    //btnReport.Attributes.Add("onclick", "return getAllKeys('" + reportSelBox.ID.Trim() + "', '" + rm.GetString("noSelected", culture) + "', '" + resultIframe.ID.Trim() + "');");
                    //btnReport.Attributes.Add("onclick", "return getAllGridValues('" + reportSelBox.ID.Trim() + "', '" + rm.GetString("noReportData", culture) + "', '" + resultIframe.ID.Trim() + "');");
                    
                    setLanguage();
                    populateTypes();
                    populateLocation();
                    populateDirection();
                    populateGate();
                    
                    lblError.Text = "";
                    tbFrom.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    tbTo.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    btnFromDate.Focus();

                    // if returned from result page, reload selected filter state
                    Dictionary<string, string> filterState = new Dictionary<string, string>();
                    if (Session["FilterState"] != null)
                        filterState = (Dictionary<string, string>)Session["FilterState"];
                    CommonWeb.Misc.LoadState(this, "PassesSearch.", filterState);

                    // put sql parameters for result page in session
                    InitializeSQLParameters();
                    resultIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesSearch.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                lbtnExit.Text = rm.GetString("lbtnExit", culture);
                lbtnMenu.Text = rm.GetString("lbtnMenu", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblDateFormat.Text = rm.GetString("lblDateFormat", culture);
                lblDateExample.Text = rm.GetString("lblDateExample", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblLocation.Text = rm.GetString("lblLocation", culture);
                lblDirection.Text = rm.GetString("lblDirection", culture);
                lblGate.Text = rm.GetString("lblGate", culture);
                lblPassesSearch.Text = rm.GetString("lblPassesSearch", culture);
                if (Session["LoggedInUser"] != null)
                    lblLoggedInUser.Text = Session["LoggedInUser"].ToString().Trim();
                else
                    lblLoggedInUser.Text = "";

                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateTypes()
        {
            try
            {
                PassType pt = new PassType();
                pt.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> ptArray = pt.Search();
                ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));
                               
                cbType.DataSource = ptArray;
                cbType.DataTextField = "Description";
                cbType.DataValueField = "PassTypeID";

                cbType.DataBind(); // bez ovoga se ne poveze lista objekata sa drop down listom i nista se ne prikazuje

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateDirection()
        {
            try
            {
                cbDirection.Items.Clear();

                cbDirection.Items.Add(rm.GetString("all", culture));
                cbDirection.Items.Add(Constants.DirectionIn);
                cbDirection.Items.Add(Constants.DirectionOut);
                //cbDirection.Items.Add(Constants.DirectionInOut);

                cbDirection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        private void populateLocation()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive;
                List<LocationTO> locArray = loc.Search();
                locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

                cbLocation.DataSource = locArray;
                cbLocation.DataTextField = "Name";
                cbLocation.DataValueField = "LocationID";
                
                cbLocation.DataBind();
                
                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateGate()
        {
            try
            {
                List<GateTO> gateArray = new Gate().Search();
                gateArray.Insert(0, new GateTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), new DateTime(), -1, -1));

                cbGate.DataSource = gateArray;
                cbGate.DataTextField = "Name";
                cbGate.DataValueField = "GateID";

                cbGate.DataBind();

                cbGate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtnMenu_Click(Object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/ACTAWeb/Default.aspx", false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesSearch.lbtnMenu_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSearch_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";
                DateTime from = CommonWeb.Misc.createDate(tbFrom.Text.Trim());
                DateTime to = CommonWeb.Misc.createDate(tbTo.Text.Trim());
                int passTypeID = -1;
                int locID = -1;
                int gateID = -1;                
                string filter = "ps.employee_id = '" + Session["UserID"].ToString().Trim().ToUpper() + "' AND ";

                if (!tbFrom.Text.Trim().Equals("") && from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFrom.Focus();
                    from = new DateTime();
                    return;
                }

                if (!tbTo.Text.Trim().Equals("") && to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbTo.Focus();
                    to = new DateTime();
                    return;
                }

                if (!from.Equals(new DateTime()) && !(to.Equals(new DateTime())) && from.Date > to.Date)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    tbFrom.Focus();
                    return;
                }

                if (!from.Equals(new DateTime()))
                {
                    if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                        filter += "ps.event_time >= CONVERT(datetime,'" + from.Date.ToString("yyyy-MM-dd") + "', 111) AND ";
                    else if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                        filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.Date.ToString(dateTimeFormat, ci).Trim() + "' AND ";
                }

                if (!to.Equals(new DateTime()))
                {
                    if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                        filter += "ps.event_time < CONVERT(datetime,'" + to.AddDays(1).Date.ToString("yyyy-MM-dd") + "', 111) AND ";
                    else if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                        filter += "DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.AddDays(1).Date.ToString(dateTimeFormat, ci).Trim() + "' AND ";
                }
                
                if (cbType.SelectedIndex > 0)
                {
                    if (!int.TryParse(cbType.SelectedValue.Trim(), out passTypeID))
                    {
                        lblError.Text = rm.GetString("invalidPassType", culture);
                        cbType.Focus();
                        passTypeID = -1;
                        return;
                    }
                    else
                    {
                        filter += "ps.pass_type_id = '" + passTypeID.ToString().Trim().ToUpper() + "' AND ";
                    }
                }

                if (cbLocation.SelectedIndex > 0)
                {
                    if (!int.TryParse(cbLocation.SelectedValue.Trim(), out locID))
                    {
                        lblError.Text = rm.GetString("invalidLocation", culture);
                        cbLocation.Focus();
                        locID = -1;
                        return;
                    }
                    else
                    {
                        filter += "ps.location_id = '" + locID.ToString().Trim().ToUpper() + "' AND ";
                    }
                }

                if (cbGate.SelectedIndex > 0)
                {
                    if (!int.TryParse(cbGate.SelectedValue.Trim(), out gateID))
                    {
                        lblError.Text = rm.GetString("invalidGate", culture);
                        cbGate.Focus();
                        gateID = -1;
                        return;
                    }
                    else
                    {
                        filter += "ps.gate_id = '" + gateID.ToString().Trim().ToUpper() + "' AND ";
                    }
                }

                if (cbDirection.SelectedIndex > 0)
                {
                    filter += "RTRIM(LTRIM(UPPER(ps.direction))) LIKE N'" + cbDirection.Text.Trim().ToUpper() + "' AND ";
                }

                filter += "pt.pass_type_id = ps.pass_type_id AND loc.location_id = ps.location_id AND ps.gate_id = g.gate_id";

                // put sql statement required data in session
                Session["filter"] = filter;
                Session["sortCol"] = "ps.event_time";
                Session["sortDir"] = Constants.sortASC;

                // save selected filter state
                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "PassesSearch.", new Dictionary<string, string>());
                                
                //Response.Redirect("/ACTAWeb/ACTAWebUI/ResultPage.aspx?Filter=/ACTAWeb/ACTAWebUI/PassesSearch.aspx&Title=" + rm.GetString("passes", culture), false);
                resultIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx";
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesSearch.btnSearch_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnClear_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSelItems.Text = "";

                tbFrom.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                tbTo.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                cbType.SelectedIndex = 0;
                cbLocation.SelectedIndex = 0;
                cbGate.SelectedIndex = 0;
                cbDirection.SelectedIndex = 0;

                ClearSessionValues();

                btnFromDate.Focus();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesSearch.btnClear_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void ClearSessionValues()
        {
            try
            {
                // every session value is set to default value of corresponding type                
                if (Session["filter"] != null)
                    Session["filter"] = "";
                if (Session["sortCol"] != null)
                    Session["sortCol"] = "";
                if (Session["sortDir"] != null)
                    Session["sortDir"] = "";
                if (Session["FilterState"] != null)
                    Session["FilterState"] = new Dictionary<string, string>();                    
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
                Session["header"] = "ID," + rm.GetString("hdrDirection", culture) + "," + rm.GetString("hdrEventTime", culture) + "," + rm.GetString("hdrPassType", culture) + ","
                + rm.GetString("hdrLocation", culture) + "," + rm.GetString("hdrGate", culture) + "," + rm.GetString("hdrWH", culture) + ","
                + rm.GetString("hdrProcessed", culture) + "," + rm.GetString("hdrMM", culture);
                Session["fields"] = "ps.pass_id AS id, ps.direction AS direction, ps.event_time AS event_time, pt.description AS pass_desc, loc.name AS loc_name, g.name AS g_name, ps.is_wrk_hrs AS is_w_h, "
                + "ps.pair_gen_used AS processed, ps.manual_created AS m_manipulated";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(2, (int)Constants.FormatTypes.DateTimeFormat);
                Session["fieldsFormating"] = formating;
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, string> formatValues = new Dictionary<string, string>();
                formatValues.Add("0", rm.GetString("no", culture));
                formatValues.Add("1", rm.GetString("yes", culture));
                values.Add(7, formatValues);
                values.Add(8, formatValues);
                values.Add(9, formatValues);
                Session["fieldsFormatedValues"] = values;
                Session["tables"] = "passes ps, pass_types pt, locations loc, gates g";
                Session["key"] = "id";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void clearSelection(HtmlInputHidden selBox)
        {
            try
            {
                selBox.Value = "";
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

        // create dictionaty, key is row number, value iz list of strings representing row data.
        private Dictionary<int, List<string>> getReportRecordsValues(HtmlInputHidden selBox)
        {
            try
            {
                Dictionary<int, List<string>> reportData = new Dictionary<int, List<string>>();
                
                string[] resultRecords = selBox.Value.Trim().Split(Constants.rowDelimiter);

                int rowNum = 0;
                foreach (string rec in resultRecords)
                {
                    if (!rec.Trim().Equals(""))
                    {
                        string[] record = rec.Split(Constants.delimiter);
                        List<string> reportRecord = new List<string>();
                        
                        // do not get last string, it is empty strng after delimiter that does not belong to record row
                        for (int i = 0; i < record.Length - 1; i++)
                        {
                            reportRecord.Add(record[i].Trim());                            
                        }

                        if (reportRecord.Count > 0)
                        {
                            reportData.Add(rowNum, reportRecord);
                            rowNum++;
                        }
                    }
                }               

                return reportData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                int rowCount = 0;
                DataTable passes = null;

                if (Session["fields"] != null && !Session["fields"].ToString().Trim().Equals("")
                    && Session["tables"] != null && !Session["tables"].ToString().Trim().Equals("")
                    && Session["filter"] != null && !Session["filter"].ToString().Trim().Equals("")
                    && Session["sortCol"] != null && !Session["sortCol"].ToString().Trim().Equals("")
                    && Session["sortDir"] != null && !Session["sortDir"].ToString().Trim().Equals(""))
                {
                    Result result = new Result();
                    rowCount = result.SearchResultCount(Session["tables"].ToString().Trim(), Session["filter"].ToString().Trim());
                    
                    //Dictionary<int, List<string>> reportRecords = getReportRecordsValues(reportSelBox);
                    
                    if (rowCount > 0)
                    {
                        // get all passes for search criteria for report
                        passes = new Result().SearchResultTable(Session["fields"].ToString().Trim(), Session["tables"].ToString().Trim(), Session["filter"].ToString().Trim(),
                        Session["sortCol"].ToString().Trim(), Session["sortDir"].ToString().Trim(), 1, rowCount);

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_passes");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("event_time", typeof(System.DateTime));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("pass_id", typeof(int));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("direction", typeof(System.String));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("is_wrk_hrs", typeof(System.String));
                        tableCR.Columns.Add("location", typeof(System.String));
                        tableCR.Columns.Add("pair_gen_used", typeof(System.String));
                        tableCR.Columns.Add("manual_created", typeof(System.String));
                        tableCR.Columns.Add("gate_id", typeof(System.String));

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
                                row["pass_id"] = pass["id"].ToString().Trim();
                            row["last_name"] = lblLoggedInUser.Text.Substring(0, lblLoggedInUser.Text.IndexOf(' '));
                            row["first_name"] = lblLoggedInUser.Text.Substring(lblLoggedInUser.Text.IndexOf(' ') + 1);
                            if (pass["direction"] != DBNull.Value)
                                row["direction"] = pass["direction"].ToString().Trim();
                            if (pass["event_time"] != DBNull.Value)
                                row["event_time"] = pass["event_time"].ToString().Trim();
                            if (pass["pass_desc"] != DBNull.Value)
                                row["pass_type"] = pass["pass_desc"].ToString().Trim();
                            if (pass["loc_name"] != DBNull.Value)
                                row["location"] = pass["loc_name"].ToString().Trim();
                            if (pass["g_name"] != DBNull.Value)
                                row["gate_id"] = pass["g_name"].ToString().Trim();
                            if (pass["is_w_h"] != DBNull.Value)
                            {
                                if (pass["is_w_h"].ToString().Trim().Equals(((int)Constants.IsWrkCount.IsCounter).ToString()))
                                {
                                    row["is_wrk_hrs"] = rm.GetString("yes", culture);
                                }
                                else
                                {
                                    row["is_wrk_hrs"] = rm.GetString("no", culture);
                                }
                            }
                            else
                                row["is_wrk_hrs"] = "";
                            if (pass["processed"] != DBNull.Value)
                            {
                                if (pass["processed"].ToString().Trim().Equals(((int)Constants.PairGenUsed.Used).ToString()))
                                {
                                    row["pair_gen_used"] = rm.GetString("yes", culture);
                                }
                                else
                                {
                                    row["pair_gen_used"] = rm.GetString("no", culture);
                                }
                            }
                            else
                                row["pair_gen_used"] = "";
                            if (pass["m_manipulated"] != DBNull.Value)
                            {
                                if (pass["m_manipulated"].ToString().Trim().Equals(((int)Constants.recordCreated.Manualy).ToString()))
                                {
                                    row["manual_created"] = rm.GetString("yes", culture);
                                }
                                else
                                {
                                    row["manual_created"] = rm.GetString("no", culture);
                                }
                            }
                            else
                                row["manual_created"] = "";                            
                            
                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {                            
                            lblError.Text = rm.GetString("noReportData", culture);
                            return;
                        }

                        string selPassType = "*";
                        string selDirection = "*";
                        string selLocation = "*";
                        string selGate = "*";

                        if (cbType.SelectedIndex > 0)
                            selPassType = cbType.SelectedItem.Text;
                        if (cbDirection.SelectedIndex > 0)
                            selDirection = cbDirection.SelectedItem.Text;
                        if (cbLocation.SelectedIndex > 0)
                            selLocation = cbLocation.SelectedItem.Text;
                        if (cbGate.SelectedIndex > 0)
                            selGate = cbGate.SelectedItem.Text;

                        Session["passesSelPassType"] = selPassType;
                        Session["passesSelDirection"] = selDirection;
                        Session["passesSelLocation"] = selLocation;
                        Session["passesSelGate"] = selGate;
                        Session["passesDS"] = dataSetCR;
                        Session["passesFrom"] = tbFrom.Text;
                        Session["passesTo"] = tbTo.Text;

                        Session["reportName"] = rm.GetString("lblPassesReport", culture);
                        string reportURL = "";
                        if (NotificationController.GetLanguage().Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/PassesReportClassic_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/PassesReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx&Report=" + reportURL.Trim(), false);
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

                Session["samePage"] = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesSearch.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/PassesSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}

