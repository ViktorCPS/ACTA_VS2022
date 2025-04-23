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
    public partial class MCEmployeeSchedulesPage : System.Web.UI.Page
    {
        const string pageName = "MCEmployeeSchedulesPage";
        const string newLine = "<br>";

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

                InitializeSchGrid();

                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    btnSearch.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    setLanguage();

                    if (Request.QueryString["emplID"] != null)
                    {
                        DateTime monthFirst = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;

                        tbFromDate.Text = monthFirst.ToString(Constants.dateFormat);
                        tbToDate.Text = monthFirst.AddMonths(2).AddDays(-1).ToString(Constants.dateFormat);

                        btnSearch_Click(this, new EventArgs());
                    }
                    else
                        searchTable.Visible = false;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in MCEmployeeSchedulesPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/MCEmployeeSchedulesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCEmployeeSchedulesPage).Assembly);

                lblError.Text = "";
                                
                DateTime start = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                if (start.Equals(new DateTime()))                
                    lblError.Text = rm.GetString("noDateStart", culture);

                DateTime end = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                if (end.Equals(new DateTime()))                
                    lblError.Text = rm.GetString("noDateEnd", culture);

                if (start.Date > end.Date)                
                    lblError.Text = rm.GetString("startAfterEnd", culture);

                string emplID = "";

                if (Request.QueryString["emplID"] != null)
                    emplID = Request.QueryString["emplID"].Trim();

                if (emplID.Length <= 0)                
                    lblError.Text = rm.GetString("noSelectedEmployee", culture);

                DataTable schTable = GetSchedules(emplID, start, end);

                schGrid.DataSource = schTable;
                schGrid.DataBind();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplRiskManagementDataPage.btnSearch_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplRiskManagementDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private DataTable GetSchedules(string employee, DateTime start, DateTime end)
        {
            try
            {
                DataTable resultTable = new DataTable();
                                
                resultTable.Columns.Add("date", typeof(string));
                resultTable.Columns.Add("schema", typeof(string));
                resultTable.Columns.Add("intervals", typeof(string));

                if (employee.Length <= 0 || start.Equals(new DateTime()) || end.Equals(new DateTime()))
                    return resultTable;

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(employee, start, end, null);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                foreach (int emplID in emplSchedules.Keys)
                {
                    DateTime currDate = start.Date;
                    while (currDate.Date <= end.Date)
                    {
                        DataRow resultRow = resultTable.NewRow();
                        resultRow["date"] = currDate.Date.ToString(Constants.dateFormat);

                        List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(currDate.Date, emplSchedules[emplID], schemas);
                        string intervalsString = "";
                        string schema = "";

                        if (dayIntervals.Count > 0)
                        {
                            if (schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                                schema = dayIntervals[0].TimeSchemaID.ToString() + " - " + schemas[dayIntervals[0].TimeSchemaID].Name.Trim();

                            foreach (WorkTimeIntervalTO interval in dayIntervals)
                            {
                                intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + newLine;
                            }
                            if (intervalsString.Length > 0)
                                intervalsString = intervalsString.Substring(0, intervalsString.Length - 2);
                        }

                        resultRow["schema"] = schema;
                        resultRow["intervals"] = intervalsString;
                        resultTable.Rows.Add(resultRow);

                        currDate = currDate.Date.AddDays(1);
                    }
                }

                return resultTable;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCEmployeeSchedulesPage).Assembly);

                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                btnSearch.Text = rm.GetString("btnSearch", culture);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSchGridHeader(double width)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(MCEmployeeSchedulesPage).Assembly);

                hdrRow.Cells.Clear();

                List<string> colNames = new List<string>();
                colNames.Add(rm.GetString("hdrDate", culture));
                colNames.Add(rm.GetString("hdrShift", culture));
                colNames.Add(rm.GetString("hdrInterval", culture));

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

        private void InitializeSchGrid()
        {
            try
            {
                // get column width
                double width = schGrid.Width.Value / 4;

                InitializeSchGridHeader(width);

                schGrid.Columns.Add(CreateBoundColumn("date", 0, width));
                schGrid.Columns.Add(CreateBoundColumn("schema", 1, width));
                schGrid.Columns.Add(CreateBoundColumn("intervals", 2, width));

                schGrid.ShowHeader = false;
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "MCEmployeeSchedulesPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "MCEmployeeSchedulesPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
