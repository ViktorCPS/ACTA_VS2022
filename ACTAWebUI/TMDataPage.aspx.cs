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
    public partial class TMDataPage : System.Web.UI.Page
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
                    rm = new ResourceManager("ACTAWebUI.Resource", typeof(TMDataPage).Assembly);
                    DateTimeFormatInfo dateTimeformat = new CultureInfo("en-US", true).DateTimeFormat;
                    dateTimeFormat = dateTimeformat.SortableDateTimePattern;

                    tbPeriod.Text = DateTime.Now.ToString(Constants.monthFormat.Trim());

                    Menu1.Items[0].Selected = true;

                    for (int i = 0; i < Menu1.Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            Menu1.Items[i].Selected = true;
                            //Menu1.Items[i].ImageUrl = "/ACTAWeb/CommonWeb/images/activeTab.jpg";
                            //Menu1.Items[i].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/activeTabSeparator.jpg";
                            MultiView1.SetActiveView(MultiView1.Views[i]);
                        }
                        else
                        {
                            //Menu1.Items[i].ImageUrl = "/ACTAWeb/CommonWeb/images/nonActiveTab.jpg";
                            //Menu1.Items[i].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/nonActiveTabSeparator.jpg";
                        }
                    }

                    // reload selected filter state
                    LoadState();
                }

                if (Session["TMDataPage.btnShowClik"] != null && (bool)Session["TMDataPage.btnShowClik"])
                    btnShow_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TMDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                for (int i = 0; i < Menu1.Items.Count; i++)
                {
                    if (i == int.Parse(e.Item.Value))
                    {
                        Menu1.Items[i].Selected = true;
                        //Menu1.Items[i].ImageUrl = "/ACTAWeb/CommonWeb/images/activeTab.jpg";
                        //Menu1.Items[i].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/activeTabSeparator.jpg";
                        MultiView1.SetActiveView(MultiView1.Views[i]);
                    }
                    else
                    {
                        //Menu1.Items[i].ImageUrl = "/ACTAWeb/CommonWeb/images/nonActiveTab.jpg";
                        //Menu1.Items[i].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/nonActiveTabSeparator.jpg";
                    }
                }
                                
                // save selected filter state
                SaveState();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TMDataPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                //if (Session["FilterState"] != null)
                //    Session["FilterState"] = new Dictionary<string, string>();
                if (Session["TMDataPage.btnShowClik"] != null)
                    Session["TMDataPage.btnShowClik"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;
                                
                DateTime month = new DateTime();

                if (tbPeriod.Text.Trim().Equals(""))
                {                    
                    tbPeriod.Focus();
                    return;
                }

                month = CommonWeb.Misc.createDate("01." + tbPeriod.Text.Trim());

                if (month.Equals(new DateTime()))
                {                    
                    tbPeriod.Focus();
                    return;
                }

                FindIOPairsForEmployee(month);
                DrawGraphControl(month);

                // save selected filter state
                SaveState();

                Session["TMDataPage.btnShowClik"] = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TabPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                ctrlHolder.Controls.Clear();
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

                        int hours = 0;
                        int min = 0;
                        foreach (IOPairTO iopair in IOPairList)
                        {
                            if (iopair.StartTime.Date.Equals(month.Date) || iopair.EndTime.Date.Equals(month.Date))
                            {
                                ioPairsForDay.Add(iopair);

                                if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                                {
                                    if (int.Parse(iopair.StartTime.ToString("mm")) == 0)
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH"));
                                    }
                                    else
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH")) - 1;
                                        min += 60 - int.Parse(iopair.StartTime.ToString("mm"));
                                    }

                                    min += int.Parse(iopair.EndTime.ToString("mm"));
                                }

                                while (min >= 60)
                                {
                                    hours++;
                                    min -= 60;
                                }
                            }
                        }

                        string timeString = ""; //text for summ time cell
                        if (hours == 0) { timeString += "00h"; }
                        else
                        {
                            if (hours < 10) { timeString += "0"; }
                            timeString += hours + "h";
                        }
                        if (min == 0) { timeString += "00min"; }
                        else
                        {
                            if (min < 10) { timeString += "0"; }
                            timeString += min + "min";
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

                        Color backColor = Color.White;
                        bool isAltCtrl = false;

                        if ((i - startIndex) % 2 != 0)
                        {
                            backColor = ColorTranslator.FromHtml("#E6E6E6");
                            isAltCtrl = true;
                        }

                        EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                        emplDay.ID = "emplDayView" + i.ToString();
                        //emplDay.DayPairList = ioPairsForDay;
                        emplDay.DayIntervalList = timeSchemaIntervalList;
                        emplDay.BackColor = backColor;
                        emplDay.Date = month;
                        emplDay.Total = timeString.Trim();
                        emplDay.Day = day;
                        if (i == startIndex)
                            emplDay.IsFirst = true;
                        emplDay.IsAltCtrl = isAltCtrl;

                        ctrlHolder.Controls.Add(emplDay);

                        month = month.AddDays(1);
                    }
                }
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

                if (Session["FilterState"] != null)
                    filterState = (Dictionary<string, string>)Session["FilterState"];

                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "TMDataPage.", filterState);
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

                if (Session["FilterState"] != null)
                {
                    filterState = (Dictionary<string, string>)Session["FilterState"];
                    CommonWeb.Misc.LoadState(this, "TMDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
