using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;
using System.Collections;



namespace Reports.Magna
{
    public partial class MagnaPresenceTracking : Form
    {

        // Language settings
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog debug;
        ApplUserTO logInUser;

        List<WorkingUnitTO> wUnits;

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        DateTime from = new DateTime();
        DateTime to = new DateTime();


        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime, HolidayTO>();

        // key is legend description, and value is counter how many of them has been found same
        Dictionary<string, int> legendDescriptions = new Dictionary<string, int>();

        Filter filter;


        public MagnaPresenceTracking()
        {
            InitializeComponent();



            this.CenterToScreen();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();
            wUnits = new List<WorkingUnitTO>();

            // Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            setLanguage();

            // New Document name
            //newDocFileName = "Mesecni izvestaj o prisustvu zaposlenih-";

            // get all time schemas
            timeSchemas = new TimeSchema().Search();

        }

        private void setLanguage()
        {
            this.Text = rm.GetString("presenceTracking", culture);

            gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
            gbFor.Text = rm.GetString("gbFor", culture);
            gbFilter.Text = rm.GetString("gbFilter", culture);
            gbOverview.Text = rm.GetString("gbOverview", culture);

            //radio button's text
            rbNumOfHours.Text = rm.GetString("rbNumOfHours", culture);
            rbPresence.Text = rm.GetString("rbPresence", culture);

            chbHierarhicly.Text = rm.GetString("hierarchically", culture);

            this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            this.btnCancel.Text = rm.GetString("btnCancel", culture);

            this.lblWorkingUnit.Text = rm.GetString("lblName", culture);
            this.lblFor.Text = rm.GetString("lblFor", culture);
            lblReportType.Text = rm.GetString("lblDocFormat", culture);
            this.chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
        }

