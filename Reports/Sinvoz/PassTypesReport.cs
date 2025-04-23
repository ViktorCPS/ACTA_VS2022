using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports.Sinvoz
{
    public partial class PassTypesReport : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog debug;

        // specific pass types
        private const int HOLIDAY = -200;
        private const int WORKONHOLIDAY = -201;
        private const int OVERTIME = -202;
        private const int PAUSE = -102;
        private const int UNEXCUSEDABSENCE = -100;
        private const int EXTRAHOURS = -2;
        private const int EXTRAHOURSTOTAL = -3;
        private const int HOURSUSED = -101;

        private const int WORK = 0;
        private const int OFFICIALOUT = 2;
        private const int standardPauseID = 0;

        // duration of standard pause
        TimeSpan pauseDuration = new TimeSpan();
        
        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;
        string wuString;

        List<EmployeeTO> currentEmployees = new List<EmployeeTO>();
        Dictionary<int, EmployeeTO> employeesDic = new Dictionary<int, EmployeeTO>();

        // all time shemas
        Dictionary<int, WorkTimeSchemaTO> timeSchemas = new Dictionary<int,WorkTimeSchemaTO>();
                
        // all employee time schedules
        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

        // all holidays, pauses, pass types
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime, HolidayTO>();
        Dictionary<int, TimeSchemaPauseTO> pauses = new Dictionary<int, TimeSchemaPauseTO>();
        Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();

        Filter filter;

        public PassTypesReport()
        {
            try
            {
                InitializeComponent();
                               
                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PassTypesReport).Assembly);
                setLanguage();
                logInUser = NotificationController.GetLogInUser();

                DateTime date = DateTime.Now.Date;                

                dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
                dtpToDate.Value = date;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";

                cbWorkingUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void PassTypesReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateWorkingUnitCombo();                

                wuString = "";
                foreach (WorkingUnitTO wu in wUnits)
                {
                    wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateEmplCombo(-1);

                // load filter
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());
                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                //get all pausses
                List<TimeSchemaPauseTO> pauseList = new TimeSchemaPause().Search("");
                foreach (TimeSchemaPauseTO p in pauseList)
                {
                    pauses.Add(p.PauseID, p);
                    if (p.PauseID == standardPauseID)
                        pauseDuration = new TimeSpan(0, p.PauseDuration, 0);
                }

                //get all time schemas
                List<WorkTimeSchemaTO> schemasAll = new TimeSchema().Search();
                foreach (WorkTimeSchemaTO schema in schemasAll)
                {
                    if (!timeSchemas.ContainsKey(schema.TimeSchemaID))
                        timeSchemas.Add(schema.TimeSchemaID, schema);
                }

                //get all passTypes
                //List<int> isPass = new List<int>();
                //isPass.Add(Constants.passOnReader);
                //isPass.Add(Constants.wholeDayAbsence);
                List<PassTypeTO> ptList = new PassType().Search();
                foreach (PassTypeTO pt in ptList)
                {
                    if (!passTypes.ContainsKey(pt.PassTypeID))
                        passTypes.Add(pt.PassTypeID, pt);
                }
        
                // add specific pass types
                //if (!passTypes.ContainsKey(HOLIDAY))
                //    passTypes.Add(HOLIDAY, new PassTypeTO(HOLIDAY, rm.GetString("ptHolidays", culture), -1, -1, ""));
                //if (!passTypes.ContainsKey(WORKONHOLIDAY))
                //    passTypes.Add(WORKONHOLIDAY, new PassTypeTO(WORKONHOLIDAY, rm.GetString("ptWorkOnHoliday", culture), -1, -1, ""));
                //if (!passTypes.ContainsKey(OVERTIME))
                //    passTypes.Add(OVERTIME, new PassTypeTO(OVERTIME, rm.GetString("ptOvertime", culture), -1, -1, ""));
                //if (!passTypes.ContainsKey(PAUSE))
                //    passTypes.Add(PAUSE, new PassTypeTO(PAUSE, rm.GetString("ptPause", culture), -1, -1, ""));
                //if (!passTypes.ContainsKey(UNEXCUSEDABSENCE))
                //    passTypes.Add(UNEXCUSEDABSENCE, new PassTypeTO(UNEXCUSEDABSENCE, rm.GetString("ptUnexcusedAbsence", culture), -1, -1, ""));
                if (!passTypes.ContainsKey(EXTRAHOURS))
                    passTypes.Add(EXTRAHOURS, new PassTypeTO(EXTRAHOURS, rm.GetString("ptExtraHours", culture), -1, -1, ""));
                if (!passTypes.ContainsKey(EXTRAHOURSTOTAL))
                    passTypes.Add(EXTRAHOURSTOTAL, new PassTypeTO(EXTRAHOURSTOTAL, rm.GetString("ptExtraHoursTotal", culture), -1, -1, ""));                
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.PassTypesReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {            
                if (cbWorkingUnit.SelectedIndex >= 0 && cbWorkingUnit.SelectedValue is int)
                    populateEmplCombo((int)cbWorkingUnit.SelectedValue);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmplCombo(int wuID)
        {
            try
            {
                string workUnitID = wuID.ToString();
                if (wuID < 0)
                {
                    currentEmployees = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    }
                    currentEmployees = new Employee().SearchByWU(workUnitID);
                }

                // remove retired
                if (!chbShowRetired.Checked)
                {
                    IEnumerator emplEnum = currentEmployees.GetEnumerator();

                    while (emplEnum.MoveNext())
                    {
                        EmployeeTO emplTO = (EmployeeTO)emplEnum.Current;
                        if (emplTO.Status == Constants.statusRetired)
                        {
                            currentEmployees.Remove((EmployeeTO)emplEnum.Current);
                            emplEnum = currentEmployees.GetEnumerator();
                        }
                    }
                }
                
                foreach (EmployeeTO employee in currentEmployees)
                {
                    employee.LastName += " " + employee.FirstName;
                }                

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmployees.Insert(0, empl);

                cbEmployee.DataSource = currentEmployees;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.populateEmplCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("PassTypesReport", culture);
                
                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                gbTimeInterval.Text = rm.GetString("timeInterval", culture);
                
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                lblReportType.Text = rm.GetString("reportType", culture);
                lblReportFormat.Text = rm.GetString("lblReportFormat", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);                
                lblTo.Text = rm.GetString("lblTo", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSaveCriteria.Text = rm.GetString("btnSave", culture);
                
                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                
                rbAnalytical.Text = rm.GetString("analitycal", culture);
                rbSummary.Text = rm.GetString("summary", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {            
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbWorkingUnit.SelectedIndex >= 0 && cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmplCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
			{
                string reportName = "";
                if (rbAnalytical.Checked)
                    reportName = "Analiticki izvestaj po tipovima prolazaka-";
                else
                    reportName = "Sumarni izvestaj po tipovima prolazaka-";

                reportName += DateTime.Now.ToString("dd_MM_yyy HH_mm");

                SaveFileDialog sfd = new SaveFileDialog();                
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls|" +                
                "CSV (*.csv)|*.csv";

                if (rbXLS.Checked)
                    sfd.FilterIndex = 1;
                else
                    sfd.FilterIndex = 2;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;                                

				this.Cursor = Cursors.WaitCursor;

                string filePath = sfd.FileName;

				List<EmployeeTO> employees = new List<EmployeeTO>();
				List<int> employeesID = new List<int>();
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();
                			
				if (this.dtpFromDate.Value <= this.dtpToDate.Value)
				{
                    if (cbEmployee.SelectedIndex > 0)
                    {
                        // calculate for selected employee
                        employees.Add((EmployeeTO)cbEmployee.SelectedItem);
                    }
                    else
                    {
                        // calculate for all employees from combo box
                        employees = currentEmployees;
                    }

					foreach (EmployeeTO empl in employees)
					{
						// Employee IDs
                        if (empl.EmployeeID > -1)
                        {
                            employeesID.Add(empl.EmployeeID);

                            if (!employeesDic.ContainsKey(empl.EmployeeID))
                                employeesDic.Add(empl.EmployeeID, empl);
                        }
					}

                    if (employeesID.Count <= 0)
                    {
                        MessageBox.Show(rm.GetString("noCSVExport", culture));
                        return;
                    }

					// get all valid IO Pairs for selected employees and time interval
					// io pairs are sorted by employee, io_pair_date, iopair start_time ascending
					List<IOPairTO> ioPairList = new IOPair().SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesID, -1);
                                        
					// Key is Employee ID, value is List of valid IO Pairs for that Employee
					Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();

					foreach (int emplID in employeesID)
					{
						emplPairs.Add(emplID, new List<IOPairTO>());
					}

					// io pairs for particular employee will be sorted by io_pair_date ascending
					for (int i = 0; i < ioPairList.Count; i++)
					{
						emplPairs[ioPairList[i].EmployeeID].Add(ioPairList[i]);
					}

					// get all time schedules for all employees for the given period of time
					foreach (int emplID in employeesID)
					{
                        if (!emplTimeSchedules.ContainsKey(emplID))
                            emplTimeSchedules.Add(emplID, GetEmployeeTimeSchedules(emplID, dtpFromDate.Value, dtpToDate.Value));
					}
                    					                    
                    // key is employee id, value is dictionary of hours by pass types
                    Dictionary<int, Dictionary<int, TimeSpan>> emplPTHours = new Dictionary<int,Dictionary<int,TimeSpan>>();
                    // key is employee id, value is dictionary of hours by pass types by day
                    Dictionary<int, Dictionary<DateTime, Dictionary<int, TimeSpan>>> emplDayPTHours = new Dictionary<int,Dictionary<DateTime,Dictionary<int,TimeSpan>>>();

                    // calculate hours by pass types for each employee
					foreach(int employeeID in employeesID)
					{
                        emplDayPTHours.Add(employeeID, new Dictionary<DateTime,Dictionary<int,TimeSpan>>());

                        // key is date, value is extra hours that are not used for that date
                        Dictionary<DateTime, int> totalExtraHours = new Dictionary<DateTime, int>();

                        // calculate extra hours total
                        List<ExtraHourTO> extraHoursTotalList = new ExtraHour().SearchEmployeeAvailableDates(employeeID);

                        int extraHoursTotalSum = 0;
                        foreach (ExtraHourTO extraHour in extraHoursTotalList)
                        {
                            if (!totalExtraHours.ContainsKey(extraHour.DateEarned.Date))
                                totalExtraHours.Add(extraHour.DateEarned.Date, extraHour.ExtraTimeAmt);
                            else
                                totalExtraHours[extraHour.DateEarned.Date] += extraHour.ExtraTimeAmt;

                            extraHoursTotalSum += extraHour.ExtraTimeAmt;
                        }
                                                
                        // key is pass type id, value is total hours of that pass type
						Dictionary<int, TimeSpan> ptHours = new Dictionary<int,TimeSpan>();

                        // calculate for each day
						for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
						{
							bool isRegularSchema = true; // for future use, if there are night shifts

                            // get time schema intervals for current day
                            Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
							if (edi == null) continue;

                            Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
							IDictionaryEnumerator ediEnum = edi.GetEnumerator();
							while(ediEnum.MoveNext())
							{
                                employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
							}

                            // get pairs for current day
							List<IOPairTO> edp = GetEmployeeDayPairs(emplPairs[employeeID],isRegularSchema,day);
							List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
							foreach(IOPairTO ioPairTO in edp)
							{
								employeeDayPairs.Add(new IOPairTO(ioPairTO));
							}

                            Dictionary<int, TimeSpan> dayPTHours = new Dictionary<int,TimeSpan>();
							
                            if (isRegularSchema)
							{
                                dayPTHours = CalculatePTHoursPerRegularEmployeeDay(employeeID, employeeDayPairs, employeeDayIntervals, day, totalExtraHours);
							}
                            //else
                            //{
                            //    dayPTHours = CalculatePTHoursPerBrigadeEmployeeDay(employeeID, employeeDayPairs, employeeDayIntervals, day);
                            //}

							IDictionaryEnumerator dayPTHoursEnum = dayPTHours.GetEnumerator();
							while(dayPTHoursEnum.MoveNext())
							{
                                if (!ptHours.ContainsKey((int)dayPTHoursEnum.Key))
								{
                                    ptHours.Add((int)dayPTHoursEnum.Key, (TimeSpan)dayPTHoursEnum.Value);
								}
								else
								{
                                    ptHours[(int)dayPTHoursEnum.Key] = ((TimeSpan)ptHours[(int)dayPTHoursEnum.Key].Add((TimeSpan)dayPTHoursEnum.Value));
								}
							}

                            // add hours to daily sum for employee
                            emplDayPTHours[employeeID].Add(day, dayPTHours);
						}

                        if (extraHoursTotalSum > 0)
                        {
                            int days = extraHoursTotalSum / (24 * 60);
                            int totalMinTotal = extraHoursTotalSum % (24 * 60);
                            int hoursTotal = totalMinTotal / 60;
                            totalMinTotal = totalMinTotal % 60;
                            TimeSpan extraHoursTotal = new TimeSpan(days, hoursTotal, totalMinTotal, 0);

                            ptHours.Add(EXTRAHOURSTOTAL, extraHoursTotal);
                        }
                        else
                            ptHours.Add(EXTRAHOURSTOTAL, new TimeSpan(0));
                        
                        // add hours to total sum for employee
                        emplPTHours.Add(employeeID, ptHours);
					}

                    // if there are no data for report
                    if (emplDayPTHours.Count == 0 && emplPTHours.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noCSVExport", culture));
                        return;
                    }

                    // generate report
                    generateReport(emplDayPTHours, emplPTHours, filePath);
				}
				else
				{
					MessageBox.Show (rm.GetString("wrongDatePickUp", culture));
					return;
				}				
								
				this.Close();
			}
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
			finally
			{
				this.Cursor = Cursors.Arrow;
			}            
        }

        private void generateReport(Dictionary<int, Dictionary<DateTime, Dictionary<int, TimeSpan>>> emplDayPTHours, Dictionary<int, Dictionary<int, TimeSpan>> emplPTHours, string filePath)
        {
            try
            {
                // reorganize pass types
                // key is column index, value is pass type in that column
                Dictionary<int, int> ptColumns = new Dictionary<int, int>();

                ptColumns.Add(0, WORK);
                ptColumns.Add(1, PAUSE);
                ptColumns.Add(2, OVERTIME);
                ptColumns.Add(3, HOLIDAY);
                ptColumns.Add(4, WORKONHOLIDAY);

                int colIndex = 5;

                // add passes on reader 
                foreach (int key in passTypes.Keys)
                {
                    if (key != WORK && key != PAUSE && key != OVERTIME && passTypes[key].IsPass == Constants.passOnReader)
                    {
                        ptColumns.Add(colIndex, key);
                        colIndex++;
                    }
                }

                // add whole day absences
                foreach (int key in passTypes.Keys)
                {
                    if (passTypes[key].IsPass == Constants.wholeDayAbsence)
                    {
                        ptColumns.Add(colIndex, key);
                        colIndex++;
                    }
                }

                ptColumns.Add(colIndex, UNEXCUSEDABSENCE);
                colIndex++;
                ptColumns.Add(colIndex, HOURSUSED);
                colIndex++;
                ptColumns.Add(colIndex, EXTRAHOURS);
                colIndex++;
                ptColumns.Add(colIndex, EXTRAHOURSTOTAL);

                if (rbXLS.Checked)
                    generateXLSReport(emplDayPTHours, emplPTHours, filePath, ptColumns);
                else
                    generateCSVReport(emplDayPTHours, emplPTHours, filePath, ptColumns);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.generateReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateXLSReport(Dictionary<int, Dictionary<DateTime, Dictionary<int, TimeSpan>>> emplDayPTHours, Dictionary<int, Dictionary<int, TimeSpan>> emplPTHours, string filePath, Dictionary<int, int> ptColumns)
        {
            try
            {
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                object misValue = System.Reflection.Missing.Value;

                int i = 1;
                int j = 3;

                // insert header
                ws.Cells[i, 1] = "Ime i prezime";
                ws.Cells[i, 2] = "Ident broj";
                foreach (int key in ptColumns.Keys)
                {
                    ws.Cells[i, j] = passTypes[ptColumns[key]].Description.Trim();
                    j++;
                }

                i++;
                if (rbAnalytical.Checked)
                {
                    foreach (int emplID in emplDayPTHours.Keys)
                    {                        
                        if (employeesDic.ContainsKey(emplID))
                            ws.Cells[i, 1] = employeesDic[emplID].LastName.Trim();
                        else
                            ws.Cells[i, 1] = "";

                        // get indent number
                        EmployeeAsco4 emplAsco = new EmployeeAsco4();
                        emplAsco.EmplAsco4TO.EmployeeID = emplID;
                        List<EmployeeAsco4TO> emplAscoTOList = emplAsco.Search();

                        if (emplAscoTOList.Count > 0)
                        {
                            ws.Cells[i, 2] = emplAscoTOList[0].NVarcharValue4.Trim();
                        }
                        else
                            ws.Cells[i, 2] = "";

                        for (j = 0; j < ptColumns.Count; j++)
                        {
                            ws.Cells[i, j + 3] = "";
                        }

                        i++;
                        // pass types hours summary by days
                        foreach (DateTime day in emplDayPTHours[emplID].Keys)
                        {
                            ws.Cells[i, 1] = day.ToString("dd.MM.yyyy.");
                            ws.Cells[i, 2] = "";

                            // get pass types summary data
                            for (j = 0; j < ptColumns.Count; j++)
                            {
                                if (emplDayPTHours[emplID][day].ContainsKey(ptColumns[j]))
                                    ws.Cells[i, j + 3] = String.Format("{0:0.0}", emplDayPTHours[emplID][day][ptColumns[j]].TotalMinutes / 60);
                                else
                                    ws.Cells[i, j + 3] = "0.0";
                            }

                            i++;
                        }
                        
                        // last row with summary data                        
                        ws.Cells[i, 1] = "Ukupno";
                        ws.Cells[i, 2] = "";

                        // get pass types summary data
                        for (j = 0; j < ptColumns.Count; j++)
                        {
                            if (emplPTHours.ContainsKey(emplID) && emplPTHours[emplID].ContainsKey(ptColumns[j]))
                                ws.Cells[i, j + 3] = String.Format("{0:0.0}", emplPTHours[emplID][ptColumns[j]].TotalMinutes / 60);
                            else
                                ws.Cells[i, j + 3] = "0.0";
                        }

                        i++;
                    }
                }
                else
                {
                    foreach (int emplID in emplPTHours.Keys)
                    {
                        if (employeesDic.ContainsKey(emplID))
                            ws.Cells[i, 1] = employeesDic[emplID].LastName.Trim();
                        else
                            ws.Cells[i, 1] = "";

                        // get indent number
                        EmployeeAsco4 emplAsco = new EmployeeAsco4();
                        emplAsco.EmplAsco4TO.EmployeeID = emplID;
                        List<EmployeeAsco4TO> emplAscoTOList = emplAsco.Search();

                        if (emplAscoTOList.Count > 0)
                        {
                            ws.Cells[i, 2] = emplAscoTOList[0].NVarcharValue4.Trim();
                        }
                        else
                            ws.Cells[i, 2] = "";

                        // get pass types summary data
                        for (j = 0; j < ptColumns.Count; j++)
                        {
                            if (emplPTHours[emplID].ContainsKey(ptColumns[j]))
                                ws.Cells[i, j + 3] = String.Format("{0:0.0}", emplPTHours[emplID][ptColumns[j]].TotalMinutes / 60);
                            else
                                ws.Cells[i, j + 3] = "0.0";
                        }

                        i++;
                    }
                }

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;

                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.generateXLSReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PassTypesReport.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private void generateCSVReport(Dictionary<int, Dictionary<DateTime, Dictionary<int, TimeSpan>>> emplDayPTHours, Dictionary<int, Dictionary<int, TimeSpan>> emplPTHours, string filePath, Dictionary<int, int> ptColumns)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("Ime_i_prezime", typeof(System.String));
                table.Columns.Add("Ident_broj", typeof(System.String));

                // Specify the column list to export
                int[] iColumns = new int[ptColumns.Count + 2];
                string[] cHeaders = new string[ptColumns.Count + 2];

                iColumns[0] = 0;
                iColumns[1] = 1;
                cHeaders[0] = "Ime i prezime";
                cHeaders[1] = "Ident broj";

                int index = 2;
                foreach (int key in ptColumns.Keys)
                {
                    // replece empty characters, brackets and % becouse those characters can not be in table columns name
                    table.Columns.Add(passTypes[ptColumns[key]].Description.Trim().Replace(' ', '_').Replace('(', '-').Replace(')', '-').Replace("%", "posto"), typeof(System.String));
                    iColumns[index] = index;
                    cHeaders[index] = passTypes[ptColumns[key]].Description.Trim();
                    index++;
                }

                if (rbAnalytical.Checked)
                {

                    foreach (int emplID in emplDayPTHours.Keys)
                    {
                        // first row with employee data
                        DataRow drEmplData = table.NewRow();

                        if (employeesDic.ContainsKey(emplID))
                            drEmplData[0] = employeesDic[emplID].LastName.Trim();
                        else
                            drEmplData[0] = "";

                        // get indent number
                        EmployeeAsco4 emplAsco = new EmployeeAsco4();
                        emplAsco.EmplAsco4TO.EmployeeID = emplID;
                        List<EmployeeAsco4TO> emplAscoTOList = emplAsco.Search();

                        if (emplAscoTOList.Count > 0)
                        {
                            drEmplData[1] = emplAscoTOList[0].NVarcharValue4.Trim();
                        }
                        else
                            drEmplData[1] = "";

                        for (int i = 0; i < ptColumns.Count; i++)
                        {
                            drEmplData[i + 2] = "";
                        }

                        table.Rows.Add(drEmplData);

                        // pass types hours summary by days
                        foreach (DateTime day in emplDayPTHours[emplID].Keys)
                        {
                            DataRow dr = table.NewRow();

                            dr[0] = day.ToString("dd.MM.yyyy.");
                            dr[1] = "";

                            // get pass types summary data
                            for (int i = 0; i < ptColumns.Count; i++)
                            {
                                if (emplDayPTHours[emplID][day].ContainsKey(ptColumns[i]))
                                    dr[i + 2] = String.Format("{0:0.0}", emplDayPTHours[emplID][day][ptColumns[i]].TotalMinutes / 60);
                                else
                                    dr[i + 2] = "0.0";
                            }

                            table.Rows.Add(dr);
                        }

                        // last row with summary data
                        DataRow drTotal = table.NewRow();

                        drTotal[0] = "Ukupno";
                        drTotal[1] = "";

                        // get pass types summary data
                        for (int i = 0; i < ptColumns.Count; i++)
                        {
                            if (emplPTHours.ContainsKey(emplID) && emplPTHours[emplID].ContainsKey(ptColumns[i]))
                                drTotal[i + 2] = String.Format("{0:0.0}", emplPTHours[emplID][ptColumns[i]].TotalMinutes / 60);
                            else
                                drTotal[i + 2] = "0.0";
                        }

                        table.Rows.Add(drTotal);
                    }
                }
                else
                {
                    foreach (int emplID in emplPTHours.Keys)
                    {
                        DataRow dr = table.NewRow();

                        if (employeesDic.ContainsKey(emplID))
                            dr[0] = employeesDic[emplID].LastName.Trim();
                        else
                            dr[0] = "";

                        // get indent number
                        EmployeeAsco4 emplAsco = new EmployeeAsco4();
                        emplAsco.EmplAsco4TO.EmployeeID = emplID;
                        List<EmployeeAsco4TO> emplAscoTOList = emplAsco.Search();

                        if (emplAscoTOList.Count > 0)
                        {
                            dr[1] = emplAscoTOList[0].NVarcharValue4.Trim();
                        }
                        else
                            dr[1] = "";

                        // get pass types summary data
                        for (int i = 0; i < ptColumns.Count; i++)
                        {
                            if (emplPTHours[emplID].ContainsKey(ptColumns[i]))
                                dr[i + 2] = String.Format("{0:0.0}", emplPTHours[emplID][ptColumns[i]].TotalMinutes / 60);
                            else
                                dr[i + 2] = "0.0";
                        }

                        table.Rows.Add(dr);
                    }
                }

                table.AcceptChanges();

                // Export the details of specified columns to Excel                
                Export objExport = new Export("Win", cHeaders);

                objExport.ExportDetails(table, Export.ExportFormat.CSV, filePath);

                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.generateCSVReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// gets list of employee's time schedules for the given period of time
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private List<EmployeeTimeScheduleTO> GetEmployeeTimeSchedules(int employeeID, DateTime from, DateTime to)
        {
            try
            {
                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

                DateTime date = from.Date;
                while ((date <= to.Date) || (date.Month == to.Month))
                {
                    List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, date);

                    foreach (EmployeeTimeScheduleTO ets in timeSchedule)
                    {
                        timeScheduleList.Add(ets);
                    }

                    date = date.AddMonths(1);
                }

                return timeScheduleList;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.GetEmployeeTimeSchedules(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Gets all the employee's io pairs for the given working day
        /// </summary>
        /// <param name="emplPairs"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private List<IOPairTO> GetEmployeeDayPairs(List<IOPairTO> emplPairs, bool isRegularSchema, DateTime day)
        {
            try
            {
                List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
                foreach (IOPairTO iopair in emplPairs)
                {
                    if (iopair.IOPairDate == day)
                    {
                        employeeDayPairs.Add(iopair);
                    }

                    // pairs that belong to the tomorrow's part of the night shift (00:00-07:00)
                    if (!isRegularSchema && (iopair.IOPairDate == day.AddDays(1) &&
                        iopair.StartTime.TimeOfDay < new TimeSpan(7, 0, 0)))
                    {
                        employeeDayPairs.Add(iopair);
                    }
                }
                return employeeDayPairs;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.GetEmployeeDayPairs(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// gets employee's working intervals for the given day
        /// </summary>
        /// <param name="employeeTimeScheduleList"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private Dictionary<int, WorkTimeIntervalTO> GetDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> employeeTimeScheduleList, DateTime day, ref bool isRegularSchema)
        {
            try
            {
                // find actual time schedule for the day
                int timeScheduleIndex = -1;
                for (int scheduleIndex = 0; scheduleIndex < employeeTimeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (day >= employeeTimeScheduleList[scheduleIndex].Date)
                    //&& (day.Month == ((EmployeesTimeSchedule) (employeeTimeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }
                if (timeScheduleIndex == -1) return null;

                EmployeeTimeScheduleTO employeeTimeSchedule = employeeTimeScheduleList[timeScheduleIndex];

                // find actual time schema for the day
                WorkTimeSchemaTO actualTimeSchema = null;
                if (timeSchemas.ContainsKey(employeeTimeSchedule.TimeSchemaID))
                    actualTimeSchema = timeSchemas[employeeTimeSchedule.TimeSchemaID];

                if (actualTimeSchema == null) return null;

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                //int dayNum = (employeeTimeSchedule.StartCycleDay + day.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
                TimeSpan ts = new TimeSpan(day.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
                int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

                Dictionary<int, WorkTimeIntervalTO> intervals = actualTimeSchema.Days[dayNum];

                isRegularSchema = !isNightShiftDay(intervals);

                return intervals;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.GetDayTimeSchemaIntervals(): " + ex.Message + "\n");
                throw ex;
            }
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        bool isWeekend(DateTime day)
        {
            return ((day.DayOfWeek == DayOfWeek.Saturday) || (day.DayOfWeek == DayOfWeek.Sunday));
        }
        
        bool isNightShiftDay(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            try
            {
                // see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
                IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator();
                while (dayIntervalsEnum.MoveNext())
                {
                    WorkTimeIntervalTO dayInterval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
                    if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)) ||
                        (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))) &&
                        (dayInterval.EndTime > dayInterval.StartTime))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.isNightShiftDay(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// calculate hours per each pass type for one employee and one day
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="ioPairs"></param>
        /// <param name="dayIntervals"></param>
        /// <param name="day"></param>
        private Dictionary<int, TimeSpan> CalculatePTHoursPerRegularEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day, Dictionary<DateTime, int> extraHours)
        {
            try
            {
                // key is pass type, value is total hours of that pass type
                Dictionary<int, TimeSpan> ptHours = new Dictionary<int, TimeSpan>();

                // if weekend day do nothing
                if (!isWeekend(day))
                {
                    // remove unneeded pairs and trim valid pairs
                    bool isWholeDayAbsence = CleanUpDayPairs(ioPairs, dayIntervals, day);

                    // if holiday day add intervals length to the pass type id of holiday and if there are pairs, add their length to work on holiday type
                    if (isHoliday(day))
                    {
                        foreach (WorkTimeIntervalTO interval in dayIntervals.Values)
                        {
                            if (!ptHours.ContainsKey(HOLIDAY))
                                ptHours.Add(HOLIDAY, interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay));
                            else
                                ptHours[HOLIDAY] = ptHours[HOLIDAY].Add(interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay));
                        }

                        foreach (IOPairTO pair in ioPairs)
                        {
                            if (passTypes.ContainsKey(pair.PassTypeID) && passTypes[pair.PassTypeID].IsPass != Constants.wholeDayAbsence)
                            {
                                if (!ptHours.ContainsKey(WORKONHOLIDAY))
                                    ptHours.Add(WORKONHOLIDAY, pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay));
                                else
                                    ptHours[WORKONHOLIDAY] = ptHours[WORKONHOLIDAY].Add(pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay));
                            }
                        }
                    }
                    else
                    {
                        // if whole day absence add hours of corresponding absence, ignore pairs of other type
                        if (isWholeDayAbsence)
                        {
                            foreach (IOPairTO iopair in ioPairs)
                            {
                                if (passTypes.ContainsKey(iopair.PassTypeID) && passTypes[iopair.PassTypeID].IsPass == Constants.wholeDayAbsence)
                                {
                                    if (!ptHours.ContainsKey(iopair.PassTypeID))
                                        ptHours.Add(iopair.PassTypeID, iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay));
                                    else
                                        ptHours[iopair.PassTypeID] = ptHours[iopair.PassTypeID].Add(iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay));
                                }
                            }
                        }
                        else
                        {
                            // if no valid pairs set latency for the whole day
                            if (ioPairs.Count <= 0)
                            {
                                foreach (WorkTimeIntervalTO interval in dayIntervals.Values)
                                {
                                    if (!ptHours.ContainsKey(UNEXCUSEDABSENCE))
                                        ptHours.Add(UNEXCUSEDABSENCE, interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay));
                                    else
                                        ptHours[UNEXCUSEDABSENCE] = ptHours[UNEXCUSEDABSENCE].Add(interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay));
                                }
                            }
                            else
                            {
                                // sum duration of all pairs
                                foreach (IOPairTO iopair in ioPairs)
                                {
                                    if (!ptHours.ContainsKey(iopair.PassTypeID))
                                        ptHours.Add(iopair.PassTypeID, iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay));
                                    else
                                        ptHours[iopair.PassTypeID] = ptHours[iopair.PassTypeID].Add(iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay));
                                }
                                
                                // calculate latenance for first pair
                                int intervalIndex = 0;
                                while (intervalIndex >= 0 && intervalIndex < dayIntervals.Count)
                                {
                                    if (ioPairs[0].StartTime.TimeOfDay > dayIntervals[intervalIndex].StartTime.TimeOfDay)
                                    {
                                        DateTime end = ioPairs[0].StartTime;
                                        if (dayIntervals[intervalIndex].EndTime.TimeOfDay < ioPairs[0].StartTime.TimeOfDay)
                                            end = dayIntervals[intervalIndex].EndTime;

                                        if (!ptHours.ContainsKey(UNEXCUSEDABSENCE))
                                            ptHours.Add(UNEXCUSEDABSENCE, end.TimeOfDay.Subtract(dayIntervals[intervalIndex].StartTime.TimeOfDay));
                                        else
                                            ptHours[UNEXCUSEDABSENCE] = ptHours[UNEXCUSEDABSENCE].Add(end.TimeOfDay.Subtract(dayIntervals[intervalIndex].StartTime.TimeOfDay));

                                        if (ioPairs[0].StartTime.TimeOfDay <= dayIntervals[intervalIndex].EndTime.TimeOfDay)
                                            intervalIndex = -1;
                                        else
                                            intervalIndex++;
                                    }
                                    else
                                        intervalIndex = -1;
                                }

                                // calculate holes between pairs
                                for (int i = 1; i < ioPairs.Count; i++)
                                {
                                    // get candidat for hole
                                    IOPairTO holePair = new IOPairTO(ioPairs[i - 1]);
                                    holePair.StartTime = holePair.EndTime;
                                    holePair.EndTime = ioPairs[i].StartTime;

                                    if (holePair.StartTime < holePair.EndTime)
                                    {
                                        // check if there is interval for hole
                                        foreach (WorkTimeIntervalTO interval in dayIntervals.Values)
                                        {
                                            if ((holePair.StartTime.TimeOfDay <= interval.StartTime.TimeOfDay && holePair.EndTime.TimeOfDay >= interval.StartTime.TimeOfDay)
                                                || (holePair.EndTime.TimeOfDay >= interval.EndTime.TimeOfDay && holePair.StartTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                                                || (holePair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && holePair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay))
                                            {
                                                DateTime start = holePair.StartTime;
                                                if (interval.StartTime.TimeOfDay > holePair.StartTime.TimeOfDay)
                                                    start = interval.StartTime;
                                                DateTime end = holePair.EndTime;
                                                if (interval.EndTime.TimeOfDay < holePair.EndTime.TimeOfDay)
                                                    end = interval.EndTime;
                                                if (!ptHours.ContainsKey(UNEXCUSEDABSENCE))
                                                    ptHours.Add(UNEXCUSEDABSENCE, end.TimeOfDay.Subtract(start.TimeOfDay));
                                                else
                                                    ptHours[UNEXCUSEDABSENCE] = ptHours[UNEXCUSEDABSENCE].Add(end.TimeOfDay.Subtract(start.TimeOfDay));
                                            }
                                        }
                                    }
                                }

                                // calculate hole for last pair
                                intervalIndex = dayIntervals.Count - 1;
                                while (intervalIndex >= 0)
                                {
                                    if (ioPairs[ioPairs.Count - 1].EndTime.TimeOfDay < dayIntervals[intervalIndex].EndTime.TimeOfDay)
                                    {
                                        DateTime start = ioPairs[ioPairs.Count - 1].EndTime;
                                        if (dayIntervals[intervalIndex].StartTime.TimeOfDay > ioPairs[ioPairs.Count - 1].EndTime.TimeOfDay)
                                            start = dayIntervals[intervalIndex].StartTime;

                                        if (!ptHours.ContainsKey(UNEXCUSEDABSENCE))
                                            ptHours.Add(UNEXCUSEDABSENCE, dayIntervals[intervalIndex].EndTime.TimeOfDay.Subtract(start.TimeOfDay));
                                        else
                                            ptHours[UNEXCUSEDABSENCE] = ptHours[UNEXCUSEDABSENCE].Add(dayIntervals[intervalIndex].EndTime.TimeOfDay.Subtract(start.TimeOfDay));

                                        if (ioPairs[ioPairs.Count - 1].EndTime.TimeOfDay >= dayIntervals[intervalIndex].StartTime.TimeOfDay)
                                            intervalIndex = -1;
                                        else
                                            intervalIndex--;
                                    }
                                    else
                                        intervalIndex = -1;
                                }

                                // calculate work hours and pauses
                                TimeSpan workHours = new TimeSpan(0);
                                TimeSpan officialOutHours = new TimeSpan(0);

                                if (ptHours.ContainsKey(WORK))
                                    workHours = ptHours[WORK];
                                if (ptHours.ContainsKey(OFFICIALOUT))
                                    officialOutHours = ptHours[OFFICIALOUT];

                                TimeSpan totalWork = workHours.Add(officialOutHours);

                                if (totalWork > new TimeSpan(4, 0, 0))	// add a pause and subtract pause from working hours
                                {
                                    ptHours.Add(PAUSE, pauseDuration);
                                    if (ptHours.ContainsKey(WORK) && ptHours[WORK] > pauseDuration)
                                        ptHours[WORK] = ptHours[WORK].Subtract(pauseDuration);
                                    else if (ptHours.ContainsKey(OFFICIALOUT) && ptHours[OFFICIALOUT] > pauseDuration)
                                        ptHours[OFFICIALOUT] = ptHours[OFFICIALOUT].Subtract(pauseDuration);
                                }
                            }
                        }
                    }
                }
       
                // calculate extra hours
                int totalMin = 0;

                if (extraHours.ContainsKey(day.Date))
                    totalMin = extraHours[day.Date];

                if (totalMin > 0)
                {
                    int hours = totalMin / 60;
                    totalMin = totalMin % 60;
                    TimeSpan extraHour = new TimeSpan(hours, totalMin, 0);

                    ptHours.Add(EXTRAHOURS, extraHour);
                }
                else
                    ptHours.Add(EXTRAHOURS, new TimeSpan(0));

                // calculate overtime work            
                List<ExtraHourUsedTO> extraHourUsedList = new ExtraHourUsed().Search(employeeID, day, day, Constants.extraHoursUsedOvertime);
                int totalUsedMin = 0;
                foreach (ExtraHourUsedTO used in extraHourUsedList)
                {
                    if (used.ExtraTimeAmtUsed > 0)
                        totalUsedMin += used.ExtraTimeAmtUsed;
                }

                if (totalUsedMin > 0)
                {
                    int hoursUsed = totalUsedMin / 60;
                    totalUsedMin = totalUsedMin % 60;
                    TimeSpan overTime = new TimeSpan(hoursUsed, totalUsedMin, 0);

                    ptHours.Add(OVERTIME, overTime);
                }
                else
                    ptHours.Add(OVERTIME, new TimeSpan(0));

                return ptHours;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.CalculatePTHoursPerRegularEmployeeDay(): " + ex.Message + "\n");
                throw ex;
            }
        }

        //private Dictionary<int, double> CalculatePTHoursPerBrigadeEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        //{
        //    Dictionary<int, double> ptHours = new Dictionary<int, double>();
            
        //    // remove unneeded pairs and trim first and last pair to the working time interval boundaries
        //    // considering latency rules, differently for regular and night shift days
        //    if (!isNightShiftDay(dayIntervals))
        //    {
        //        CleanUpDayPairs(ioPairs, dayIntervals, day);
        //    }
        //    else
        //    {
        //        CleanUpNightPairs(employeeID, ioPairs, dayIntervals, day);
        //    }

        //    // if not working day do nothing
        //    if (!isWorkingDay(dayIntervals)) return ptHours;

        //    TimeSpan expectedHours = is8hoursShift(dayIntervals) ? new TimeSpan(8, 0, 0) : new TimeSpan(12, 0, 0);
        //    TimeSpan expectedBreak = (expectedHours == new TimeSpan(8, 0, 0)) ? new TimeSpan(0, 30, 0) : new TimeSpan(0, 45, 0);
        //    TimeSpan expectedMealTickets = (expectedHours == new TimeSpan(8, 0, 0)) ? new TimeSpan(1, 0, 0) : new TimeSpan(1, 30, 0);

        //    // if no valid pairs set latency for the whole day (payment code 4010) and close the day
        //    if (ioPairs.Count <= 0)
        //    {
        //        ptHours.Add(passTypes[Constants.late].PaymentCode, expectedHours.Subtract(expectedBreak));
        //        return ptHours;
        //    }

        //    // if whole day absences found add 7.5 hours or 11.25 h to its payment code (various) and close the day
        //    foreach (IOPairTO iopair in ioPairs)
        //    {
        //        PassTypeTO passType = passTypes[iopair.PassTypeID];
        //        if (passType.IsPass == Constants.wholeDayAbsence)
        //        {
        //            ptHours.Add(passType.PaymentCode, expectedHours.Subtract(expectedBreak));
        //            return ptHours;
        //        }
        //    }

        //    // sum duration of all pairs (they're all with pass type 1 - prolasci na citacu)
        //    TimeSpan totalDuration = new TimeSpan(0, 0, 0);
        //    foreach (IOPairTO iopair in ioPairs)
        //    {
        //        totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
        //        if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalDuration = totalDuration.Add(new TimeSpan(0, 1, 0));
        //    }

        //    // calculate latency as difference between expected hours and actual duration (already
        //    // calculated considering start and end latencies, round it up to full hour and set
        //    // latency (payment code 4010)
        //    TimeSpan totalLatency = expectedHours - totalDuration;
        //    if (totalLatency.Minutes != 0)
        //    {
        //        totalLatency = totalLatency.Add(new TimeSpan(1, -totalLatency.Minutes, -totalLatency.Seconds));
        //    }

        //    // calculate sum of ordinary private exits (4000 - bez preraspodele) and round it up to full hour
        //    TimeSpan totalPrivateExits = new TimeSpan(0, 0, 0);
        //    foreach (IOPairTO iopair in ioPairs)
        //    {
        //        if (iopair.PassTypeID == Constants.privateOut)
        //        {
        //            totalPrivateExits = totalPrivateExits.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
        //            if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalPrivateExits = totalPrivateExits.Add(new TimeSpan(0, 1, 0));
        //        }
        //    }
        //    if (totalPrivateExits.Minutes != 0)
        //    {
        //        totalPrivateExits = totalPrivateExits.Add(new TimeSpan(1, -totalPrivateExits.Minutes, -totalPrivateExits.Seconds));
        //    }

        //    // calculate sum of night hours (0560 - the ones after 22:00), round it down it to full hour
        //    TimeSpan totalNightHours = new TimeSpan(0, 0, 0);
        //    foreach (IOPairTO iopair in ioPairs)
        //    {
        //        if ((iopair.PassTypeID != Constants.privateOut) && (iopair.EndTime.TimeOfDay > new TimeSpan(22, 0, 0)))
        //        {
        //            TimeSpan startNightWork = new TimeSpan(22, 0, 0);
        //            if (iopair.StartTime.TimeOfDay > startNightWork) startNightWork = iopair.StartTime.TimeOfDay;
        //            totalNightHours = totalNightHours.Add(iopair.EndTime.TimeOfDay - startNightWork);
        //        }
        //        if ((iopair.PassTypeID != Constants.privateOut) && (iopair.StartTime.TimeOfDay < new TimeSpan(6, 0, 0)))
        //        {
        //            TimeSpan endNightWork = new TimeSpan(6, 0, 0);
        //            if (iopair.EndTime.TimeOfDay < endNightWork) endNightWork = iopair.EndTime.TimeOfDay;
        //            totalNightHours = totalNightHours.Add(endNightWork - iopair.StartTime.TimeOfDay);
        //        }
        //        if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalNightHours = totalNightHours.Add(new TimeSpan(0, 1, 0));
        //    }
        //    if (totalNightHours.Minutes != 0)
        //    {
        //        totalNightHours = totalNightHours.Add(new TimeSpan(0, -totalNightHours.Minutes, -totalNightHours.Seconds));
        //    }

        //    // calculate payed hours (payment code 0031) considering break time, and meal tickets (payment code 3000)
        //    TimeSpan totalPayedHours = expectedHours - totalLatency - totalPrivateExits;

        //    if (totalPayedHours > new TimeSpan(expectedHours.Hours / 2, 0, 0))	// add a meal ticket and subtract break time from working hours
        //    {
        //        ptHours.Add(passTypes[Constants.tickets].PaymentCode, expectedMealTickets);
        //        totalPayedHours = totalPayedHours.Subtract(expectedBreak);
        //        //if (totalNightHours > totalPayedHours-totalNightHours) totalNightHours = totalNightHours.Subtract(expectedBreak);

        //        // if working time is 12h, break time is splitted, 15min is calculated in day hours and 30min are calculated in night hours
        //        // if working time is 8h, break time is 30min and is calculated in night hours
        //        if (totalNightHours > totalPayedHours - totalNightHours) totalNightHours = totalNightHours.Subtract(new TimeSpan(0, 30, 0));
        //    }
        //    else	// subtract break time from greater of total latency time and total private exits time
        //    {
        //        if ((totalLatency > totalPrivateExits) && (totalLatency >= expectedBreak))
        //        {
        //            totalLatency = totalLatency.Subtract(expectedBreak);
        //        }
        //        else if (totalPrivateExits >= expectedBreak)
        //        {
        //            totalPrivateExits = totalPrivateExits.Subtract(expectedBreak);
        //        }
        //    }

        //    // set value for the payment code 0031 and 0560 (regular work and work on holiday)
        //    if (totalPayedHours.Hours > 0)
        //    {
        //        ptHours.Add(passTypes[Constants.regularWork].PaymentCode, totalPayedHours);
        //        if (isHoliday(day))
        //        {
        //            ptHours.Add(passTypes[Constants.workOnHoliday].PaymentCode, totalPayedHours);
        //        }
        //    }

        //    // set value for the payment code 4010 (latency)
        //    if (totalLatency.Hours > 0)
        //    {
        //        ptHours.Add(passTypes[Constants.late].PaymentCode, totalLatency);
        //    }

        //    // set value for the payment code 0400 (night work)
        //    if (totalNightHours.Hours > 0)
        //    {
        //        ptHours.Add(passTypes[Constants.nightWork].PaymentCode, totalNightHours);
        //    }

        //    // set value for the payment code 4000 (private exits)
        //    if (totalPrivateExits.Hours > 0)
        //    {
        //        ptHours.Add(passTypes[Constants.privateOut].PaymentCode, totalPrivateExits);
        //    }

        //    return ptHours;
        //}

        /// <summary>
        /// removes unneeded pairs and trim pairs start and end time to whole hours
        /// return value is true if whole day absence pair found (then return and do not do triming pairs) and false otherwise
        /// pairs are sorted by start_time ascending
        /// </summary>
        /// <param name="ioPairs"></param>
        /// <param name="dayIntervals"></param>
        bool CleanUpDayPairs(List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            try
            {
                // remove invalid pairs and pairs outside of the interval and check if some of valid pairs is whole day absence
                // if whole day absence pair found, return, do not do triming pairs, hours will be set considering daily intervals
                IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
                while (ioPairsEnum.MoveNext())
                {
                    IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                    
                    // if invalid
                    if ((iopair.IOPairDate != day) || (iopair.StartTime > iopair.EndTime) ||
                        (iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) ||
                        (iopair.EndTime.TimeOfDay < dayIntervals[0].StartTime.TimeOfDay))
                    {
                        ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                        ioPairsEnum = ioPairs.GetEnumerator();
                    } // if valid
                    else if (!isHoliday(day))
                    {
                        if (passTypes.ContainsKey(iopair.PassTypeID) && passTypes[iopair.PassTypeID].IsPass == Constants.wholeDayAbsence)
                            return true; // whole day absence found, ignore other pairs
                    }
                }

                if (ioPairs.Count <= 0) return false;

                // find first and last pair
                IOPairTO firstPair = ioPairs[0];
                IOPairTO lastPair = ioPairs[ioPairs.Count -1];
                
                // round up first pair start time to first interval start if it begin earlier
                if (firstPair.StartTime.TimeOfDay < dayIntervals[0].StartTime.TimeOfDay)
                {
                    firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[0].StartTime.TimeOfDay);
                }
               
                // round down last pair end time to last interval end time if it end later
                if (lastPair.EndTime.TimeOfDay > dayIntervals[dayIntervals.Count - 1].EndTime.TimeOfDay)
                {
                    lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[dayIntervals.Count - 1].EndTime.TimeOfDay);
                }
                
                // trim pairs
                // regular work start at next hour if exists latenance and end at previous hour if is ended before whole hour
                // other types start at previous hour if exist latenance and end at next hour if are ended before whole hour
                // after other type pair is expanded, go through all later pairs and move thair start to expanded pair end                
                for (int i = 0; i < ioPairs.Count; i++)
                {
                    if (ioPairs[i].PassTypeID == WORK || ioPairs[i].PassTypeID == HOURSUSED)
                    {
                        if (ioPairs[i].StartTime.Minute != 0 || ioPairs[i].StartTime.Second != 0)
                        {
                            ioPairs[i].StartTime = ioPairs[i].StartTime.Add(new TimeSpan(1, -ioPairs[i].StartTime.Minute, -ioPairs[i].StartTime.Second));
                        }
                        if (ioPairs[i].EndTime.Minute != 0 || ioPairs[i].EndTime.Second != 0)
                        {
                            ioPairs[i].EndTime = ioPairs[i].EndTime.Add(new TimeSpan(0, -ioPairs[i].EndTime.Minute, -ioPairs[i].EndTime.Second));
                        }
                        if (ioPairs[i].StartTime > ioPairs[i].EndTime) ioPairs[i].EndTime = ioPairs[i].StartTime;
                    }
                    else //if (passTypes.ContainsKey(iopair.PassTypeID) && passTypes[iopair.PassTypeID].IsPass == Constants.passOnReader)
                    {
                        if (ioPairs[i].StartTime.Minute != 0 || ioPairs[i].StartTime.Second != 0)
                        {
                            ioPairs[i].StartTime = ioPairs[i].StartTime.Add(new TimeSpan(0, -ioPairs[i].StartTime.Minute, -ioPairs[i].StartTime.Second));
                        }
                        if (ioPairs[i].EndTime.Minute != 0 || ioPairs[i].EndTime.Second != 0)
                        {
                            ioPairs[i].EndTime = ioPairs[i].EndTime.Add(new TimeSpan(1, -ioPairs[i].EndTime.Minute, -ioPairs[i].EndTime.Second));
                        }
                        if (ioPairs[i].StartTime > ioPairs[i].EndTime) ioPairs[i].StartTime = ioPairs[i].EndTime;

                        for (int j = i + 1; j < ioPairs.Count; j++)
                        {
                            if (ioPairs[j].StartTime < ioPairs[i].EndTime)
                                ioPairs[j].StartTime = ioPairs[i].EndTime;
                        }
                    }
                }

                return false; // no whole day absences
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.CleanUpDayPairs(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void chbShowRetired_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmplCombo((int)cbWorkingUnit.SelectedValue);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PassTypesReport.chbShowRetired_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// removes unneeded pairs belonging to the night shift and trim first and last pair
        /// to the working time interval boundaries considering latency rules
        /// </summary>
        /// <param name="ioPairs"></param>
        /// <param name="dayIntervals"></param>
        //void CleanUpNightPairs(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        //{
        //    // night shift day can have 1 or 2 intervals
        //    if (dayIntervals.Count == 1)
        //    {
        //        // if night shift day has only 00:00- interval it's already calculated as yesterday
        //        // and should be dropped
        //        if (dayIntervals[0].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
        //        {
        //            ioPairs.Clear();
        //            dayIntervals.Clear();
        //            return;
        //        }
        //        // if night shift day has only -23:59 interval make it normal night shift day adding
        //        // tomorrow's first interval
        //        else
        //        {
        //            dayIntervals.Add(1, dayIntervals[0]);
        //            bool dummy = true;
        //            Dictionary<int, WorkTimeIntervalTO> tomorrowIntervals = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day.AddDays(1), ref dummy);
        //            if (tomorrowIntervals != null) dayIntervals[0] = tomorrowIntervals[0];
        //            else
        //            {
        //                debug.writeLog(DateTime.Now + " PassTypesReport.CleanUpNightPairs(): Tomorrow intervals cannot be found! Employee: " + employeeID.ToString() + " Day: " + day.ToShortDateString() + "\n");
        //                return;
        //            }
        //        }
        //    }

        //    // remove invalid pairs and pairs outside of the interval
        //    IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
        //    while (ioPairsEnum.MoveNext())
        //    {
        //        IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
        //        if (
        //            (iopair.StartTime > iopair.EndTime) ||
        //            (
        //            (iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) &&
        //            (iopair.StartTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay) &&
        //            (iopair.EndTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) &&
        //            (iopair.EndTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay)
        //            )
        //            )
        //        {
        //            ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
        //            ioPairsEnum = ioPairs.GetEnumerator();
        //        }
        //    }
        //    if (ioPairs.Count <= 0) return;

        //    // remove morning pairs belonging to the previous day
        //    ioPairsEnum = ioPairs.GetEnumerator();
        //    while (ioPairsEnum.MoveNext())
        //    {
        //        IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
        //        if ((iopair.IOPairDate == day) && (iopair.StartTime.TimeOfDay < dayIntervals[0].EndTime.TimeOfDay))
        //        {
        //            ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
        //            ioPairsEnum = ioPairs.GetEnumerator();
        //        }
        //    }
        //    if (ioPairs.Count <= 0) return;

        //    // find first and last pair
        //    IOPairTO firstPair = (IOPairTO)ioPairs[0];
        //    IOPairTO lastPair = firstPair;
        //    foreach (IOPairTO iopair in ioPairs)
        //    {
        //        if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
        //        if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
        //    }

        //    // find the proper interval for the first and last pair considering the day
        //    int firstPairInterval = (firstPair.IOPairDate == day) ? 1 : 0;
        //    int lastPairInterval = (lastPair.IOPairDate == day) ? 1 : 0;

        //    // round up first pair start time to full hour, considering start tolerance
        //    if (firstPair.StartTime.TimeOfDay < dayIntervals[firstPairInterval].StartTime.TimeOfDay)
        //    {
        //        firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[firstPairInterval].StartTime.TimeOfDay);
        //    }
        //    else
        //    {
        //        if (firstPair.StartTime.Minute != 0 || firstPair.StartTime.Second != 0)
        //        {
        //            firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1, -firstPair.StartTime.Minute, -firstPair.StartTime.Second));
        //        }
        //    }
        //    if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

        //    // round down last pair end time to full hour, considering end tolerance
        //    if (lastPair.EndTime.TimeOfDay > dayIntervals[lastPairInterval].EndTime.TimeOfDay)
        //    {
        //        lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[lastPairInterval].EndTime.TimeOfDay);
        //    }
        //    else
        //    {
        //        if (lastPair.EndTime.Minute != 0 || lastPair.EndTime.Second != 0)
        //        {
        //            lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0, -lastPair.EndTime.Minute, -lastPair.EndTime.Second));
        //        }
        //    }
        //    if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        //}
    }
}
