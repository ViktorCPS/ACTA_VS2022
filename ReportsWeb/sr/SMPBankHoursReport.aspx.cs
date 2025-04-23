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
using System.Collections.Generic;
using TransferObjects;
using Util;
using Microsoft.Reporting.WebForms;

namespace ReportsWeb.sr
{
    public partial class SMPBankHoursReport : System.Web.UI.Page
    {
        Dictionary<EmployeeTO, List<EmployeeCounterValueTO>> listEmplCounter = new Dictionary<EmployeeTO, List<EmployeeCounterValueTO>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["ListEmployeesCounters"] != null)
                        listEmplCounter = (Dictionary<EmployeeTO,List<EmployeeCounterValueTO>>)Session["ListEmployeesCounters"];
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID");
                    dt.Columns.Add("Ime i prezime");
                    dt.Columns.Add("Banka sati");
                    
                    foreach (KeyValuePair<EmployeeTO,List<EmployeeCounterValueTO>> item in listEmplCounter)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = item.Key.EmployeeID;
                        row["Ime i prezime"] = item.Key.FirstAndLastName;
                        int minutes = item.Value[4].Value;
                        int hrs = minutes / 60;
                        int min = minutes % 60;
                        string bnkhrs = hrs.ToString() + "h " + min.ToString() + "min";
                        row["Banka sati"] = bnkhrs;
                        dt.Rows.Add(row);
                        dt.AcceptChanges();
                    }
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0],dt));
                    ReportViewer1.LocalReport.Refresh();
                }
            }
            catch (Exception ex) {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in SMPBankHoursReport.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