        private void MagnaPresenceTracking_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateWorkigUnitCombo();
                dtTo.Value = DateTime.Now.Date;
                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.PresenceTracking_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWorkigUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString(), -1));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                //debug.writeLog(DateTime.Now + " PresenceTracking.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;
                legendDescriptions = new Dictionary<string, int>();
                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    if (this.cbWorkingUnit.SelectedIndex.Equals(0))
                    {
                        MessageBox.Show(rm.GetString("noWUSelected", culture));
                        return;
                    }

                    int wuID = -1;
                    string wUnitsString = "";
                    List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                    wuID = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhicly.Checked)
                    {
                        WorkingUnit wuElement = new WorkingUnit();
                        wuElement.WUTO.WorkingUnitID = wuID;
                        wuArray = wuElement.Search();
                        wuArray = wuElement.FindAllChildren(wuArray);

                        wuID = -1;
                    }

                    foreach (WorkingUnitTO wuEl in wuArray)
                    {
                        wUnitsString += wuEl.WorkingUnitID.ToString().Trim() + ",";
                    }
                    if (wUnitsString.Length > 0)
                    {
                        wUnitsString = wUnitsString.Substring(0, wUnitsString.Length - 1);
                    }
                    else
                    {
                        wUnitsString = wuID.ToString().Trim();
                    }

                    // Get IOPairs
                    //ArrayList data = prepareData();
                    List<IOPairTO> data = prepareData(wuID, wUnitsString);

                    // Sanja: changed 04.02.2009. - to show all employees from selected working units
                    //ArrayList emplList = getEmployees(wuID, wUnitsString);
                    List<EmployeeTO> emplList = new Employee().SearchByWU(wUnitsString);
                    List<EmployeeTO> employeesList = new List<EmployeeTO>();

                    foreach (EmployeeTO employee in emplList)
                    {
                        if (this.chbShowRetired.Checked || !employee.Status.Equals(Constants.statusRetired))
                        {
                            employeesList.Add(employee);
                        }
                    }

                    #region comment
                    /*string wu = rm.GetString("all"); 
					if (cbWorkingUnit.SelectedIndex > 0)
					{
						wu = cbWorkingUnit.Text.Trim();
					}

					string month = dtTo.Value.ToString("MM_yyy");

					
					if (this.cbPDF.Checked)
					{
						PrepareDocument(wu, month);
						setHeaders();

						int count = getEmployeesCount();
						if (count > Constants.maxMonthlyReportRecords)
						{
							this.Cursor = Cursors.Arrow;
							MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
							return;
						}
						else if (count > Constants.warningMonthlyReportRecords)
						{
							this.Cursor = Cursors.Arrow;
							DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.No))
							{
								return;
							}
						}

						this.Cursor = Cursors.WaitCursor;
						//ArrayList employeesList = getEmployees();
						ArrayList ioPairForDay = new ArrayList();

						DateTime currentDay = from;
						IOPair currentIOPair = new IOPair();

						//PDFDocument cont = new PDFDocument();
						//cont.Read(Constants.pdfTemplatePath + "\\" + templateFileNamePage2);

						int k=0;
						int i = 0;
						int RowPos = 0;

						// Goes through the first 10 of all of employees
						foreach(Employee currentEmployee in employeesList)
						{
							// Write Employee Name
							if ((k < 9) && (this.doc.PageCount == 1))
							{
								//writeName(k, (currentEmployee.LastName.Trim() + " " + currentEmployee.FirstName.Trim()), this.doc);
							}
							else if ((k == 9) && (this.doc.PageCount == 1))
							{
								//this.doc.Append(cont);

								this.doc.PageNumber = this.doc.PageCount;
								RowPos = 0;
								tableY =  420;
						
							}
							else if (((this.doc.PageCount > 1) && ((k-9) % 13) == 0))
							{
								this.doc.Append(cont);
								this.doc.PageNumber = this.doc.PageCount;
								RowPos = 0;
								tableY = 420;
						
							}

							writeName(k, RowPos, (currentEmployee.LastName.Trim() + " " + currentEmployee.FirstName.Trim()), this.doc);

							while(currentDay < to)
							{
								for (int j=0; j < data.Count; j++)
								{
							
									currentIOPair = (IOPair) data[j];

									if ((currentIOPair.EmployeeID.Equals(currentEmployee.EmployeeID)) 
										&& (currentIOPair.IOPairDate.Date.Equals(currentDay.Date)))
									{
										ioPairForDay.Add(currentIOPair);
									}
								}
					
								// Write sign

								writeSign(analyzeData(ioPairForDay), i++, RowPos, this.doc);

								ioPairForDay.Clear();
								currentDay = currentDay.AddDays(1);
							}
							i=0;
							k++;
							RowPos++;
							currentDay = from;
						}

						// Save and Show document
						doc.TextStyle.Bold = false;

						this.doc.FontSize = Constants.pdfFontSize11;
						this.doc.InsertFooter(doc.FontSize);
						this.doc.Save();
						debug.writeLog(DateTime.Now + " PresenceTracking OPEN Document: Started! \n");
						doc.Open();
						debug.writeLog(DateTime.Now + " PresenceTracking OPEN Document: Finished! \n");
						this.doc.Clear();
						this.Close();
					}
					*/
                    #endregion

                    if (this.cbCR.Checked)
                    {
                        try
                        {
                            DataSet crData = PrepareCRData(data, employeesList);
                            DataTable table = crData.Tables["employee_presence"];

                            if (table.Rows.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("dataNotFound", culture));
                                return;
                            }

                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports_sr.EmployeePresenceCRView view =
                                    new Reports_sr.EmployeePresenceCRView(crData, this.dtTo.Value);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports_en.EmployeePresenceCRView_en view =
                                    new Reports_en.EmployeePresenceCRView_en(crData, this.dtTo.Value);
                                view.ShowDialog(this);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<IOPairTO> prepareData(int wuID, string wUnitsString)
        {
            List<IOPairTO> ioPairsList = new List<IOPairTO>();
            try
            {
                IOPair ioPair = new IOPair();
                DateTime selectd = dtTo.Value;

                this.from = new DateTime(selectd.Year, selectd.Month, 1, 0, 0, 0);
                this.to = this.from.AddMonths(1);
                //this.to = new DateTime(selectd.Year, selectd.Month + 1, 1, 0, 0 ,0);
                ioPairsList = ioPair.SearchForPresence(wuID, wUnitsString, from, to);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.prepareData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return ioPairsList;
        }

        private DataSet PrepareCRData(List<IOPairTO> ioPairs, List<EmployeeTO> employeeList)
        {
            Hashtable pTypes = new Hashtable(); // key is pass_type_id, value is short description from legend
            DataSet dataSet = new DataSet();
            DataTable table = dataSet.Tables.Add("employee_presence");

            table.Columns.Add("employee_id", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("working_unit", typeof(System.String));
            table.Columns.Add("date", typeof(DateTime));

            table.Columns.Add("day_1", typeof(System.String));
            table.Columns.Add("day_2", typeof(System.String));
            table.Columns.Add("day_3", typeof(System.String));
            table.Columns.Add("day_4", typeof(System.String));
            table.Columns.Add("day_5", typeof(System.String));
            table.Columns.Add("day_6", typeof(System.String));
            table.Columns.Add("day_7", typeof(System.String));
            table.Columns.Add("day_8", typeof(System.String));
            table.Columns.Add("day_9", typeof(System.String));
            table.Columns.Add("day_10", typeof(System.String));
            table.Columns.Add("day_11", typeof(System.String));
            table.Columns.Add("day_12", typeof(System.String));
            table.Columns.Add("day_13", typeof(System.String));
            table.Columns.Add("day_14", typeof(System.String));
            table.Columns.Add("day_15", typeof(System.String));
            table.Columns.Add("day_16", typeof(System.String));
            table.Columns.Add("day_17", typeof(System.String));
            table.Columns.Add("day_18", typeof(System.String));
            table.Columns.Add("day_19", typeof(System.String));
            table.Columns.Add("day_20", typeof(System.String));
            table.Columns.Add("day_21", typeof(System.String));
            table.Columns.Add("day_22", typeof(System.String));
            table.Columns.Add("day_23", typeof(System.String));
            table.Columns.Add("day_24", typeof(System.String));
            table.Columns.Add("day_25", typeof(System.String));
            table.Columns.Add("day_26", typeof(System.String));
            table.Columns.Add("day_27", typeof(System.String));
            table.Columns.Add("day_28", typeof(System.String));
            table.Columns.Add("day_29", typeof(System.String));
            table.Columns.Add("day_30", typeof(System.String));
            table.Columns.Add("day_31", typeof(System.String));
            table.Columns.Add("total", typeof(System.Int32));
            table.Columns.Add("numOfDays", typeof(System.String));
            table.Columns.Add("totalAbsence", typeof(System.Int32));
            table.Columns.Add("totalNS", typeof(System.Int32));
            table.Columns.Add("imageID", typeof(byte));
            DataTable tableI = dataSet.Tables.Add("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

            DataTable tablePT = dataSet.Tables.Add("pass_types");
            tablePT.Columns.Add("pt_0", typeof(System.String));
            tablePT.Columns.Add("pt_1", typeof(System.String));
            tablePT.Columns.Add("pt_2", typeof(System.String));
            tablePT.Columns.Add("pt_3", typeof(System.String));
            tablePT.Columns.Add("pt_4", typeof(System.String));
            tablePT.Columns.Add("pt_5", typeof(System.String));
            tablePT.Columns.Add("pt_6", typeof(System.String));
            tablePT.Columns.Add("pt_7", typeof(System.String));
            tablePT.Columns.Add("pt_8", typeof(System.String));
            tablePT.Columns.Add("pt_9", typeof(System.String));
            tablePT.Columns.Add("pt_10", typeof(System.String));
            tablePT.Columns.Add("pt_11", typeof(System.String));
            tablePT.Columns.Add("pt_12", typeof(System.String));
            tablePT.Columns.Add("pt_13", typeof(System.String));
            tablePT.Columns.Add("pt_14", typeof(System.String));
            tablePT.Columns.Add("pt_15", typeof(System.String));
            tablePT.Columns.Add("pt_16", typeof(System.String));
            tablePT.Columns.Add("pt_17", typeof(System.String));
            tablePT.Columns.Add("pt_18", typeof(System.String));
            tablePT.Columns.Add("pt_19", typeof(System.String));
            tablePT.Columns.Add("pt_20", typeof(System.String));
            tablePT.Columns.Add("pt_21", typeof(System.String));
            tablePT.Columns.Add("pt_22", typeof(System.String));
            tablePT.Columns.Add("pt_23", typeof(System.String));
            tablePT.Columns.Add("pt_24", typeof(System.String));
            tablePT.Columns.Add("pt_0_desc", typeof(System.String));
            tablePT.Columns.Add("pt_1_desc", typeof(System.String));
            tablePT.Columns.Add("pt_2_desc", typeof(System.String));
            tablePT.Columns.Add("pt_3_desc", typeof(System.String));
            tablePT.Columns.Add("pt_4_desc", typeof(System.String));
            tablePT.Columns.Add("pt_5_desc", typeof(System.String));
            tablePT.Columns.Add("pt_6_desc", typeof(System.String));
            tablePT.Columns.Add("pt_7_desc", typeof(System.String));
            tablePT.Columns.Add("pt_8_desc", typeof(System.String));
            tablePT.Columns.Add("pt_9_desc", typeof(System.String));
            tablePT.Columns.Add("pt_10_desc", typeof(System.String));
            tablePT.Columns.Add("pt_11_desc", typeof(System.String));
            tablePT.Columns.Add("pt_12_desc", typeof(System.String));
            tablePT.Columns.Add("pt_13_desc", typeof(System.String));
            tablePT.Columns.Add("pt_14_desc", typeof(System.String));
            tablePT.Columns.Add("pt_15_desc", typeof(System.String));
            tablePT.Columns.Add("pt_16_desc", typeof(System.String));
            tablePT.Columns.Add("pt_17_desc", typeof(System.String));
            tablePT.Columns.Add("pt_18_desc", typeof(System.String));
            tablePT.Columns.Add("pt_19_desc", typeof(System.String));
            tablePT.Columns.Add("pt_20_desc", typeof(System.String));
            tablePT.Columns.Add("pt_21_desc", typeof(System.String));
            tablePT.Columns.Add("pt_22_desc", typeof(System.String));
            tablePT.Columns.Add("pt_23_desc", typeof(System.String));
            tablePT.Columns.Add("pt_24_desc", typeof(System.String));

            // populate pass_types
            List<PassTypeTO> passTypesAll = new PassType().Search();
            List<PassTypeTO> passTypes = new List<PassTypeTO>();

            foreach (PassTypeTO pt in passTypesAll)
            {
                if (pt.PassTypeID == Constants.regularWork || pt.IsPass != Constants.passOnReader)
                {
                    passTypes.Add(pt);
                }
            }

            int index = 0;
            int ptLegend = 0;
            DataRow rowPT = tablePT.NewRow();
            bool addRow = false;
            while (ptLegend < Constants.maxPassTypes && index < passTypes.Count)
            {
                if (passTypes[index].PassTypeID >= 0 || passTypes[index].PassTypeID == Constants.extraHours)
                {
                    string legendDesc = "";
                    if (passTypes[index].PassTypeID > 0 || passTypes[index].PassTypeID == Constants.extraHours)
                    {
                        legendDesc = makeLegend(passTypes[index].Description);
                    }
                    else
                    {
                        if (rbPresence.Checked)
                            legendDesc = Constants.passTypeR;
                        else
                            legendDesc = Constants.passTypeR1;
                    }

                    if (!legendDesc.Trim().Equals(""))
                    {
                        rowPT["pt_" + ptLegend.ToString().Trim()] = legendDesc;
                        rowPT["pt_" + ptLegend.ToString().Trim() + "_desc"] = " - " + passTypes[index].Description;
                        addRow = true;

                        pTypes.Add(passTypes[index].PassTypeID, rowPT["pt_" + ptLegend.ToString().Trim()]);
                    }

                    ptLegend++;
                }

                index++;
            }

            if (addRow)
            {
                tablePT.Rows.Add(rowPT);
            }

            Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>(); // Key: Day, Value:IOPair List
            List<IOPairTO> pairDay = new List<IOPairTO>();
            IOPairTO pairTemp = new IOPairTO();

            try
            {
                int counter = 0;
                foreach (EmployeeTO employee in employeeList)
                {

                    List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1), new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1).AddMonths(1).AddDays(-1));
                    int lastDay = new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1).AddMonths(1).AddDays(-1).Day;
                    List<IOPairTO> pairs = new List<IOPairTO>();
                    DataRow row = table.NewRow();

                    row["employee_id"] = employee.EmployeeID;
                    row["last_name"] = employee.LastName + " " + employee.FirstName;
                    row["first_name"] = employee.FirstName;
                    row["working_unit"] = employee.WorkingUnitName;
                    row["date"] = dtTo.Value.Date;

                    // Init Values
                    for (int i = 1; i < 32; i++)
                    {
                        row["day_" + i.ToString()] = "";
                    }

                    foreach (IOPairTO pair in ioPairs)
                    {
                        if (pair.EmployeeID.Equals(employee.EmployeeID))
                        {
                            if (emplPairs.ContainsKey(pair.IOPairDate.Day))
                            {
                                List<IOPairTO> existing = emplPairs[pair.IOPairDate.Day];
                                existing.Add(pair);
                            }
                            else
                            {
                                List<IOPairTO> newDay = new List<IOPairTO>();
                                newDay.Add(pair);
                                emplPairs.Add(pair.IOPairDate.Day, newDay);
                            }
                            pairs.Add(pair);
                        }
                    }
                    int totalMinutes = 0;
                    int totalMinutesAbsence = 0;
                    int totalNightShift = 0;
                    int numOfDays = 0;
                    bool shouldWork = false;
                    for (int date = 1; date <= lastDay; date++)
                    {
                        //pairDay = (ArrayList)emplPairs[date];
                        int PassAbsence = 0;
                        shouldWork = false;
                        DateTime day = new DateTime(dtTo.Value.Year, dtTo.Value.Month, date);

                        //28.10.2009 Natasa Calculate hours insted of marking days with regular work
                        bool is2DayShift = false;
                        bool is2DaysShiftPrevious = false;
                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                        //get intervls for employee and day
                        Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                        Employee empl = new Employee();
                        empl.EmplTO = employee;
                        ArrayList schemas = empl.findTimeSchema(day);
                        if (schemas.Count > 0)
                        {
                            WorkTimeSchemaTO timeSchema = (WorkTimeSchemaTO)schemas[0];
                            //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                            if (edi != null)
                            {
                                List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int, WorkTimeIntervalTO>();
                                TimeSpan ts = new TimeSpan();
                                for (int i = 0; i < intervals.Count; i++)
                                {
                                    WorkTimeIntervalTO interval = intervals[i];
                                    dayIntervals.Add(i, interval);
                                    shouldWork = true;
                                    ts += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                }
                                if (ts.TotalMinutes > 0)
                                {
                                    if (day.Date <= DateTime.Now.Date && !isHoliday(day))
                                        numOfDays++;
                                }
                                List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(pairs, day, is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                                bool containRegularWork = false;
                                TimeSpan tsRegularWork = new TimeSpan();
                                TimeSpan tsNightShift = new TimeSpan();
                                bool containWholeDayAbsence = false;
                                TimeSpan tsWholeDayAbsence = new TimeSpan();

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                    {
                                        containRegularWork = true;
                                        tsRegularWork += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                    }
                                }

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isWholeDayAbsence(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                    {
                                        containWholeDayAbsence = true;

                                        PassAbsence = interval.PassTypeID;
                                        tsWholeDayAbsence += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                    }
                                }

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isPassNight(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                    {
                                        TimeSpan EndHelp = new TimeSpan(07, 30, 0);
                                        TimeSpan startOnly = new TimeSpan(22, 0, 0);
                                        TimeSpan endOnly = new TimeSpan(23, 59, 59);
                                        TimeSpan startOnly1 = new TimeSpan(0, 0, 0);
                                        TimeSpan endOnly1 = new TimeSpan(6, 0, 0);

                                        if (interval.EndTime.TimeOfDay >= startOnly)
                                        {

                                            TimeSpan now = new TimeSpan(22, 0, 0);
                                            if (interval.StartTime.TimeOfDay >= now)
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                            }
                                            else
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - now);
                                            }

                                        }

                                        else if (interval.EndTime.TimeOfDay <= EndHelp)
                                        {

                                            tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - endOnly1));
                                        }

                                        else if (interval.StartTime.TimeOfDay <= endOnly1)
                                        {


                                            TimeSpan now = new TimeSpan(0, 0, 0);
                                            TimeSpan after = new TimeSpan(6, 0, 0);
                                            if (interval.EndTime.TimeOfDay <= after)
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                            }
                                            else
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - after));
                                            }
                                        }

                                        else if (interval.StartTime.TimeOfDay >= startOnly)
                                        {


                                            TimeSpan after = new TimeSpan(23, 59, 59);

                                            tsNightShift += (after - interval.StartTime.TimeOfDay);

                                        }



                                    }
                                }

                                if (containRegularWork)
                                {

                                    if (containWholeDayAbsence == false)
                                    {
                                        if (rbNumOfHours.Checked)
                                        {
                                            string hours = tsRegularWork.Hours.ToString();
                                            if (tsRegularWork.Hours < 10)
                                                hours = "0" + hours;
                                            string minutes = tsRegularWork.Minutes.ToString();
                                            if (tsRegularWork.Minutes < 10)
                                                minutes = "0" + minutes;
                                            row["day_" + date.ToString()] = hours + ":" + minutes;

                                        }
                                        else
                                        {
                                            row["day_" + date.ToString()] = "+";
                                            setFlag(Constants.regularWork);
                                        }
                                        totalMinutes += (int)tsRegularWork.TotalMinutes;
                                        totalNightShift += (int)tsNightShift.TotalMinutes;
                                    }
                                    else
                                    {
                                        if (!isHoliday(day))
                                        {
                                            row["day_" + date.ToString()] = pTypes[PassAbsence].ToString() + "*";

                                            totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                        }
                                        //setFlag(PassAbsence);
                                    }
                                }
                                else if (containWholeDayAbsence == true)
                                {
                                    if (!isHoliday(day))
                                    {
                                        row["day_" + date.ToString()] = pTypes[PassAbsence].ToString();
                                        totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                    }
                                }

                                else
                                {
                                    if (dayIOPairList.Count > 0 && ((TimeSpan)(dayIOPairList[0].EndTime.Subtract((dayIOPairList[0].StartTime)))).TotalSeconds > 0)
                                    {
                                        if (this.isPass(dayIOPairList[0].PassTypeID) && dayIOPairList[0].IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                            row["day_" + date.ToString()] = "00:00";
                                        else
                                            if (pTypes.ContainsKey(dayIOPairList[0].PassTypeID))
                                                row["day_" + date.ToString()] = pTypes[dayIOPairList[0].PassTypeID].ToString();
                                        //setFlag(((IOPair)pairDay[0]).PassTypeID);
                                    }
                                    else if (!shouldWork)
                                    {
                                        row["day_" + date.ToString()] = "00:00";
                                    }
                                }
                            }
                        }
                    }

                    row["imageID"] = 1;
                    row["total"] = totalMinutes.ToString();
                    row["numOfDays"] = numOfDays.ToString();
                    row["totalAbsence"] = totalMinutesAbsence.ToString();
                    //adding row for night shifts
                    row["totalNS"] = totalNightShift.ToString();
                    if (counter == 0)
                    {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

                    table.Rows.Add(row);
                    counter++;

                    emplPairs.Clear();
                }

                table.AcceptChanges();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.PrepareCRData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return dataSet;
        }

        private string makeLegend(string description)
        {
            try
            {
                string desc = "";
                string temp = description;

                int index = -1;
                while (!temp.Trim().Equals("") && desc.Length < Constants.maxLegendDesc)
                {
                    string letter = temp.Trim().Substring(0, 1).ToUpper();
                    if (isLetter(letter))
                    {
                        desc += letter;
                    }

                    index = temp.IndexOf(' ');

                    if (index > 0)
                    {
                        temp = temp.Substring(index).Trim();
                    }
                    else
                    {
                        temp = "";
                    }
                }

                if (!legendDescriptions.ContainsKey(desc))
                {
                    legendDescriptions.Add(desc, 0);
                }
                else
                {
                    legendDescriptions[desc]++;
                    desc += legendDescriptions[desc].ToString().Trim();
                    legendDescriptions.Add(desc, 0);
                }

                return desc;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.makeLegend(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isLetter(string letter)
        {
            try
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter.Trim().ToUpper()) > 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isLetter(): " + ex.Message + "\n");
                throw ex;
            }
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        private bool isPass(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }

        private bool isWholeDayAbsence(int currentPassTypeID)
        {
            bool isWholeDayAbsence = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.wholeDayAbsence;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isWholeDayAbsence = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isWholeDayAbsence(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isWholeDayAbsence;
        }

        private bool isPassNight(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }

        private string setFlag(int passTypeID)
        {
            string flag = "";

            try
            {
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    switch (passTypeID)
                    {
                        case -7:
                            flag = Constants.passTypeRO;
                            break;
                        case -4:
                            flag = Constants.passTypeRP;
                            break;
                        case -3:
                            flag = Constants.passTypeP;
                            break;
                        case 0:
                            flag = Constants.passTypeR;
                            break;
                        case 4:
                            flag = Constants.passTypeSD;
                            break;
                        case 5:
                            flag = Constants.passTypeO;
                            break;
                        case 6:
                            flag = Constants.passTypeB;
                            break;
                        case 7:
                            flag = Constants.passTypeSP;
                            break;
                        case 8:
                            flag = Constants.passTypeNO;
                            break;
                        case 11:
                            flag = Constants.passTypeS;
                            break;
                        case 12:
                            flag = Constants.passTypeB42;
                            break;
                        case 13:
                            flag = Constants.passTypeBP;
                            break;
                        case 14:
                            flag = Constants.passTypeI;
                            break;
                        case 15:
                            flag = Constants.passTypeC;
                            break;
                        case 9:
                            flag = Constants.passTypePO;
                            break;
                        default:
                            flag = Constants.passTypeCO;
                            break;
                    }
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    switch (passTypeID)
                    {
                        case -7:
                            flag = Constants.passTypeRO;
                            break;
                        case -4:
                            flag = Constants.passTypeRP;
                            break;
                        case -3:
                            flag = Constants.passTypeP;
                            break;
                        case 0:
                            flag = Constants.passTypeR;
                            break;
                        case 4:
                            flag = Constants.passTypeSD;
                            break;
                        case 5:
                            flag = Constants.passTypeO;
                            break;
                        case 6:
                            flag = Constants.passTypeB;
                            break;
                        case 7:
                            flag = Constants.passTypeSP;
                            break;
                        case 8:
                            flag = Constants.passTypeNO;
                            break;
                        case 11:
                            flag = Constants.passTypeS;
                            break;
                        case 12:
                            flag = Constants.passTypeB42;
                            break;
                        case 13:
                            flag = Constants.passTypeBP;
                            break;
                        case 14:
                            flag = Constants.passTypeI;
                            break;
                        case 15:
                            flag = Constants.passTypeC;
                            break;
                        case 9:
                            flag = Constants.passTypePO;
                            break;
                        default:
                            flag = Constants.passTypeCO;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.setFlag(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MagnaExcel me = new MagnaExcel();
            me.ShowDialog();
        }
    }
}
