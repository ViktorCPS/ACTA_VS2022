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
using System.Drawing.PieChart.WebControl;

using CommonWeb;
using Common;
using Util;
using TransferObjects;

namespace ReportsWeb
{
    public partial class EmployeePresenceReport : System.Web.UI.Page
    {
        private static CultureInfo culture;
        private static ResourceManager rm;
        private static string dateTimeFormat;
        private static int noOfDaysInMonth = 0;
        private static List<IOPairTO> IOPairList = new List<IOPairTO>();
        private static Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ReportsWeb.Resource", typeof(EmployeePresenceReport).Assembly);
                    DateTimeFormatInfo dateTimeformat = new CultureInfo("en-US", true).DateTimeFormat;
                    dateTimeFormat = dateTimeformat.SortableDateTimePattern;

                    lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    
                    setLanguage();
                    
                    lblError.Text = "";
                    tbMonth.Text = DateTime.Now.ToString(Constants.monthFormat.Trim());
                    
                    tbMonth.Focus();
                                        
                    // if returned from result page, reload selected filter state
                    Dictionary<string, string> filterState = new Dictionary<string, string>();
                    if (Session["FilterState"] != null)
                        filterState = (Dictionary<string, string>)Session["FilterState"];
                    CommonWeb.Misc.LoadState(this, "EmployeePresenceReport.", filterState);
                }

                if (Session["ctrlGraph"] != null)
                    btnShow_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeePresenceReport.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx", false);
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
                lblMonth.Text = rm.GetString("lblMonth", culture);
                lblMonthExample.Text = rm.GetString("lblMonthExample", culture);
                lblMonthFormat.Text = rm.GetString("lblMonthFormat", culture);
                lblPresenceReport.Text = rm.GetString("lblPresenceReport", culture);
                lblDate.Text = rm.GetString("lblDate", culture);
                lblTotal.Text = rm.GetString("lblTotal", culture);
                if (Session["LoggedInUser"] != null)
                    lblLoggedInUser.Text = Session["LoggedInUser"].ToString().Trim();
                else
                    lblLoggedInUser.Text = "";

                btnShow.Text = rm.GetString("btnShow", culture);
                btnStatistics.Text = rm.GetString("btnStatistics", culture);
                btnShowGraph.Text = rm.GetString("btnShowGraph", culture);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeePresenceReport.lbtnMenu_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnStatistics_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";
                DateTime month = new DateTime();

                if (tbMonth.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noMonth", culture);
                    tbMonth.Focus();                    
                    return;
                }

