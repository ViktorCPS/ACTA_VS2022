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
using Util;
using TransferObjects;
using Tittle.Controls;

namespace ACTAWebUI
{
    public partial class ResultPage : System.Web.UI.Page
    {
        const int widthMin = 90;
        const string pageName = "ResultPage";
        const double rowBtnHeight = 14;
        
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

        private int currentPage
        {
            get
            {
                int currPage = 0;                
                if (Session[Constants.sessionResultCurrentPage] != null)
                {
                    if (!int.TryParse(Session[Constants.sessionResultCurrentPage].ToString(), out currPage))
                        currPage = 0;
                }

                return currPage;
            }
            set
            {                
                Session[Constants.sessionResultCurrentPage] = value;
            }
        }

        private int numPages
        {
            get
            {
                int num = 0;
                if (ViewState[Constants.vsResultNumPages] != null)
                {                    
                    if (!int.TryParse(ViewState[Constants.vsResultNumPages].ToString(), out num))
                        num = 0;                    
                }

                return num;
            }
            set
            {
                ViewState[Constants.vsResultNumPages] = value;
            }
        }

        private int rowCount
        {
            get
            {
                int rCount = 0;
                if (ViewState[Constants.vsResultRowCount] != null)
                {                    
                    if (!int.TryParse(ViewState[Constants.vsResultRowCount].ToString(), out rCount))
                        rCount = 0;                    
                }

                return rCount;
            }
            set
            {
                ViewState[Constants.vsResultRowCount] = value;
            }
        }
                        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    setLanguage();
                                        
                    // get result parameters from session
                    InitializeResultParameters();

                    // initialize data grid
                    InitializeDataGrid();

                    if (Session[Constants.sessionSamePage] != null && (bool)Session[Constants.sessionSamePage] && currentPage > 0)
                    {
                        // if page should reload with same page displayed
                        PopulateDataGrid(getFirstPageRow(currentPage), getLastPageRow(currentPage));
                        Session[Constants.sessionSamePage] = null;
                    }
                    else
                        // populate first page
                        PopulateDataGrid(1, Constants.recPerPage);
                }
                else
                {                    
                    // if page is post back, data grids columns and header table definitions are lost for some reason, so reinitialize them before populating data grid
                    InitializeDataGrid();

                    // initiate populating selected page (if post back is initiate through coulmn text cahnging)
                    cbPage_SelectedIndexChanged(this, new EventArgs());                    
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() 
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(ResultPage).Assembly);

