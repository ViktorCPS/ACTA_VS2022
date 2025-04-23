using System;
using System.Collections;
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
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Drawing;

using Util;
using Common;
using TransferObjects;

namespace ACTAWebUI
{
    public partial class SmallResultPage : System.Web.UI.Page
    {
        private const string vsColWidth = "SmallResultPageColWidth";
        private double width
        {
            get
            {
                double _width = 0;
                if (ViewState[vsColWidth] != null)
                {
                    if (!double.TryParse(ViewState[vsColWidth].ToString(), out _width))
                        _width = 0;
                }

                return _width;
            }
            set
            {
                ViewState[vsColWidth] = value;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    setLanguage();

                    // initialize data grid
                    InitializeDataGrid();

                    if (Session[Constants.sessionSamePage] != null && (bool)Session[Constants.sessionSamePage])
                    {
                        // if page should reload with same page displayed
                        PopulateDataGrid();
                        Session[Constants.sessionSamePage] = null;
                    }
                    else
                        // populate first page
                        PopulateDataGrid();
                }
                else
                {
                    // if page is post back, data grids columns and header table definitions are lost for some reason, so reinitialize them before populating data grid
                    InitializeDataGrid();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in SmallResultPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim()
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
                if (Session[Constants.sessionHeader] != null && !Session[Constants.sessionHeader].ToString().Trim().Equals("")
                    && Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals(""))
                {
                    // get header and colums fileds
                    string[] columnNames = Session[Constants.sessionHeader].ToString().Trim().Split(',');
                    string[] columns;
                    if (Session[Constants.sessionFields].ToString().Contains('|'))
                    {
                        columns = Session[Constants.sessionFields].ToString().Trim().Split('|');
                    }
                    else
                    {
                        columns = Session[Constants.sessionFields].ToString().Trim().Split(',');
                    }

                    // if there are column's and header's fileds and they are same length initialize data grid and header table
                    if (columnNames.Length > 0 && columns.Length > 0 && columnNames.Length == columns.Length)
                    {
                        // get column width
                        int widthMin = 15;

                        width = (resultGrid.Width.Value - Constants.chbColWidth) / columns.Length;
                        if (width < widthMin)
                        {
                            width = widthMin;
                            resultGrid.Width = (System.Web.UI.WebControls.Unit)(width * columns.Length);
                            resultPanel.Width = (System.Web.UI.WebControls.Unit)resultGrid.Width.Value;
                            resultPanel.Height = (System.Web.UI.WebControls.Unit)430;
                        }


                        // initialize table to be data grid header
                        // if there are more records and scroll appears, data grid header scrrols down too, so I made table with link buttons above data grid to be header
                        InitializeDataGridHeader(columnNames, columns);

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
                            resultGrid.Columns.Add(CreateBoundColumn(dataFiled, i));
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
        private void PopulateDataGrid()
        {
            try
            {
                // cleen row selection
              //  clearSelection();

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

                
                    // get previous selection if exists
                    if (Session[Constants.sessionSelectedKeys] != null && Session[Constants.sessionSelectedKeys] is List<string>)
                    {
                        selKeys = (List<string>)Session[Constants.sessionSelectedKeys];
                    }
                    if (filter != "")
                    {
                        // if there are records for selected filter, get records between firstRow and lastRow
                        DataTable resultTable = new Result(Session[Constants.sessionConnection]).SearchResultTable(fields, tables, filter, sortCol, sortDir);
                        resultGrid.DataSource = resultTable; // if data source is view, check box for row selection is not visible
                        resultGrid.DataBind();
                    }

                Session[Constants.sessionSelectedKeys] = null;
              

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private BoundColumn CreateBoundColumn(string dataFieldValue, int colIndex)
        {
            try
            {
                // create data grid column
                // I could not find how to change grid lines color
                // put border line arround each column item and it would look like grid lines are of that color
                BoundColumn column = new BoundColumn();
                column.DataField = dataFieldValue;
                column.ItemStyle.Width = new Unit(width);
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

        private void InitializeDataGridHeader(string[] colNames, string[] columns)
        {
            try
            {
                hdrRow.Cells.Clear();

                // add first column for select/deselect all
                // create checkbox to be header field
                //HtmlInputCheckBox hdrChb = new HtmlInputCheckBox();
                //hdrChb.ID = "hdrChbSel";
                //hdrChb.Checked = false;
                //hdrChb.Attributes.Add("onclick", "return selectAll(this);");

                // create table cell for check box header field
                //TableCell hdrSelCell = new TableCell();
                //hdrSelCell.Width = Unit.Parse(Constants.chbColWidth.ToString().Trim());
                //hdrSelCell.HorizontalAlign = HorizontalAlign.Center;
                //hdrSelCell.CssClass = "hdrListCell";
                //hdrSelCell.Controls.Add(hdrChb);

               // hdrRow.Cells.Add(hdrSelCell);

                for (int i = 0; i < colNames.Length; i++)
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
                    hdrLBtn.Click += new EventHandler(hdrLBtn_Click);

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
                PopulateDataGrid();
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
        protected void resultGrid_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            try
            {
                List<string> selKeys = new List<string>();
                if (Session[Constants.sessionSelectedKeys] != null && Session[Constants.sessionSelectedKeys] is List<string>)
                    selKeys = (List<string>)Session[Constants.sessionSelectedKeys];

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlInputCheckBox chb = (HtmlInputCheckBox)e.Item.Cells[0].FindControl("chbSel");

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

                    for (int i = 1; i < e.Item.Cells.Count; i++)
                    {
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
        private string getFormatedValue(int index, string data)
        {
            try
            {
                if (Session[Constants.sessionFieldsFormatedValues] != null && Session[Constants.sessionFieldsFormatedValues] is Dictionary<int, Dictionary<string, string>>)
                {
                    Dictionary<int, Dictionary<string, string>> fieldsFormatedValues = (Dictionary<int, Dictionary<string, string>>)Session[Constants.sessionFieldsFormatedValues];

                    if (!fieldsFormatedValues.ContainsKey(index))
                        return data;
                    if (fieldsFormatedValues[index].Count <= 0)
                        return data;
                    if (!fieldsFormatedValues[index].ContainsKey(data))
                        return data;

                    return fieldsFormatedValues[index][data].Trim();
                }
                else
                    return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