                month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim());

                if (month.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidMonth", culture);
                    tbMonth.Focus();                    
                    return;
                }

                // save selected filter state
                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "EmployeePresenceReport.", new Dictionary<string, string>());

                FindIOPairsForEmployee(month);
                DrawStatistics(month);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeePresenceReport.btnStatistics_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShowGraph_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";
                DateTime month = new DateTime();

                if (tbMonth.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noMonth", culture);
                    tbMonth.Focus();                    
                    return;
                }

                month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim());

                if (month.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidMonth", culture);
                    tbMonth.Focus();                    
                    return;
                }

                // save selected filter state
                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "EmployeePresenceReport.", new Dictionary<string, string>());

                FindIOPairsForEmployee(month);
                DrawGraph(month);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeePresenceReport.btnShowGraph_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";
                DateTime month = new DateTime();

                if (tbMonth.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noMonth", culture);
                    tbMonth.Focus();
                    return;
                }

                month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim());

                if (month.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidMonth", culture);
                    tbMonth.Focus();
                    return;
                }

                // save selected filter state
                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "EmployeePresenceReport.", new Dictionary<string, string>());

                FindIOPairsForEmployee(month);
                DrawGraphControl(month);
                Session["ctrlGraph"] = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeePresenceReport.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void ClearSessionValues()
        {
            try
            {
                Session["ctrlGraph"] = null;
                if (Session["FilterState"] != null)
                    Session["FilterState"] = new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FindIOPairsForEmployee(DateTime month)
        {         
            try
            {
                List<DateTime> datesList = new List<DateTime>();                
                DateTime nextMonth = month.AddMonths(1);
                noOfDaysInMonth = 0;
                while (month < nextMonth)
                {
                    datesList.Add(month);
                    month = month.AddDays(1);
                    noOfDaysInMonth++;
                }

                string selctedEmployee = Session["UserID"].ToString().Trim();                
                IOPairList = new IOPair().SearchAllPairsForEmpl(selctedEmployee, datesList, -1, -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList, List<WorkTimeSchemaTO> timeSchema)
        {
            try
            {
                List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();

                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleList[timeScheduleIndex])).Date;
                    //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }

                return intervalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawGraphControl(DateTime month)
        {
            try
            {               
                //ctrlHolder.Controls.Clear();
                //int startIndex = 0;
                //if (startIndex >= 0)
                //{
                //    int lastIndex = noOfDaysInMonth;

                //    List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(Session["UserID"].ToString().Trim(), month, month.AddMonths(1));
                //    string schemaID = "";
                //    foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                //    {
                //        schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                //    }
                //    if (!schemaID.Equals(""))
                //    {
                //        schemaID = schemaID.Substring(0, schemaID.Length - 2);
                //    }

                //    List<WorkTimeSchemaTO> timeSchema = new TimeSchema().Search(schemaID);
                //    for (int i = startIndex; i < lastIndex; i++)
                //    {
                //        List<IOPairTO> ioPairsForDay = new List<IOPairTO>();
                                                
                //        int hours = 0;
                //        int min = 0;
                //        foreach (IOPairTO iopair in IOPairList)
                //        {
                //            if (iopair.StartTime.Date.Equals(month.Date) || iopair.EndTime.Date.Equals(month.Date))
                //            {
                //                ioPairsForDay.Add(iopair);

                //                if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                //                {
                //                    if (int.Parse(iopair.StartTime.ToString("mm")) == 0)
                //                    {
                //                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH"));
                //                    }
                //                    else
                //                    {
                //                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH")) - 1;
                //                        min += 60 - int.Parse(iopair.StartTime.ToString("mm"));
                //                    }

                //                    min += int.Parse(iopair.EndTime.ToString("mm"));
                //                }

                //                while (min >= 60)
                //                {
                //                    hours++;
                //                    min -= 60;
                //                }
                //            }
                //        }

                //        string timeString = ""; //text for summ time cell
                //        if (hours == 0) { timeString += "00h"; }
                //        else
                //        {
                //            if (hours < 10) { timeString += "0"; }
                //            timeString += hours + "h";
                //        }
                //        if (min == 0) { timeString += "00min"; }
                //        else
                //        {
                //            if (min < 10) { timeString += "0"; }
                //            timeString += min + "min";
                //        }

                //        List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(int.Parse(Session["UserID"].ToString()), month, timeScheduleList, timeSchema);

                //        string day = "";
                //        switch (month.DayOfWeek)
                //        {
                //            case DayOfWeek.Monday:
                //                day = rm.GetString("mon", culture);
                //                break;
                //            case DayOfWeek.Tuesday:
                //                day = rm.GetString("tue", culture);
                //                break;
                //            case DayOfWeek.Wednesday:
                //                day = rm.GetString("wed", culture);
                //                break;
                //            case DayOfWeek.Thursday:
                //                day = rm.GetString("thu", culture);
                //                break;
                //            case DayOfWeek.Friday:
                //                day = rm.GetString("fri", culture);
                //                break;
                //            case DayOfWeek.Saturday:                                
                //                day = rm.GetString("sat", culture);
                //                break;
                //            case DayOfWeek.Sunday:                                
                //                day = rm.GetString("sun", culture);
                //                break;
                //            default:
                //                day = "";
                //                break;
                //        }
                        
                //        Color backColor = Color.White;                        
                        
                //        if ((i - startIndex) % 2 != 0)
                //        {
                //            backColor = Color.FromName("#E6E6E6");
                //        }                        
                        
                //        EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                //        emplDay.ID = "emplDayView" + i.ToString();
                //        emplDay.DayPairList = ioPairsForDay;
                //        emplDay.DayIntervalList = timeSchemaIntervalList;
                //        emplDay.BackColor = backColor;                        
                //        emplDay.Date = month;
                //        emplDay.Total = timeString.Trim();
                //        emplDay.Day = day;
                                                
                //        ctrlHolder.Controls.Add(emplDay);
                                                
                //        month = month.AddDays(1);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void DrawGraph(DateTime month)
        {
            try
            {
                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                this.ctrlHolder.Controls.Clear();
                Dictionary<int, List<IOPairTO>> dayPairs = new Dictionary<int, List<IOPairTO>>();
                Session["dayPairs"] = dayPairs;
                Dictionary<int, List<WorkTimeIntervalTO>> dayIntervals = new Dictionary<int, List<WorkTimeIntervalTO>>();
                Session["dayIntervals"] = dayIntervals;
                string date = "";
                int startIndex = 0;
                
                if (startIndex >= 0)
                {
                    int lastIndex = noOfDaysInMonth;

                    List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(Session["UserID"].ToString().Trim(), month, month.AddMonths(1));
                    string schemaID = "";
                    foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                    {
                        schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                    }
                    if (!schemaID.Equals(""))
                    {
                        schemaID = schemaID.Substring(0, schemaID.Length - 2);
                    }

                    List<WorkTimeSchemaTO> timeSchema = new TimeSchema().Search(schemaID);
                    
                    for (int i = startIndex; i < lastIndex; i++)
                    {
                        List<IOPairTO> ioPairsForDay = new List<IOPairTO>();

                        foreach (IOPairTO iopair in IOPairList)
                        {
                            if (iopair.StartTime.Date.Equals(month.Date) || iopair.EndTime.Date.Equals(month.Date))
                            {
                                ioPairsForDay.Add(iopair);
                            }
                        }

                        List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(int.Parse(Session["UserID"].ToString()), month, timeScheduleList, timeSchema);
                        string day = "";
                        switch (month.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                day = rm.GetString("mon", culture);
                                break;
                            case DayOfWeek.Tuesday:
                                day = rm.GetString("tue", culture);
                                break;
                            case DayOfWeek.Wednesday:
                                day = rm.GetString("wed", culture);
                                break;
                            case DayOfWeek.Thursday:
                                day = rm.GetString("thu", culture);
                                break;
                            case DayOfWeek.Friday:
                                day = rm.GetString("fri", culture);
                                break;
                            case DayOfWeek.Saturday:
                                day = rm.GetString("sat", culture);
                                break;
                            case DayOfWeek.Sunday:
                                day = rm.GetString("sun", culture);
                                break;
                            default:
                                day = "";
                                break;
                        }
                        date = month.ToString(Constants.dateFormat.Trim()) + "  " + day;

                        ((Dictionary<int, List<IOPairTO>>)Session["dayPairs"]).Add(i, ioPairsForDay);
                        ((Dictionary<int, List<WorkTimeIntervalTO>>)Session["dayIntervals"]).Add(i, timeSchemaIntervalList);

                        string backColor = "White";
                        bool isLast = false;
                        bool isWeekend = false;

                        if ((i - startIndex) % 2 != 0)
                        {
                            backColor = "LightGray";
                        }
                        if (i == lastIndex - 1)
                        {
                            isLast = true;
                        }
                        if (month.DayOfWeek == DayOfWeek.Saturday || month.DayOfWeek == DayOfWeek.Sunday)
                        {
                            isWeekend = true;
                        }

                        System.Web.UI.WebControls.Image emplViewImg = new System.Web.UI.WebControls.Image();
                        emplViewImg.Width = new Unit((int)resultPanel.Width.Value - 25);
                        emplViewImg.Height = new Unit(15);
                        //EmployeeWorkingDayView emplView = new EmployeeWorkingDayView(0, 24, 60, ioPairsForDay, date, "", passTypes, timeSchemaIntervalList, isLast, brushColor, backColor, (int)emplViewImg.Width.Value, (int)emplViewImg.Height.Value);
                        string urlString = "/ACTAWeb/ReportsWeb/EmployeeWorkingDayView.aspx?minValue=0&maxValue=24&stepValue=60&backColor=" + backColor + "&displayString=" + date.Trim();
                        urlString += "&isLast=" + isLast.ToString() + "&isWeekend=" + isWeekend.ToString();
                        urlString += "&width=" + emplViewImg.Width.Value.ToString() + "&height=" + emplViewImg.Height.Value.ToString() + "&numOfDay=" + i.ToString();

                        emplViewImg.ImageUrl = urlString;
                        //employeeWorkingDayView.SetBounds(5, 35 + 15 * (i - startIndex), this.gbGraphicReport.Width - 10, 15);
                        //this.gbGraphicReport.Controls.Add(employeeWorkingDayView);
                        ctrlHolder.Controls.Add(emplViewImg);

                        //ImageButton BarChartImage1 = new ImageButton();
                        //BarChartImage1.ID = "bar1";
                        //BarChartImage1.Width = new Unit((int)resultPanel.Width.Value - 10);
                        //BarChartImage1.Height = 15;
                        //BarChartImage1.Attributes.Add("pairID", "5");
                        ////BarChartImage1.Click += new ImageClickEventHandler(img_Click);
                        //BarChartImage1.ToolTip = "Bar 2";
                        //BarChartImage1.ImageUrl = "/ACTAWeb/ReportsWeb/BarSegment.aspx?chartType=bar&width=1000&height=10";
                        //ctrlHolder.Controls.Add(BarChartImage1);

                        month = month.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawStatistics(DateTime month)
        {
            try
            {
                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                this.ctrlHolder.Controls.Clear();

                List<string> colorList = new List<string>();

                colorList.Add("lightyellow");
                colorList.Add("pink");
                colorList.Add("lightgreen");
                colorList.Add("azure");
                colorList.Add("lavender");
                colorList.Add("orange");

                List<PassTypeTO> passTypesAll = new PassType().Search();
                Dictionary<int, string> ptDesc = new Dictionary<int, string>();

                foreach (PassTypeTO pt in passTypesAll)
                {
                    if (!ptDesc.ContainsKey(pt.PassTypeID))
                        ptDesc.Add(pt.PassTypeID, pt.Description);
                }
                
                Dictionary<int, int> pairsStatistics = new Dictionary<int,int>();

                if (IOPairList.Count > 0)
                {
                    foreach (IOPairTO pairTO in IOPairList)
                    {
                        if (pairsStatistics.ContainsKey(pairTO.PassTypeID))
                            pairsStatistics[pairTO.PassTypeID]++;
                        else
                            pairsStatistics.Add(pairTO.PassTypeID, 1);
                    }

                    string values = "";
                    string texts = "";
                    string links = "";
                    string sliceDisplacments = "";
                    string colors = "";

                    int i = 0;
                    foreach (int ptID in pairsStatistics.Keys)
                    {
                        values += ((pairsStatistics[ptID] * 100) / IOPairList.Count).ToString() + ",";
                        texts += ptDesc[ptID].Trim() + ",";
                        links += "http://www.google.com,";
                        //links += "/ACTAWeb/Default.aspx"; // ne moze da se prosledi ovakva adresa!!!
                        sliceDisplacments += "0.05,";
                        if (colorList.Count > i)
                            colors += colorList[i] + ",";
                        else
                            colors += "Yellow,";
                        i++;
                    }

                    values = values.Substring(0, values.Length - 1);
                    texts = texts.Substring(0, texts.Length - 1);
                    links = links.Substring(0, links.Length - 1);
                    sliceDisplacments = sliceDisplacments.Substring(0, sliceDisplacments.Length - 1);
                    colors = colors.Substring(0, colors.Length - 1);
                    int width = ((int)resultPanel.Width.Value) / 2;
                    int height = (int)resultPanel.Height.Value - 20;
                    //string url = "/ACTAWeb/ReportsWeb/TestGraphicsPage.aspx?width=" + width.ToString() + "&height=" + height.ToString() + "&values=" + values + "&texts=" + texts + "&links=" + links
                    //    + "&colors=" + colors + "&sliceDisplacements=" + sliceDisplacments;
                    
                    //Response.Redirect(url, false);

                    PieChart3D pieChart = new PieChart3D();
                    pieChart.ForeColor = "Black";
                    pieChart.Width = width;
                    pieChart.Height = width / 2;
                    pieChart.Values = values;
                    pieChart.Texts = texts;
                    pieChart.Links = links;
                    pieChart.Colors = colors;
                    pieChart.SliceDisplacments = sliceDisplacments;
                    pieChart.Opacity = 100;
                    pieChart.ShadowStyle = System.Drawing.PieChart.ShadowStyle.GradualShadow;
                    pieChart.EdgeColorType = System.Drawing.PieChart.EdgeColorType.DarkerThanSurface;
                    pieChart.FontFamily = "Times New Roman";
                    pieChart.FontSize = 10;
                    pieChart.FontStyle = System.Drawing.FontStyle.Bold;

                    ctrlHolder.Controls.Add(pieChart);
                }
                else
                    lblError.Text = rm.GetString("noMonthPairs", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