                lblGoToPage.Text = rm.GetString("lblGoToPage", culture);
                lblTotal.Text = rm.GetString("lblTotal", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeDataGrid()
        {
            try
            {
                if (Request.QueryString["showSelection"] != null && Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                {
                    resultGrid.Columns.Clear();
                }

                if (Request.QueryString["height"] != null)
                {
                    int height = -1;

                    if (int.TryParse(Request.QueryString["height"].Trim(), out height) && height > 0)
                        resultPanel.Height = height;
                }
                
                if (Session[Constants.sessionHeader] != null && !Session[Constants.sessionHeader].ToString().Trim().Equals("") 
                    && Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals(""))
                {
                    // get header and colums fileds
                    string[] columnNames = Session[Constants.sessionHeader].ToString().Trim().Split(',');
                    string[] columns;
                    string[] colTypes = null;
                    
                    if (Session[Constants.sessionFields].ToString().Contains('|'))
                    {
                        columns = Session[Constants.sessionFields].ToString().Trim().Split('|');
                    }
                    else
                    {
                        columns = Session[Constants.sessionFields].ToString().Trim().Split(',');
                    }

                    if (Session[Constants.sessionColTypes] != null)
                        colTypes = Session[Constants.sessionColTypes].ToString().Trim().Split(',');

                    double width = 0;

                    // if there are column's and header's fileds and they are same length initialize data grid and header table
                    if (columnNames.Length > 0 && columns.Length > 0 && columnNames.Length == columns.Length)
                    {
                        // get column width                        
                        width = (resultGrid.Width.Value - Constants.chbColWidth) / columns.Length;
                        if (width < 0)
                            width = 0;

                        if (width < widthMin)
                        {
                            width = widthMin;
                            resultGrid.Width = (System.Web.UI.WebControls.Unit)(width * columns.Length);
                            resultPanel.Width = (System.Web.UI.WebControls.Unit) (resultGrid.Width.Value + 20);
                            resultPanel.Height = (System.Web.UI.WebControls.Unit)430;
                            if (Request.QueryString["height"] != null)
                            {
                                int height = -1;

                                if (int.TryParse(Request.QueryString["height"].Trim(), out height) && height > 0)
                                    resultPanel.Height = height;
                            }

                            double footerWidth = resultPanel.Width.Value - Constants.standardFooterTableWidth;
                            if (footerWidth > 0)
                            {
                                TableCell footerCell = new TableCell();
                                footerCell.ID = "footerCell";
                                footerCell.Width = new Unit(footerWidth);
                                footerCell.CssClass = "footerCell";
                                footerRow.Cells.Add(footerCell);
                                footerRow.Width = resultPanel.Width;
                                footerTable.Width = resultPanel.Width;
                            }
                            hdrRow.Width = resultPanel.Width;
                            hdrTable.Width = resultPanel.Width;
                        }                        

                        // initialize table to be data grid header
                        // if there are more records and scroll appears, data grid header scrolls down too, so I made table with link buttons above data grid to be header
                        InitializeDataGridHeader(columnNames, columns, width);

                        for (int i = 0; i < columns.Length; i++)
                        {
                            string dataFiled = "";
                            if (columns[i].IndexOf("AS") > 0)
                                // if select is from more then one table and fields are with aliases
                                dataFiled = columns[i].Substring(columns[i].IndexOf("AS") + 3).Trim();
                            else
                                // if select is from one table and there is no aliases
                                dataFiled = columns[i].Trim();

                            // create data grid column and add it to collection                            
                            int colType = -1;
                            if (colTypes != null && colTypes.Length > i)
                            {
                                if (!int.TryParse(colTypes[i].Trim(), out colType))
                                    colType = -1;
                            }

                            if (colType == -1 || colType == (int)Constants.ColumnTypes.TEXT)
                                resultGrid.Columns.Add(CreateBoundColumn(dataFiled, i, width));
                            else
                                resultGrid.Columns.Add(CreateTemplateColumn(dataFiled, i, width, colType));
                        }

                        // do not show data grid header, header table will play that role
                        resultGrid.ShowHeader = false;

                        if (Session[Constants.sessionKey] != null && !Session[Constants.sessionKey].ToString().Trim().Equals(""))
                            resultGrid.DataKeyField = Session[Constants.sessionKey].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void PopulateDataGrid(int firstRow, int lastRow)
        {
            try
            {
                if (Request.QueryString["showSelection"] == null || Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                {                
                    ClientScript.RegisterStartupScript(GetType(), "setSelection", "setSelectedKeys();", true);                    
                }

                ClientScript.RegisterStartupScript(GetType(), "setChange", "setChangedKeys();", true);

                string fields = "";
                string tables = "";
                string filter = "";
                string sortCol = "";
                string sortDir = "";
                List<string> selKeys = new List<string>();

                // get result parameters from session                
                if (Session[Constants.sessionFields] != null)
                    fields = Session[Constants.sessionFields].ToString();                
                if (Session[Constants.sessionTables] != null)
                    tables = Session[Constants.sessionTables].ToString();                
                if (Session[Constants.sessionFilter] != null)
                    filter = Session[Constants.sessionFilter].ToString();                
                if (Session[Constants.sessionSortCol] != null)
                    sortCol = Session[Constants.sessionSortCol].ToString();
                if (Session[Constants.sessionSortDir] != null)
                    sortDir = Session[Constants.sessionSortDir].ToString();
                                
                if (rowCount > 0)
                {
                    // get previous selection if exists
                    //if (Session[Constants.sessionSelectedKeys] != null && Session[Constants.sessionSelectedKeys] is List<string>)
                    //{
                    //    selKeys = (List<string>)Session[Constants.sessionSelectedKeys];
                    //}                    

                    // if there are records for selected filter, get records between firstRow and lastRow
                    DataTable resultTable = new DataTable();
                    if (Session[Constants.sessionDataTableList] == null)
                        resultTable = new Result(Session[Constants.sessionConnection]).SearchResultTable(fields, tables, filter, sortCol, sortDir, firstRow, lastRow);
                    else if (Session[Constants.sessionDataTableList] is List<List<object>> && Session[Constants.sessionDataTableColumns] != null 
                        && Session[Constants.sessionDataTableColumns] is List<DataColumn>)
                    {
                        // create resultTable
                        List<DataColumn> resultColumns = (List<DataColumn>)Session[Constants.sessionDataTableColumns];
                        foreach (DataColumn column in resultColumns)
                        {
                            resultTable.Columns.Add(column.ColumnName, column.DataType);
                        }

                        List<List<object>> table = (List<List<object>>)Session[Constants.sessionDataTableList];

                        // sort table
                        // sort list of objects
                        int sortOrder = Constants.sortAsc;
                        if (sortDir.Trim().ToUpper().Equals(Constants.sortDESC.Trim().ToUpper()))
                            sortOrder = Constants.sortDesc;

                        int sortField = -1;

                        for (int i = 0; i < ((List<DataColumn>)Session[Constants.sessionDataTableColumns]).Count; i++)
                        {
                            if (((List<DataColumn>)Session[Constants.sessionDataTableColumns])[i].ColumnName.Trim().ToUpper().Equals(sortCol.Trim().ToUpper()))
                            {
                                sortField = i;
                                break;
                            }
                        }

                        if (sortField >= 0 && sortField < ((List<DataColumn>)Session[Constants.sessionDataTableColumns]).Count)
                        {
                            table.Sort(new ListSort(sortOrder, sortField));
                        }
                        
                        for (int i = firstRow - 1; i >= 0 && i < table.Count && i < lastRow; i++)
                        {
                            DataRow row = resultTable.NewRow();
                            for (int j = 0; j < table[i].Count && j < resultColumns.Count; j++)
                            {
                                row[resultColumns[j].ColumnName] = table[i][j];
                            }

                            resultTable.Rows.Add(row);
                            resultTable.AcceptChanges();
                        }                            
                    }

                    resultGrid.DataSource = resultTable; // if data source is view, check box for row selection is not visible
                    resultGrid.DataBind();

                    // calculate page shown
                    currentPage = firstRow / Constants.recPerPage + 1;
                }
                else
                    currentPage = 0;
                
                // set visibility of prev/next
                lbtnPrev.Enabled = true;
                lbtnNext.Enabled = true;

                if (currentPage == 0)
                {
                    lbtnPrev.Enabled = false;
                    lbtnNext.Enabled = false;
                }
                else
                {
                    if (currentPage == 1)
                        lbtnPrev.Enabled = false;
                    if (currentPage == numPages)
                        lbtnNext.Enabled = false;
                }

                // select corresponding page
                cbPage.SelectedValue = currentPage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void clearSelection()
        {
            try
            {
                // hdr select/deselect all was checked from unknown reasons on page post back so I make it unchecked here
                ((HtmlInputCheckBox)hdrRow.Cells[0].FindControl("hdrChbSel")).Checked = false;
                selectedKeys.Value = "";                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populatePages()
        {
            try
            {
                cbPage.Items.Clear();

                if (rowCount <= 0)
                {
                    // disable selecting of pages if there are no records to display
                    cbPage.Enabled = false;
                    numPages = 0;
                }
                else
                {
                    // calculate number of pages
                    numPages = rowCount / Constants.recPerPage;

                    if (rowCount % Constants.recPerPage > 0)
                        numPages++;

                    // populate combo with pages plus page/numofpages row on page
                    for (int i = 1; i <= numPages; i++)
                    {
                        ListItem item = new ListItem(i.ToString().Trim() + " (" + i.ToString().Trim() + "/" + numPages.ToString().Trim() + ")", i.ToString().Trim());
                        cbPage.Items.Add(item);
                    }

                    cbPage.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeResultParameters()
        {
            try
            {
                if (Session[Constants.sessionDataTableList] == null)
                {
                    string tables = "";
                    string filter = "";

                    // get result parameters from session
                    if (Session[Constants.sessionTables] != null)
                        tables = Session[Constants.sessionTables].ToString();
                    if (Session[Constants.sessionFilter] != null)
                        filter = Session[Constants.sessionFilter].ToString();

                    // calculate number of records for selected filter
                    if (!tables.Trim().Equals("") && !filter.Trim().Equals(""))
                        rowCount = new Result(Session[Constants.sessionConnection]).SearchResultCount(tables, filter);
                    else
                        rowCount = 0;
                }
                else if (Session[Constants.sessionDataTableList] is List<List<object>>)
                {
                    rowCount = ((List<List<object>>)Session[Constants.sessionDataTableList]).Count;                    
                }

                lblTotalCount.Text = rowCount.ToString().Trim();

                // populate page combo
                populatePages();
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

                string format = getFormat(colIndex);

                if (!format.Trim().Equals(""))
                    column.DataFormatString = format;

                return column;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TemplateColumn CreateTemplateColumn(string dataFieldValue, int colIndex, double width, int type)
        {
            try
            {                
                string key = "";
                if (Session[Constants.sessionKey] != null)
                    key = Session[Constants.sessionKey].ToString().Trim();
                // create data grid column
                // I could not find how to change grid lines color
                // put border line arround each column item and it would look like grid lines are of that color
                TemplateColumn column = new TemplateColumn();                
                column.ItemTemplate = new DataGridTemplate(type, dataFieldValue, colIndex, key, width);
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

        private string getFormat(int colIndex)
        {
            try
            {
                if (Session[Constants.sessionFieldsFormating] != null && Session[Constants.sessionFieldsFormating] is Dictionary<int, int>)
                {
                    Dictionary<int, int> fieldsFormating = (Dictionary<int, int>)Session[Constants.sessionFieldsFormating];

                    if (fieldsFormating.ContainsKey(colIndex))
                    {
                        if (fieldsFormating[colIndex] == (int)Constants.FormatTypes.DateFormat)
                            return "{0:" + Constants.dateFormat.Trim() + "}";
                        if (fieldsFormating[colIndex] == (int)Constants.FormatTypes.DateTimeFormat)
                            return "{0:" + Constants.dateFormat.Trim() + " " + Constants.timeFormat.Trim() + "}";
                        if (fieldsFormating[colIndex] == (int)Constants.FormatTypes.YearFormat)
                            return "{0:" + Constants.yearFormat.Trim() + "}";
                        if (fieldsFormating[colIndex] == (int)Constants.FormatTypes.DoubleFormat)
                            return "{0:" + Constants.doubleFormat.Trim() + "}";
                        if (fieldsFormating[colIndex] == (int)Constants.FormatTypes.TimeFormat)
                            return "{0:" + Constants.timeFormat.Trim() + "}";
                    }
                }
                
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getFormatedValue(int index, string data)
        {
            try
            {
                if (Session[Constants.sessionFieldsFormatedValues] != null && Session[Constants.sessionFieldsFormatedValues] is Dictionary<int, Dictionary<string, string>>)
                {
                    Dictionary<int, Dictionary<string, string>> fieldsFormatedValues = (Dictionary<int, Dictionary<string, string>>)Session[Constants.sessionFieldsFormatedValues];

                    if (!fieldsFormatedValues.ContainsKey(index))
                    {
                        if (data.Equals(new DateTime().ToString(Constants.dateFormat)) || data.Equals(new DateTime().ToString(Constants.dateFormat + " " + Constants.timeFormat)))
                            return Constants.notApplicable;
                        else
                            return data.Replace(Environment.NewLine, "<br/>");
                    }
                    if (fieldsFormatedValues[index].Count <= 0)
                        return data.Replace(Environment.NewLine, "<br/>");
                    if (!fieldsFormatedValues[index].ContainsKey(data))
                        return data.Replace(Environment.NewLine, "<br/>");

                    return fieldsFormatedValues[index][data].Trim();
                }
                else if (data.Equals(new DateTime().ToString(Constants.dateFormat)) || data.Equals(new DateTime().ToString(Constants.dateFormat + " " + Constants.timeFormat)))
                    return Constants.notApplicable;
                else
                    return data.Replace(Environment.NewLine, "<br/>");                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object getItemColor(string key)
        {
            try
            {
                if (Session[Constants.sessionItemsColors] != null && Session[Constants.sessionItemsColors] is Dictionary<string, Color>
                    && ((Dictionary<string, Color>)Session[Constants.sessionItemsColors]).ContainsKey(key))
                    return ((Dictionary<string, Color>)Session[Constants.sessionItemsColors])[key];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeDataGridHeader(string[] colNames, string[] columns, double width)
        {
            try
            {
                hdrRow.Cells.Clear();

                if (Request.QueryString["showSelection"] == null || Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                {
                    // add first column for select/deselect all
                    // create checkbox to be header field
                    HtmlInputCheckBox hdrChb = new HtmlInputCheckBox();
                    hdrChb.ID = "hdrChbSel";
                    hdrChb.Checked = false;
                    hdrChb.Attributes.Add("onclick", "return selectAll(this);");

                    // create table cell for check box header field
                    TableCell hdrSelCell = new TableCell();
                    hdrSelCell.Width = Unit.Parse(Constants.chbColWidth.ToString().Trim());
                    hdrSelCell.HorizontalAlign = HorizontalAlign.Center;
                    hdrSelCell.CssClass = "hdrListCell";
                    hdrSelCell.Controls.Add(hdrChb);

                    hdrRow.Cells.Add(hdrSelCell);
                }

                for (int i = 0; i < colNames.Length; i++ )
                {
                    // create link button to be header field
                    LinkButton hdrLBtn = new LinkButton();
                    hdrLBtn.CssClass = "hdrListTitle";
                    hdrLBtn.Text = colNames[i].Trim();
                    string id = columns[i].Trim();
                    if (id.IndexOf("AS") >= 0)
                        id = id.Substring(0, id.IndexOf("AS"));                    
                    hdrLBtn.ID = id;
                    hdrLBtn.Width = Unit.Parse((width - 5).ToString());
                    hdrLBtn.Click +=new EventHandler(hdrLBtn_Click);

                    // create table cell for header field
                    TableCell hdrCell = new TableCell();
                    hdrCell.Width = new Unit(width);                    
                    hdrCell.CssClass = "hdrListCell";                    
                    hdrCell.Controls.Add(hdrLBtn);

                    hdrRow.Cells.Add(hdrCell);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtnPrev_Click(Object sender, EventArgs e)
        {
            try
            {
                // get previous page
                PopulateDataGrid(getFirstPageRow(currentPage - 1), getLastPageRow(currentPage - 1));

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.lbtnPrev_Click(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void lbtnNext_Click(Object sender, EventArgs e)
        {
            try
            {
                // get next page
                PopulateDataGrid(getFirstPageRow(currentPage + 1), getLastPageRow(currentPage + 1));

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.lbtnNext_Click(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbPage_SelectedIndexChanged(Object sender, EventArgs e)
        {
            try
            {
                // get selected page
                PopulateDataGrid(getFirstPageRow(int.Parse(cbPage.SelectedValue.Trim())), getLastPageRow(int.Parse(cbPage.SelectedValue.Trim())));

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.cbPage_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private int getFirstPageRow(int pageNum)
        {
            try
            {
                // get first row on selected page
                return (pageNum -1) * Constants.recPerPage + 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getLastPageRow(int pageNum)
        {
            try
            {
                // get last page row on selected page
                int lastRow = pageNum * Constants.recPerPage;

                if (lastRow > rowCount)
                    lastRow = rowCount;

                return lastRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void hdrLBtn_Click(Object sender, EventArgs e)
        {
            try
            {
                // link button click is simulating sorting list
                // set sort column and sort direction
                string oldSortCol = "";
                string sortDir = "";

                if (Session[Constants.sessionSortCol] != null)
                    oldSortCol = Session[Constants.sessionSortCol].ToString().Trim();
                if (Session[Constants.sessionSortDir] != null)
                    sortDir = Session[Constants.sessionSortDir].ToString().Trim();

                string sortCol = ((LinkButton)sender).ID.Trim();
                Session[Constants.sessionSortCol] = sortCol;

                if (oldSortCol.Trim().Equals(sortCol.Trim()))
                {
                    if (sortDir.Trim().ToUpper().Equals(Constants.sortASC.Trim().ToUpper()))
                        sortDir = Constants.sortDESC.Trim();
                    else
                        sortDir = Constants.sortASC.Trim();
                }
                else
                {
                    sortDir = Constants.sortASC.Trim();
                }

                Session[Constants.sessionSortDir] = sortDir;
                
                // get first page for new sort criteria
                PopulateDataGrid(1, Constants.recPerPage);

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.hdrLBtn_Click(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void resultGrid_Unload(Object sender, EventArgs e)
        {
            try
            {
                Session[Constants.sessionSelectedKeys] = null;
                Session[Constants.sessionChangedKeys] = null;
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.resultGrid_Unload(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
                
        protected void resultGrid_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            try
            {
                List<string> selKeys = new List<string>();
                Dictionary<string, Dictionary<int, string>> chgKeys = new Dictionary<string, Dictionary<int, string>>();

                // first try to get selection from page hidden field
                if (!selectedKeys.Value.Trim().Equals(""))
                    selKeys = getSelectionValues(selectedKeys);
                else if (Session[Constants.sessionSelectedKeys] != null && Session[Constants.sessionSelectedKeys] is List<string>)
                    selKeys = (List<string>)Session[Constants.sessionSelectedKeys];

                // first try to get changes from page hidden field
                if (!changedKeys.Value.Trim().Equals(""))
                    chgKeys = getChangedValues(changedKeys);
                else if (Session[Constants.sessionChangedKeys] != null && Session[Constants.sessionChangedKeys] is Dictionary<string, Dictionary<int, string>>)
                    chgKeys = (Dictionary<string, Dictionary<int, string>>)Session[Constants.sessionChangedKeys];

                // get column types
                string[] colTypes = null;
                if (Session[Constants.sessionColTypes] != null)
                    colTypes = Session[Constants.sessionColTypes].ToString().Trim().Split(',');
                
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (resultGrid.DataKeys != null && resultGrid.DataKeys.Count > e.Item.ItemIndex)
                    {
                        // set item color
                        object itemColor = getItemColor(resultGrid.DataKeys[e.Item.ItemIndex].ToString());
                        if (itemColor != null && itemColor is Color)
                            e.Item.BackColor = (Color)itemColor;

                        //// set item tooltip
                        //string itemTooltip = getItemTooltip(resultGrid.DataKeys[e.Item.ItemIndex].ToString());
                        //if (!itemTooltip.Equals(""))
                        //    e.Item.ToolTip = itemTooltip.Replace("<br/>", Environment.NewLine);
                    }

                    if (Request.QueryString["showSelection"] == null || Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                    {
                        HtmlInputCheckBox chb = (HtmlInputCheckBox)e.Item.Cells[0].FindControl("chbSel");

                        if (resultGrid.DataKeys != null && resultGrid.DataKeys.Count > e.Item.ItemIndex)
                        {
                            chb.Value = resultGrid.DataKeys[e.Item.ItemIndex].ToString();

                            if (selKeys.Contains(resultGrid.DataKeys[e.Item.ItemIndex].ToString()))
                            {
                                chb.Checked = true;
                                e.Item.ForeColor = e.Item.BackColor;
                                e.Item.BackColor = ColorTranslator.FromHtml(Constants.selItemColor);
                                //selectedKeys.Value += chb.Value + Constants.delimiter.ToString().Trim();
                            }
                            else
                                chb.Checked = false;
                        }
                    }                                        

                    int startColIndex = 0;
                    if (Request.QueryString["showSelection"] == null || Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                        startColIndex = 1;

                    for (int i = startColIndex; i < e.Item.Cells.Count; i++)
                    {
                        int index = startColIndex == 0 ? i : i - 1;
                        if (colTypes != null && colTypes.Length > index && !colTypes[index].Trim().Equals(((int)Constants.ColumnTypes.TEXT).ToString().Trim()))
                        {
                            if (colTypes[index].Trim().Equals(((int)Constants.ColumnTypes.EDIT_TEXT).ToString().Trim())
                                || colTypes[index].Trim().Equals(((int)Constants.ColumnTypes.CHANGE_TEXT).ToString().Trim()))
                            {
                                string[] columns;
                                if (Session[Constants.sessionFields].ToString().Contains('|'))
                                {
                                    columns = Session[Constants.sessionFields].ToString().Trim().Split('|');
                                }
                                else
                                {
                                    columns = Session[Constants.sessionFields].ToString().Trim().Split(',');
                                }

                                string dataFiled = "";
                                if (columns.Length > index)
                                {
                                    if (columns[index].IndexOf("AS") > 0)
                                        // if select is from more then one table and fields are with aliases
                                        dataFiled = columns[index].Substring(columns[index].IndexOf("AS") + 3).Trim();
                                    else
                                        // if select is from one table and there is no aliases
                                        dataFiled = columns[index].Trim();
                                }

                                Control ctrl = e.Item.Cells[i].FindControl(dataFiled);
                                if (ctrl != null && ctrl is TextBox)
                                {
                                    // check if original value is changed
                                    string rowID = "";
                                    if (resultGrid.DataKeys != null && resultGrid.DataKeys.Count > e.Item.ItemIndex)
                                        rowID = resultGrid.DataKeys[e.Item.ItemIndex].ToString();
                                    if (chgKeys.ContainsKey(rowID) && chgKeys[rowID].ContainsKey(index))
                                        ((TextBox)ctrl).Text = getFormatedValue(i, chgKeys[rowID][index]);
                                    else
                                    {
                                        string format = getFormat(index);
                                        string text = ((TextBox)ctrl).Text;
                                        if (!format.Trim().Equals(""))
                                            text = string.Format(format, DataBinder.Eval(e.Item.DataItem, dataFiled));
                                        ((TextBox)ctrl).Text = getFormatedValue(i, text);
                                    }
                                }

                                if (ctrl != null && ctrl is Label)
                                {
                                    // check if original value is changed
                                    string rowID = "";
                                    if (resultGrid.DataKeys != null && resultGrid.DataKeys.Count > e.Item.ItemIndex)
                                        rowID = resultGrid.DataKeys[e.Item.ItemIndex].ToString();
                                    if (chgKeys.ContainsKey(rowID) && chgKeys[rowID].ContainsKey(index))
                                        ((Label)ctrl).Text = getFormatedValue(i, chgKeys[rowID][index]);
                                    else
                                    {
                                        string format = getFormat(index);
                                        string text = ((Label)ctrl).Text;
                                        if (!format.Trim().Equals(""))
                                            text = string.Format(format, DataBinder.Eval(e.Item.DataItem, dataFiled));
                                        ((Label)ctrl).Text = getFormatedValue(i, text);
                                    }
                                }
                            }
                            
                            continue;
                        }

                        e.Item.Cells[i].Text = getFormatedValue(i, e.Item.Cells[i].Text);
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
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ResultPage.resultGrid_ItemDataBound(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/ResultPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setMCSchedulingColor(DataGridItem item)
        {
            try
            {                                
                // get column types
                string[] colTypes = null;
                if (Session[Constants.sessionColTypes] != null)
                    colTypes = Session[Constants.sessionColTypes].ToString().Trim().Split(',');
                
                // if page is shown from MCSchedulingPage, item is red if termin is out of day interval
                if (colTypes != null && colTypes.Contains<string>(((int)Constants.ColumnTypes.VISIT_HIST).ToString()))
                {
                    DateTime schDate = new DateTime();
                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();

                    try
                    {
                        int startColIndex = 0;
                        if (Request.QueryString["showSelection"] == null || Request.QueryString["showSelection"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                            startColIndex = 1;

                        // get columns
                        string[] columns;
                        if (Session[Constants.sessionFields].ToString().Contains('|'))
                        {
                            columns = Session[Constants.sessionFields].ToString().Trim().Split('|');
                        }
                        else
                        {
                            columns = Session[Constants.sessionFields].ToString().Trim().Split(',');
                        }

                        // get scheduled date and time
                        List<int> dateIndexes = new List<int>();
                        dateIndexes.Add(Constants.MCSchedulingDColIndex);
                        dateIndexes.Add(Constants.MCSchedulingMColIndex);
                        dateIndexes.Add(Constants.MCSchedulingYColIndex);
                        dateIndexes.Add(Constants.MCSchedulingTerminColIndex);

                        string day = "";
                        string month = "";
                        string year = "";
                        string time = "";
                        string dataFiled = "";
                        foreach (int index in dateIndexes)
                        {
                            if (columns.Length > index)
                            {
                                if (columns[index].IndexOf("AS") > 0)
                                    // if select is from more then one table and fields are with aliases
                                    dataFiled = columns[index].Substring(columns[index].IndexOf("AS") + 3).Trim();
                                else
                                    // if select is from one table and there is no aliases
                                    dataFiled = columns[index].Trim();
                            }

                            string cellText = "";
                            Control ctrl = item.Cells[index + startColIndex].FindControl(dataFiled);
                            if (ctrl != null && ctrl is TextBox)
                                cellText = ((TextBox)ctrl).Text.Trim();
                            else
                                cellText = item.Cells[index + startColIndex].Text.Trim();

                            if (index == Constants.MCSchedulingDColIndex)
                                day = cellText;
                            else if (index == Constants.MCSchedulingMColIndex)
                                month = cellText;
                            else if (index == Constants.MCSchedulingYColIndex)
                                year = cellText;
                            else if (index == Constants.MCSchedulingTerminColIndex)
                                time = cellText;
                        }

                        DateTime schDay = CommonWeb.Misc.createDate(day.Trim().PadLeft(2, '0') + "." + month.Trim().PadLeft(2, '0') + "." + year.Trim() + ".");

                        DateTime schTime = CommonWeb.Misc.createTime(time.Trim());

                        schDate = new DateTime(schDay.Year, schDay.Month, schDay.Day, schTime.Hour, schTime.Minute, 0);

                        // get intervals
                        string intervalsString = item.Cells[startColIndex + Constants.MCSchedulingIntervalsColIndex].Text.Trim();
                        int indexNewLine = intervalsString.IndexOf("<br/>");
                        while (indexNewLine >= 0 || !intervalsString.Trim().Equals(""))
                        {
                            string interval = "";

                            if (indexNewLine >= 0)
                                interval = intervalsString.Substring(0, indexNewLine);
                            else
                                interval = intervalsString;

                            string[] startEnd = interval.Split('-');
                            if (startEnd.Length == 2)
                            {
                                DateTime startTime = CommonWeb.Misc.createTime(startEnd[0]);
                                DateTime endTime = CommonWeb.Misc.createTime(startEnd[1]);

                                WorkTimeIntervalTO dayInterval = new WorkTimeIntervalTO();
                                dayInterval.StartTime = startTime;
                                dayInterval.EndTime = endTime;
                                dayIntervals.Add(dayInterval);
                            }

                            if (indexNewLine >= 0)
                                intervalsString = intervalsString.Substring(indexNewLine + 5);
                            else
                                intervalsString = "";

                            indexNewLine = intervalsString.IndexOf("<br>");
                        }
                    }
                    catch
                    { }

                    if (!schDate.Equals(new DateTime()) && !schDate.Equals(Constants.dateTimeNullValue()))
                    {
                        bool intervalFound = false;

                        foreach (WorkTimeIntervalTO interval in dayIntervals)
                        {
                            if (interval.StartTime.TimeOfDay <= schDate.TimeOfDay && interval.EndTime.TimeOfDay > schDate.TimeOfDay)
                            {
                                intervalFound = true;
                                break;
                            }
                        }

                        if (!intervalFound)
                        {
                            item.BackColor = Color.Pink;

                            List<string> selKeys = new List<string>();

                            // first try to get selection from page hidden field
                            if (!selectedKeys.Value.Trim().Equals(""))
                                selKeys = getSelectionValues(selectedKeys);
                            else if (Session[Constants.sessionSelectedKeys] != null && Session[Constants.sessionSelectedKeys] is List<string>)
                                selKeys = (List<string>)Session[Constants.sessionSelectedKeys];

                            // if record is selected set it to selected color
                            if (resultGrid.DataKeys != null && resultGrid.DataKeys.Count > item.ItemIndex && selKeys.Contains(resultGrid.DataKeys[item.ItemIndex].ToString()))
                            {
                                item.ForeColor = item.BackColor;
                                item.BackColor = ColorTranslator.FromHtml(Constants.selItemColor);
                            }
                        }
                    }
                }
            }
            catch
            { }
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

        private Dictionary<string, Dictionary<int, string>> getChangedValues(HtmlInputHidden chgBox)
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> changedKeys = new Dictionary<string, Dictionary<int, string>>();

                string[] changedRows = chgBox.Value.Trim().Split(Constants.rowDelimiter);

                foreach (string row in changedRows)
                {
                    string[] changedValues = row.Trim().Split(Constants.delimiter);

                    if (changedValues.Length == 3)
                    {
                        if (!changedKeys.ContainsKey(changedValues[0]))
                            changedKeys.Add(changedValues[0], new Dictionary<int, string>());

                        int colIndex = -1;
                        if (!int.TryParse(changedValues[1], out colIndex))
                            colIndex = -1;

                        if (colIndex >= 0)
                        {
                            if (!changedKeys[changedValues[0]].ContainsKey(colIndex))
                                changedKeys[changedValues[0]].Add(colIndex, "");

                            changedKeys[changedValues[0]][colIndex] = changedValues[2];
                        }
                    }
                }

                return changedKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class ListSort : IComparer<List<object>>
        {
            private int compOrder;
            private int compField;
            
            public ListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;                
            }

            public int Compare(List<object> x, List<object> y)
            {
                object o1 = null;
                object o2 = null;

                if (x.Count == y.Count && compField >= 0 && compField < x.Count)
                {
                    if (compOrder == Constants.sortAsc)
                    {
                        o1 = x[compField];
                        o2 = y[compField];
                    }
                    else
                    {
                        o1 = y[compField];
                        o2 = x[compField];
                    }

                    if (o1.GetType().Equals(o2.GetType()))
                    {
                        Type compType = o1.GetType();

                        if (compType.Equals(typeof(string)))
                            return o1.ToString().Trim().CompareTo(o2.ToString().Trim());
                        else if (compType.Equals(typeof(int)))
                            return ((int)o1).CompareTo((int)o2);
                        else if (compType.Equals(typeof(double)))
                            return ((double)o1).CompareTo((double)o2);
                        else if (compType.Equals(typeof(decimal)))
                            return ((decimal)o1).CompareTo((decimal)o2);
                        else if (compType.Equals(typeof(float)))
                            return ((float)o1).CompareTo((float)o2);
                        else if (compType.Equals(typeof(DateTime)))
                            return ((DateTime)o1).CompareTo((DateTime)o2);                        
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }          
        }

        // if there is template columns, unique key in each row (DataKey) should be put in Session (Session[Constants.sessionKey]) before navigating ResultPage!!!
        public class DataGridTemplate : ITemplate
        {
            double colWidth;            
            int templateType;
            string columnName;
            int colIndex;
            string rowUnique;            
            
            public DataGridTemplate(int type, string colname, int index, string key, double width)                
            {                
                templateType = type;
                columnName = colname;
                colIndex = index;
                rowUnique = key;                
                colWidth = width;                
            }

            public void InstantiateIn(System.Web.UI.Control container)
            {
                switch (templateType)
                {
                    case (int)Constants.ColumnTypes.EDIT_TEXT:
                        TextBox tb = new TextBox();
                        tb.Width = new Unit(colWidth - 5);
                        tb.ID = columnName;
                        tb.Text = "";
                        tb.Visible = true;
                        tb.DataBinding += new EventHandler(tb_DataBinding);                        
                        tb.CssClass = "resultGrid";                        
                        container.Controls.Add(tb);
                        break;
                    case (int)Constants.ColumnTypes.VISIT_HIST:
                        ImageButton btn = new ImageButton();
                        btn.ID = columnName;
                        btn.Width = new Unit(rowBtnHeight);
                        btn.Height = new Unit(rowBtnHeight);
                        btn.ImageUrl = "/ACTAWeb/CommonWeb/images/warning.png";
                        btn.Visible = true;
                        btn.DataBinding += new EventHandler(btnVisitHist_DataBinding);
                        btn.CssClass = "resultGrid";
                        container.Controls.Add(btn);
                        break;
                    case (int)Constants.ColumnTypes.CHANGE_TEXT:
                        Label lbl = new Label();
                        lbl.Width = new Unit(colWidth - 5);
                        lbl.ID = columnName;
                        lbl.Text = "";
                        lbl.Visible = true;
                        lbl.CssClass = "resultGrid";
                        lbl.DataBinding += new EventHandler(lbl_DataBinding);                        
                        container.Controls.Add(lbl);
                        break;
                    case (int)Constants.ColumnTypes.TOOLTIP:
                        Notes note = new Notes();
                        note.ID = columnName;
                        note.Visible = true;
                        note.Sticky = true;
                        double noteW = 0;
                        if (!double.TryParse(note.NotesWidth.Trim(), out noteW))
                            noteW = 0;
                        int startX = (int)(colIndex * colWidth - noteW - 5);
                        if (startX <= 0)
                            startX = 1;
                        note.Fix = "[" + startX.ToString().Trim() + ", 1]";
                        note.Temp = 5000;
                        note.EnableImage = "timeicon.gif";
                        note.DataBinding += new EventHandler(note_DataBinding);
                        note.CssClass = "resultGrid";
                        container.Controls.Add(note);
                        break;
                    default:
                        Literal lc = new Literal();
                        lc.ID = columnName;
                        lc.Text = "";
                        lc.Visible = true;
                        lc.DataBinding += new EventHandler(lc_DataBinding);
                        container.Controls.Add(lc);
                        break;
                }
            }

            private void tb_DataBinding(object sender, System.EventArgs e)
            {
                try
                {
                    TextBox tb;
                    tb = (TextBox)sender;
                    DataGridItem container = (DataGridItem)tb.NamingContainer;
                    tb.Text += DataBinder.Eval(container.DataItem, tb.ID);
                    string id = DataBinder.Eval(container.DataItem, rowUnique).ToString();
                    tb.Attributes.Add("onchange", "return changedRow(" + id.Trim() + ", " + colIndex.ToString().Trim() + ", this);");                    
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            private void lbl_DataBinding(object sender, System.EventArgs e)
            {
                try
                {
                    Label lbl;
                    lbl = (Label)sender;
                    DataGridItem container = (DataGridItem)lbl.NamingContainer;
                    lbl.Text += DataBinder.Eval(container.DataItem, lbl.ID);                    
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            private void btnVisitHist_DataBinding(object sender, System.EventArgs e)
            {
                try
                {                    
                    ImageButton btn;
                    btn = (ImageButton)sender;
                    DataGridItem container = (DataGridItem)btn.NamingContainer;
                    btn.Visible = DataBinder.Eval(container.DataItem, btn.ID).ToString().Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                    if (btn.Visible)
                    {
                        string id = DataBinder.Eval(container.DataItem, rowUnique).ToString();
                        btn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                        btn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");                        
                        btn.Attributes.Add("onclick", "return visitHistPreview('" + id.Trim() + "');");
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            private void note_DataBinding(object sender, System.EventArgs e)
            {
                try
                {
                    // set item tooltip
                    Notes note;
                    note = (Notes)sender;
                    DataGridItem container = (DataGridItem)note.NamingContainer;
                    note.Text += DataBinder.Eval(container.DataItem, note.ID);
                    note.Visible = !note.Text.Equals("");
                    //string id = DataBinder.Eval(container.DataItem, rowUnique).ToString();
                    //note.Visible = gridTooltips.ContainsKey(id) && !gridTooltips[id].Trim().Equals("");                        
                    //if (note.Visible)
                    //    note.Text = gridTooltips[id].Trim();                    
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            private void lc_DataBinding(object sender, System.EventArgs e)
            {
                try
                {
                    Literal lc;
                    lc = (Literal)sender;
                    DataGridItem container = (DataGridItem)lc.NamingContainer;
                    lc.Text += DataBinder.Eval(container.DataItem, lc.ID);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }
    }
}
