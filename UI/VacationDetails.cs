using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using TransferObjects;
using Common;
using Util;
using Reports;

using System.Resources;
using System.Globalization;
using System.Collections.Generic;

namespace UI
{
    public partial class VacationDetails : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
        EmployeeVacEvidTO employeeVacation;

        //allUsers, Key is userID, value is UserName
        Dictionary<string, string> users = new Dictionary<string,string>();

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        List<EmployeeAbsenceTO> selected = new List<EmployeeAbsenceTO>();

        bool selecting = false;
        
        public VacationDetails(EmployeeVacEvidTO vacation)
        {
            InitializeComponent();
            this.CenterToScreen();
            employeeVacation = vacation;

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
                        
            List<ApplUserTO> auArray = new ApplUser().Search();
            foreach (ApplUserTO user in auArray)
            {
                if (!users.ContainsKey(user.UserID))
                    users.Add(user.UserID, user.Name);
            }

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(VacationEvidence).Assembly);
            logInUser = NotificationController.GetLogInUser();

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }

            // get all time schemas
            timeSchemas = new TimeSchema().Search();
            //geting employeeAbsences with passType = vacation to calculate used vacations for each employee
            DateTime vacYear = new DateTime();
            DateTime nextYear = new DateTime();
            if(!vacation.VacYear.Equals(new DateTime()))
            {
                vacYear = vacation.VacYear.AddYears(-1);
                nextYear = vacation.VacYear.AddYears(1);
            }

            EmployeeAbsence ea = new EmployeeAbsence();
            ea.EmplAbsTO.PassTypeID = Constants.vacation;
            ea.EmplAbsTO.DateStart = vacYear;
            ea.EmplAbsTO.DateEnd = nextYear;
            List<EmployeeAbsenceTO> absencesList = ea.SearchForVacEvid(vacation.EmployeeID.ToString(), "", new DateTime(), new DateTime());

            Dictionary<DateTime, List<EmployeeAbsenceTO>> absenceTable = new Dictionary<DateTime,List<EmployeeAbsenceTO>>();
            foreach (EmployeeAbsenceTO absence in absencesList)
            {
                if (!absenceTable.ContainsKey(absence.VacationYear))
                {
                    absenceTable.Add(absence.VacationYear, new List<EmployeeAbsenceTO>());
                }
                List<EmployeeAbsenceTO> absList = absenceTable[absence.VacationYear];
                absList.Add(absence);
                absenceTable[absence.VacationYear] = absList;
            }

            if (absenceTable.ContainsKey(vacation.VacYear))
            {
                //from year vacation plan and absences count real used days and total time to use
                EmployeeVacEvidTO vacTemp = Common.Misc.getVacationWithAbsences(vacation, absenceTable[vacation.VacYear], holidays, timeSchemas);
                vacation.DaysLeft = vacTemp.DaysLeft;
                vacation.UsedDays = vacTemp.UsedDays;
            }
            List<EmployeeAbsenceTO> absenceLastYear = new List<EmployeeAbsenceTO>();
            if(absenceTable.ContainsKey(vacation.VacYear.AddYears(-1)))
                absenceLastYear = absenceTable[vacation.VacYear.AddYears(-1)];
            //search last year vacations
            List<EmployeeVacEvidTO> vacLastYear = new EmployeeVacEvid().getVacations(vacation.EmployeeID.ToString(), vacation.VacYear.AddYears(-1), vacation.VacYear.AddYears(-1), -1, -1);
            if (vacLastYear.Count > 0)
            {
                EmployeeVacEvidTO vationLY = Common.Misc.getVacationWithAbsences(vacLastYear[0], absenceLastYear, holidays, timeSchemas);
                TimeSpan ts = vationLY.ValidTo.Date.Subtract(DateTime.Now);
                vacation.FromLastYear = Math.Min(vationLY.DaysLeft, ts.Days); 
            }

            //total for use is summ of days left from last year and days left from this year
            if (vacation.DaysLeft >= 0 && vacation.FromLastYear >= 0)
            {
                vacation.TotalForUse = vacation.DaysLeft + vacation.FromLastYear;
            }
            else
                vacation.TotalForUse = vacation.DaysLeft;
                            
            setLanguage();

            this.tbEmployee.Text = vacation.LastName;
            this.tbWorkingUnit.Text = vacation.WorkingUnit;
            this.tbYear.Text = vacation.VacYear.Year.ToString();
            this.tbApproved.Text = vacation.NumOfDays.ToString();
            this.tbValidTo.Text = vacation.ValidTo.ToString("dd.MM.yyyy");
            if (vacation.FromLastYear != -1)
            {
                this.tbTransmited.Text =vacation.FromLastYear.ToString();
            }
            else
            {
                this.tbTransmited.Text ="0";
            }
            if(vacation.VacYear.Year != DateTime.Now.Year)
                this.tbTransmited.Text = "N/A";
            if (vacation.UsedDays != -1)
            {
                this.tbUsed.Text = vacation.UsedDays.ToString();
            }
            else
            {
                this.tbUsed.Text = "0";
            }
            if (vacation.DaysLeft != -1)
            {
                this.tbLeft.Text = vacation.DaysLeft.ToString();
            }
            else
            {
                this.tbLeft.Text = "0";
            }
           
            List<EmployeeVacEvidScheduleTO> vacationSchedules = new EmployeeVacEvid().getVacationSchedules(vacation.EmployeeID, vacation.VacYear);
            populateUsedList(absencesList);
            populateUsingPlanListView(vacationSchedules);           
        }       

        private void populateUsedList(List<EmployeeAbsenceTO> absencesList)
        {
            try
            {
                lvUsingDetails.BeginUpdate();
                int i = 0;
                DateTime createdTime = new DateTime();
                foreach (EmployeeAbsenceTO ea in absencesList)
                {
                    if (ea.DateStart.Year == employeeVacation.VacYear.Year)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ea.DateStart.ToString("dd.MM.yyyy");
                        item.SubItems.Add(ea.DateEnd.ToString("dd.MM.yyyy"));
                        item.SubItems.Add(ea.VacationYear.ToString("yyyy"));

                        List<EmployeeTimeScheduleTO> EmplScheduleList = Common.Misc.GetEmployeeTimeSchedules(employeeVacation.EmployeeID, ea.DateStart, ea.DateEnd);
                        int used = Common.Misc.getNumOfWorkingDays(EmplScheduleList, ea.DateStart, ea.DateEnd, holidays, timeSchemas);
                        item.SubItems.Add(used.ToString());
                        if (users.ContainsKey(ea.CreatedBy))
                            item.SubItems.Add(users[ea.CreatedBy]);
                        if (ea.CreatedTime <= createdTime.AddSeconds(3) && ea.CreatedTime >= createdTime.AddSeconds(-3))
                        {
                            item.SubItems.Add(i.ToString());
                        }
                        else
                        {
                            i++;
                            item.SubItems.Add(i.ToString());

                        }
                        createdTime = ea.CreatedTime;
                        item.Tag = ea;
                        lvUsingDetails.Items.Add(item);
                    }
                }

                lvUsingDetails.EndUpdate();
                lvUsingDetails.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationDetails.populateUsedList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateUsingPlanListView(List<EmployeeVacEvidScheduleTO> schedules)
        {
            try
            {
                lvUsingPlan.BeginUpdate();
                for (int i = 0; i < schedules.Count; i++)
                {
                    EmployeeVacEvidScheduleTO vacationSch = schedules[i];
                    ListViewItem item = new ListViewItem();
                    item.Text = vacationSch.StartDate.ToString("dd.MM.yyyy");
                    item.SubItems.Add(vacationSch.EndDate.ToString("dd.MM.yyyy"));
                    
                    //item.SubItems.Add(vacationSch.Status);
                    if(users.ContainsKey(employeeVacation.CreatedBy))
                    item.SubItems.Add(users[employeeVacation.CreatedBy]);
                    lvUsingPlan.Items.Add(item);
                }

                lvUsingPlan.EndUpdate();
                lvUsingPlan.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationDetails.populateUsingPlanListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationDetails.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnPrintRequest.Text = rm.GetString("btnPrintRequest", culture);
               
                // Form name
                this.Text = rm.GetString("vacationEvidenceDetails", culture);

                // group box text
                this.gbEmployeeData.Text = rm.GetString("gbEmployeeData", culture);
                gbSummaryData.Text = rm.GetString("gbSummaryData", culture);
                gbUsingDetails.Text = rm.GetString("gbUsingDetails", culture);
                gbUsingPlan.Text = rm.GetString("gbUsingPlan", culture)+"*";

                
                // label's text
                lblWorkingUnit.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblYear.Text = rm.GetString("hdrYear", culture);
                lblApproved.Text = rm.GetString("hdrApproved", culture);
                lblForCurrentYear.Text = rm.GetString("lblForCurrentYear", culture);
                lblLeft.Text = rm.GetString("hdrLeft", culture);
                lblTransmited.Text = rm.GetString("hdrTransposed", culture);
                lblUsed.Text = rm.GetString("hdrUsedDays", culture);
                lblUsingPlan.Text = rm.GetString("lblUsingPlan", culture);
                lblValidTo.Text = rm.GetString("lblValidTo", culture);
                
                // list view initialization
                lvUsingPlan.BeginUpdate();
                lvUsingPlan.Columns.Add(rm.GetString("hdrFrom", culture), (lvUsingPlan.Width - 10) / 3, HorizontalAlignment.Left);
                lvUsingPlan.Columns.Add(rm.GetString("hdrTo", culture), (lvUsingPlan.Width - 10) / 3, HorizontalAlignment.Left);
                //lvUsingPlan.Columns.Add(rm.GetString("hdrStatus", culture), (lvUsingPlan.Width - 10) / 4, HorizontalAlignment.Left);
                lvUsingPlan.Columns.Add(rm.GetString("hdrOperator", culture), (lvUsingPlan.Width - 10) / 3, HorizontalAlignment.Left);
                lvUsingPlan.EndUpdate();

                lvUsingDetails.BeginUpdate();
                lvUsingDetails.Columns.Add(rm.GetString("hdrFrom", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.Columns.Add(rm.GetString("hdrTo", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.Columns.Add(rm.GetString("hdrYear", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.Columns.Add(rm.GetString("hdrNumOfDays", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.Columns.Add(rm.GetString("hdrOperator", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.Columns.Add(rm.GetString("hdrRequestNum", culture), (lvUsingDetails.Width - 10) / 6, HorizontalAlignment.Left);
                lvUsingDetails.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationDetails.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void VacationDetails_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationDetails.VacationDetails_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrintRequest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (selected.Count > 0)
                {
                   
                    DateTime startTime = new DateTime();
                    DateTime endTime = new DateTime();
                    List<EmployeeAbsenceTO> absList = new List<EmployeeAbsenceTO>();
                    foreach (EmployeeAbsenceTO abs in selected)
                    {
                        absList.Add(abs);
                        if (startTime.Equals(new DateTime()) || startTime > abs.DateStart)
                            startTime = abs.DateStart;
                        if (endTime < abs.DateEnd)
                            endTime = abs.DateEnd;
                    }
                    EmployeeAbsenceTO absence = new EmployeeAbsenceTO();
                    if (absList.Count > 0)
                        absence = absList[0];
                    
                    EmployeeTO selEmployeeTO = new Employee().Find(absence.EmployeeID.ToString());
                    
                    selEmployeeTO.LastName += " " + selEmployeeTO.FirstName;
                    Reports.GSK.GSKVAcationRequest req = new Reports.GSK.GSKVAcationRequest(employeeVacation, selEmployeeTO, startTime, endTime, absList);
                    req.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationDetails.btnPrintRequest_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void VacationDetails_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(lvUsingDetails.Items.Count>0)
                lvUsingDetails.Items[lvUsingDetails.Items.Count - 1].Selected = true;
            //lvUsingDetails.Enabled = false;
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationDetails.VacationDetails_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvUsingDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!selecting)
                {
                    this.Cursor = Cursors.WaitCursor;

                    selecting = true;
                    selected = new List<EmployeeAbsenceTO>();
                    foreach (ListViewItem item in lvUsingDetails.SelectedItems)
                    {
                        lvUsingDetails.BeginUpdate();
                        EmployeeAbsenceTO abs = (EmployeeAbsenceTO)item.Tag;
                        selected.Add(abs);
                        string reqNum = item.SubItems[5].Text.ToString().Trim();
                        foreach (ListViewItem it in lvUsingDetails.Items)
                        {
                            string num = it.SubItems[5].Text.ToString().Trim();
                            if (reqNum.Equals(num))
                            {
                                it.Selected = true;
                                EmployeeAbsenceTO ab = (EmployeeAbsenceTO)it.Tag;
                                bool found = false;
                                foreach (EmployeeAbsenceTO emplAbs in selected)
                                {
                                    if (emplAbs.RecID == ab.RecID)
                                        found = true;
                                }
                                if (!found)
                                    selected.Add(ab);
                            }
                            else
                            {
                                it.Selected = false;
                            }
                        }
                        lvUsingDetails.EndUpdate();
                        lvUsingDetails.Invalidate();
                    }

                    selecting = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationDetails.lvUsingDetails_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }      
        }        
    }
}