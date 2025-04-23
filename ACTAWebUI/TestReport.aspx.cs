using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Util;
using Common;
using TransferObjects;
using System.Data;
using System.IO;
using System.Net;
using System.Globalization;
using System.Resources;

namespace ACTAWebUI
{
    public partial class TestReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DebugLog log = new DebugLog(Constants.logFilePath + "LogReport.txt");
                log.writeLog("Start" + " " + DateTime.Now.ToString("HH:mm:sss"));


                int rowCount = 0;
                DataTable ioPairs = null;
                log.writeLog(rowCount.ToString());

                if (Session[Constants.sessionFields] != null && !Session[Constants.sessionFields].ToString().Trim().Equals("")
                    && Session[Constants.sessionTables] != null && !Session[Constants.sessionTables].ToString().Trim().Equals("")
                    && Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortCol] != null && !Session[Constants.sessionSortCol].ToString().Trim().Equals("")
                    && Session[Constants.sessionSortDir] != null && !Session[Constants.sessionSortDir].ToString().Trim().Equals(""))
                {
                    Result result = new Result(Session[Constants.sessionConnection]);
                    rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());
                    log.writeLog(rowCount.ToString());
                    if (rowCount > 0)
                    {
                        log.writeLog(rowCount.ToString()+DateTime.Now.ToString("HH:mm:ss"));
                        // get all passes for search criteria for report
         
                        ioPairs = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim(),
                        Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);
                        log.writeLog(rowCount.ToString()+DateTime.Now.ToString("HH:mm:ss"));
                        List<string> listMonth = new List<string>();
                        string emplIDs = Session["emplIDs"].ToString();
                       
                        log.writeLog(ioPairs.Rows.Count+DateTime.Now.ToString("HH:mm:ss"));
                        DateTime fromDate = (DateTime)Session[Constants.sessionFromDate];
                        DateTime toDate1 = (DateTime)Session[Constants.sessionToDate];
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplIDs, fromDate, toDate1, null);
                        log.writeLog(rowCount.ToString() + DateTime.Now.ToString("HH:mm:ss"));
                        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();
                      
                        log.writeLog(rowCount.ToString() + DateTime.Now.ToString("HH:mm:ss"));
                        ApplUserTO user = (ApplUserTO)Session[Constants.sessionLogInUser];
                        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_DetaiedDataReport" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\";
                        filePath += "DetailedDataReport.xls";
                        log.writeLog(filePath);

                        string Pathh = Directory.GetParent(filePath).FullName;
                        if (!Directory.Exists(Pathh))
                        {
                            Directory.CreateDirectory(Pathh);
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        //DELETE OLD FOLDERS AND FILES FROM TEMP
                        string tempFolder = Constants.logFilePath + "Temp";
                        string[] dirs = Directory.GetDirectories(tempFolder);
                        foreach (string direct in dirs)
                        {
                            DateTime creation = Directory.GetCreationTime(direct);
                            TimeSpan create = creation.AddHours(2).TimeOfDay;

                            if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                                Directory.Delete(direct, true);
                        }

                        CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                        ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TestReport).Assembly);

                        StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);

                        string[] columns = new string[20];

                        columns[0] = rm.GetString("hdrEmployeeID", culture);
                        columns[1] = rm.GetString("hdrEmployee", culture);
                        columns[2] = rm.GetString("hdrShift", culture);
                        columns[3] = rm.GetString("hdrCycleDay", culture);
                        columns[4] = rm.GetString("hdrInterval", culture);
                        columns[5] = rm.GetString("hdrWeek", culture);
                        columns[6] = rm.GetString("hdrYear", culture);
                        columns[7] = rm.GetString("hdrMonth", culture);
                        columns[8] = rm.GetString("hdrDay", culture);
                        columns[9] = rm.GetString("hdrCostCenterCode", culture);
                        columns[10] = rm.GetString("hdrCostCenterName", culture);
                        columns[11] = rm.GetString("hdrWorkgroup", culture);
                        columns[12] = rm.GetString("hdrUte", culture);
                        columns[13] = rm.GetString("lblBranch", culture).Remove(rm.GetString("lblBranch", culture).IndexOf(':'));
                        columns[14] = rm.GetString("hdrEmplType", culture);
                        columns[15] = rm.GetString("hdrStartTime", culture);
                        columns[16] = rm.GetString("hdrEndTime", culture);
                        columns[17] = rm.GetString("hdrPassType", culture);
                        columns[18] = rm.GetString("hdrTotal", culture);
                        columns[19] = rm.GetString("hdrDescription", culture);

                        string columnsString = "";
                        foreach (string c in columns)
                        {
                            columnsString += c + "\t";
                        }
                        writer.WriteLine(columnsString);
                        int numr = 0;
                        foreach (DataRow ioPair in ioPairs.Rows)
                        {
                            numr++;
                            string resultStr = "";
                            int empl = -1;
                            int day = -1;
                            int month = -1;

                            int year = -1;

                            if (ioPair["empl_id"] != DBNull.Value)
                            {
                                resultStr += ioPair["empl_id"].ToString().Trim() + "\t";

                                empl = int.Parse(ioPair["empl_id"].ToString());
                            }
                            if (ioPair["first_name"] != DBNull.Value)
                                resultStr += ioPair["first_name"].ToString().Trim() + "\t";

                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            if (ioPair["date_year"] != DBNull.Value)
                            {
                                year = int.Parse(ioPair["date_year"].ToString().Trim());
                            }
                            if (ioPair["date_month"] != DBNull.Value)
                            {

                                month = int.Parse(ioPair["date_month"].ToString().Trim());
                            }
                            if (ioPair["date_day"] != DBNull.Value)
                            {

                                day = int.Parse(ioPair["date_day"].ToString().Trim());
                            }
                            DateTime ioPairDate = new DateTime();

                            if (year != -1 && month != -1 && day != -1) { ioPairDate = new DateTime(year, month, day); }
                            if (empl != -1)
                            {
                                if (emplSchedules.ContainsKey(empl))
                                    schedules = emplSchedules[empl];

                                WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                                if (sch.TimeSchemaID != -1)
                                    resultStr += sch.Description.Trim() + "\t";
                                else
                                    resultStr += "N/A" + "\t";

                                List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                                int cycleDay = 0;
                                string intervalsString = "";
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    cycleDay = interval.DayNum + 1;

                                    intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                                    if (!interval.Description.Trim().Equals(""))
                                        intervalsString += "(" + interval.Description.Trim() + ")";

                                    intervalsString += "; ";
                                }
                                resultStr += cycleDay.ToString().Trim() + "\t";
                                resultStr += intervalsString.Trim() + "\t";

                            }
                            if (ioPair["date_week"] != DBNull.Value)
                                resultStr += ioPair["date_week"].ToString().Trim() + "\t";

                            if (ioPair["date_year"] != DBNull.Value)
                            {
                                resultStr += ioPair["date_year"].ToString().Trim() + "\t";
                            }
                            if (ioPair["date_month"] != DBNull.Value)
                            {
                                resultStr += ioPair["date_month"].ToString().Trim() + "\t";
                            }
                            if (ioPair["date_day"] != DBNull.Value)
                            {
                                resultStr += ioPair["date_day"].ToString().Trim() + "\t";                          
                            }

                            if (ioPair["cost_centre"] != DBNull.Value)
                                resultStr += ioPair["cost_centre"].ToString().Trim() + "\t";

                            if (ioPair["cost_centre_code"] != DBNull.Value)
                                resultStr += ioPair["cost_centre_code"].ToString().Trim() + "\t";

                            if (ioPair["workgroup"] != DBNull.Value)
                                resultStr += ioPair["workgroup"].ToString().Trim() + "\t";

                            if (ioPair["ute"] != DBNull.Value)
                                resultStr += ioPair["ute"].ToString().Trim() + "\t";

                            if (ioPair["branch"] != DBNull.Value)
                                resultStr += ioPair["branch"].ToString().Trim() + "\t";

                            if (ioPair["empl_type"] != DBNull.Value)
                                resultStr += ioPair["empl_type"].ToString().Trim() + "\t";

                            if (ioPair["start_time"] != DBNull.Value)
                                resultStr += ioPair["start_time"].ToString().Trim() + "\t";

                            if (ioPair["end_time"] != DBNull.Value)
                                resultStr += ioPair["end_time"].ToString().Trim() + "\t";

                            if (ioPair["pass_type"] != DBNull.Value)
                                resultStr += ioPair["pass_type"].ToString().Trim() + "\t";


                            if (ioPair["total"] != DBNull.Value)
                            {
                                string hours = ioPair["total"].ToString().Remove(ioPair["total"].ToString().IndexOf(':'));
                                string minutes = ioPair["total"].ToString().Substring(ioPair["total"].ToString().IndexOf(':') + 1);
                                minutes = minutes.Remove(minutes.IndexOf(':'));

                                decimal minute = (decimal)(int.Parse(minutes)) / (decimal)60;


                                int hour = int.Parse(hours);

                                double num = (double)hour + (double)minute;
                                num = Math.Round(num, 2);
                                resultStr += num + "\t";

                            }


                            if (ioPair["description"] != DBNull.Value)
                                resultStr += ioPair["description"].ToString().Trim() + "\t";

                            writer.WriteLine(resultStr);

                        }

                        log.writeLog(numr + " " + DateTime.Now.ToString("HH:mm:sss"));
                        log.writeLog("Close" + " " + DateTime.Now.ToString("HH:mm:sss"));

                        writer.Close();
                        Response.Write("finished");

                        Session["DetailedDataReportFilePath"] = filePath;

                    }
                }
            }
            catch (Exception ex)
            {
                
               try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLDetailedDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }



        }

        public void DownloadFile(string url)
        {

            string URL = url;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(URL);

            if (fileInfo.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/force-download";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(fileInfo.Name));

                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                HttpContext.Current.Response.End();
            }

        }
    }
}
