using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Drawing.Printing;

using TransferObjects;
using Common;
using Reports;
using Util;

namespace UI
{
    public partial class StatisticGraphicReports : Form
    {
        protected DebugLog log;
        protected ResourceManager rm;
        protected CultureInfo culture;
        protected ApplUserTO logInUser;
        protected List<WorkTimeSchemaTO> timeSchemaList;

        // Key is Pass Type Id, Value is Pass Type
        protected Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();
        protected List<HolidayTO> holidays;
        protected List<WorkingUnitTO> wUnits;
        protected string wuString ;

        // Current Woking Unit
        protected WorkingUnit currentWorkingUnit;
        protected List<WorkingUnitTO> currentWorkingUnitsList;
        List<WorkingUnitTO> selectedWUList;

        Dictionary<int, List<EmployeeTO>> employeesList;
        List<WorkTimeIntervalTO> intervalList;
        //Working units List View indexes
        const int WUName = 0;
        const int ParentWUID = 1;

        protected ListViewItemComparer _comp;
        //properties for printing
        private PageSettings pgSettings = new PageSettings();
        PrintDocument printDocument1 = new PrintDocument();
        private Bitmap memoryImage;


        //for locations tree view
        List<LocationTO> locArray;

        public StatisticGraphicReports()
        {
            InitializeComponent();
            
            selectedWUList = new List<WorkingUnitTO>();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void populateParentWorkigUnitCombo()
        {
            try
            {
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                this.cbParentWorkingUnit.DataSource = wuArray;
                this.cbParentWorkingUnit.DisplayMember = "Name";
                this.cbParentWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void InitialiseListView()
        {
            lvWorkingUnits.BeginUpdate();
            lvWorkingUnits.Columns.Add(rm.GetString("lblWorkingUnit", culture), (lvWorkingUnits.Width - 4) / 2, HorizontalAlignment.Left);
            lvWorkingUnits.Columns.Add(rm.GetString("lblParentWUID", culture), (lvWorkingUnits.Width - 4) / 2 - 20, HorizontalAlignment.Left);
            lvWorkingUnits.EndUpdate();
        }

        protected void populateWorkingUnitsListView(List<WorkingUnitTO> WorkingUnitsList)
        {
            try
            {
                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Items.Clear();

                WorkingUnit tempWu = new WorkingUnit();
                if (WorkingUnitsList.Count > 0)
                {
                    foreach (WorkingUnitTO wunit in WorkingUnitsList)
                    {
                        ListViewItem item = new ListViewItem();

                        //if ((currentWorkingUnit.Find(Int32.Parse(item.Text.Trim()))) && (currentWorkingUnit.WorkingUnitID != 0))
                        //if ((tempWu.Find(wunit.WorkingUnitID)))// && (tempWu.WorkingUnitID != 0))
                        //{
                            item.Text = wunit.Name.Trim();
                            tempWu.WUTO.ParentWorkingUID = wunit.ParentWorkingUID;
                            item.SubItems.Add(tempWu.getParentWorkingUnit().Name.Trim());
                            item.Tag = wunit.WorkingUnitID;

                            lvWorkingUnits.Items.Add(item);

                        //}
                    }
                }

                lvWorkingUnits.EndUpdate();
                lvWorkingUnits.Invalidate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.populateWorkingUnitsListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //private string parse(string forParsing)
        //{
        //    string parsedString = forParsing.Trim();
        //    if (parsedString.StartsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(1);
        //        parsedString = "%" + parsedString;
        //    }

        //    if (parsedString.EndsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(0, parsedString.Length - 1);
        //        parsedString = parsedString + "%";
        //    }

        //    return parsedString;
        //}


        public void cbParentWorkingUnit_SelectedIndexChanged(object o, EventArgs e)
        {
            try
            {                
                // Value -1 is assigned to "ALL" in cbParentUnitID
                //if (cbParentWorkingUnit.SelectedIndex > 0 && Int32.Parse(cbParentWorkingUnit.SelectedValue.ToString().Trim()) != -1)
                //{
                //    partentWUID = cbParentWorkingUnit.SelectedValue.ToString().Trim();
                //}

                WorkingUnit wu = new WorkingUnit();
                if (cbParentWorkingUnit.SelectedIndex > 0)
                {
                    wu.WUTO.ParentWorkingUID = (int)cbParentWorkingUnit.SelectedValue;
                }

                currentWorkingUnitsList = wu.Search();
                               
                lvWorkingUnits.Items.Clear();
                if (currentWorkingUnitsList.Count > 0)
                {
                    populateWorkingUnitsListView(currentWorkingUnitsList);
                }
                _comp.SortColumn = 0;
                lvWorkingUnits.Sorting = SortOrder.Ascending;
                currentWorkingUnit.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.cbParentWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void StatisticGraphicReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);
                logInUser = NotificationController.GetLogInUser();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                InitialiseListView();

                //get all holidays
                holidays = new Holiday().Search(new DateTime(), new DateTime());

                currentWorkingUnitsList = new List<WorkingUnitTO>();
                currentWorkingUnit = new WorkingUnit();
                _comp = new ListViewItemComparer(lvWorkingUnits);
                populateParentWorkigUnitCombo();
                if (wUnits.Count > 0)
                {
                    populateWorkingUnitsListView(wUnits);
                }
                // get all Pass Types
                List<PassTypeTO> passTypesAll = new PassType().Search();
                foreach (PassTypeTO pt in passTypesAll)
                {
                    passTypes.Add(pt.PassTypeID, pt);
                }
                this.gbGraphic.Enabled = false;
                employeesList = new Dictionary<int, List<EmployeeTO>>();
                this.CenterToScreen();
                populateLocationCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.StatisticGraphicReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }
        /// <summary>
        /// Populate Location Combo Box
        /// </summary>
        private void populateLocationCombo()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                locArray = loc.Search();
                locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

                cbLocation.DataSource = locArray;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        protected void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("statisticRports", culture);
                
                // group box text
                gbWorkingUnits.Text = rm.GetString("gbWorkingUnits", culture);
                gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbStatisticReportType.Text = rm.GetString("gbStatisticReportType", culture);
                gbGraphic.Text = rm.GetString("gbGraphic", culture);
                gbStatisticReport.Text = rm.GetString("statisticRports", culture);
                gbLocation.Text = rm.GetString("gbLocation", culture);
                gbIsWrkHrs.Text = rm.GetString("gbIsWrkHrs", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnShow.Text = rm.GetString("btnShow", culture);
                btnPrint.Text = rm.GetString("btnPrint", culture);

                //radio button's text
                rbAll.Text = rm.GetString("rbAll", culture);
                rbYes.Text = rm.GetString("yes", culture);
                rbNo.Text = rm.GetString("no", culture);

                // label's text
                lblParentWUID.Text = rm.GetString("lblParentWUID", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                //CheckBox's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbAbsenceDuringWorkingTime.Text = rm.GetString("chbAbsenceDuringWorkingTime", culture);
                chbWholeDayAbsence.Text = rm.GetString("chbWholeDayAbsence", culture);
                chbPhysicalAttendance.Text = rm.GetString("chbPhysicalAttendance", culture);
                chbExtraHours.Text = rm.GetString("chbExtraHours", culture);

                //Radio button's
                rbMultiWorkingUnit.Text = rm.GetString("rbMultiWorkingUnit", culture);
                rbSingleWorkingUnit.Text = rm.GetString("rbSingleWorkingUnit", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                
                //if there is no selected items in list show message
                if (this.lvWorkingUnits.SelectedItems.Count == 0 )
                {
                    MessageBox.Show(rm.GetString("SelectWU", culture));
                }
                else
                {
                    int isWrkCounter = -1;
                    if (rbYes.Checked)
                    {
                        isWrkCounter = (int)Constants.IsWrkCount.IsCounter;
                    }
                    if (rbNo.Checked)
                    {
                        isWrkCounter = (int)Constants.IsWrkCount.IsNotCounter;
                    }
                    if (this.rbSingleWorkingUnit.Checked)// if statistic report type is Single working unit
                    {
                        if (this.lvWorkingUnits.SelectedItems.Count > 4)
                        {
                            MessageBox.Show(rm.GetString("MustSelectLessWU", culture));
                        }
                        else
                        {
                            this.gbStatisticReport.Controls.Clear();
                            //return hashtable for selected working units, key is WUID value is list of IOPairs
                            Dictionary<int, Dictionary<int, List<IOPairTO>>> ioPairsList = this.getIOPairsForSelectedWU((int)cbLocation.SelectedValue,isWrkCounter);
                            //geting intervals for selected working units
                            List<EmployeeTimeScheduleTO> employeesTimeScheduleList = new List<EmployeeTimeScheduleTO>();
                            intervalList = new List<WorkTimeIntervalTO>();
                            string employeesString = this.getEmployeesString();

                            employeesTimeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeesString, dtpFrom.Value.Date, dtpTo.Value.Date);
                            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScedules = this.getEmplTimeSchedules(employeesTimeScheduleList);

                            timeSchemaList = this.getTimeSchema(employeesTimeScheduleList);

                            switch (this.lvWorkingUnits.SelectedItems.Count)
                            {
                                case 1:
                                    StatisticReportViewSingleWUControl statisticGraphicReports = new StatisticReportViewSingleWUControl(ioPairsList[int.Parse(lvWorkingUnits.SelectedItems[0].Tag.ToString())], emplTimeScedules, timeSchemaList, employeesList[int.Parse(lvWorkingUnits.SelectedItems[0].Tag.ToString())], holidays, passTypes);
                                    this.setStatisticReportView(statisticGraphicReports, 0, 10, 15, this.gbStatisticReport.Width - 20, this.gbStatisticReport.Height - 30);
                                    break;
                                case 2:
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 0), 0, 10, 15, (this.gbStatisticReport.Width - 20) / 2, this.gbStatisticReport.Height - 30);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 1), 1, 10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, this.gbStatisticReport.Height - 30);
                                    break;
                                case 3:
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 0), 0, 10, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 30) / 2);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 1), 1, 10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 2), 2, 10, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    break;
                                case 4:
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 0), 0, 10, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 1), 1, 10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 2), 2, 10, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    this.setStatisticReportView(this.getStatisticReportView(ioPairsList, emplTimeScedules, 3), 3, 10 + (this.gbStatisticReport.Width - 20) / 2, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                                    break;
                            }
                        }
                    }
                    if (this.rbMultiWorkingUnit.Checked)
                    {
                        if (this.chbPhysicalAttendance.Checked || this.chbAbsenceDuringWorkingTime.Checked || this.chbWholeDayAbsence.Checked || this.chbExtraHours.Checked)
                        {
                            this.gbStatisticReport.Controls.Clear();
                            //return hashtable for selected working units, key is WUID value is list of IOPairs
                            Dictionary<int, Dictionary<int, List<IOPairTO>>> ioPairsList = this.getIOPairsForSelectedWU((int)cbLocation.SelectedValue,isWrkCounter);
                            wuString = "";
                            foreach (WorkingUnitTO wu in selectedWUList)
                            {
                                wuString += wu.WorkingUnitID + ", ";
                            }
                            if (wuString.Length > 0)
                            {
                                wuString = wuString.Substring(0, wuString.Length - 2);
                            }
                            employeesList.Add(-1, new Employee().SearchNotInWU(wuString));
                            int count = new IOPair().SearchNotInWUCount(-1, wuString, this.dtpFrom.Value.Date, this.dtpTo.Value.Date);
                            if (count > 0)
                            {
                                List<IOPairTO> IOPairsNotInWU = new IOPair().SearchNotInWUDate(-1, wuString, dtpFrom.Value.Date, dtpTo.Value.Date);
                                Dictionary<int, List<IOPairTO>> IOPairsEmpl = this.getIOPairsEmpl(IOPairsNotInWU);
                                ioPairsList.Add(-1, IOPairsEmpl);
                            }
                            //geting intervals for selected working units
                            List<EmployeeTimeScheduleTO> employeesTimeScheduleList = new List<EmployeeTimeScheduleTO>();
                            intervalList = new List<WorkTimeIntervalTO>();

                            employeesTimeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules("", dtpFrom.Value.Date, dtpTo.Value.Date);
                            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScedules = this.getEmplTimeSchedules(employeesTimeScheduleList);
                            timeSchemaList = this.getTimeSchema(employeesTimeScheduleList);

                            WorkingUnitTO workingUnit = new WorkingUnitTO();
                            workingUnit.Name = rm.GetString("theRestWU", culture);
                            selectedWUList.Add(workingUnit);
                            //Get color for each selected wu
                            Hashtable colors = this.getColorScheme();
                            ArrayList Graphics = new ArrayList();
                            bool countPlanedAndRealizedWorkingTime = true;
                            int planedWorkingTime = 0;
                            int realizedWorkingTime = 0;
                            if (this.chbPhysicalAttendance.Checked)
                            {
                                StatisticReportViewMultiWUControl statisticReportViewMultiWUControl = new StatisticReportViewMultiWUControl(Constants.physicalAttendanceGraphType, ioPairsList, emplTimeScedules, timeSchemaList, employeesList, selectedWUList, holidays, passTypes, countPlanedAndRealizedWorkingTime, colors, this.dtpFrom.Value.Date, this.dtpTo.Value.Date);
                                statisticReportViewMultiWUControl.GraphNameString = rm.GetString("chbPhysicalAttendance", culture);
                                Graphics.Add(statisticReportViewMultiWUControl);
                                if (countPlanedAndRealizedWorkingTime)
                                {
                                    planedWorkingTime = statisticReportViewMultiWUControl.PlannedWorkingTimeMin;
                                    realizedWorkingTime = statisticReportViewMultiWUControl.RealizeWorkingTimeMin;
                                }
                            }
                            if (this.chbWholeDayAbsence.Checked)
                            {
                                if (Graphics.Count > 0)
                                {
                                    countPlanedAndRealizedWorkingTime = false;
                                }
                                StatisticReportViewMultiWUControl statisticReportViewMultiWUControl = new StatisticReportViewMultiWUControl(Constants.wholeDayAbsenceGraphType, ioPairsList, emplTimeScedules, timeSchemaList, employeesList, selectedWUList, holidays, passTypes, countPlanedAndRealizedWorkingTime, colors, this.dtpFrom.Value.Date, this.dtpTo.Value.Date);
                                statisticReportViewMultiWUControl.DateFrom = this.dtpFrom.Value.Date;
                                statisticReportViewMultiWUControl.DateTo = this.dtpTo.Value.Date;
                                statisticReportViewMultiWUControl.GraphNameString = rm.GetString("chbWholeDayAbsence", culture);
                                if (countPlanedAndRealizedWorkingTime)
                                {
                                    planedWorkingTime = statisticReportViewMultiWUControl.PlannedWorkingTimeMin;
                                    realizedWorkingTime = statisticReportViewMultiWUControl.RealizeWorkingTimeMin;
                                }
                                else
                                {
                                    statisticReportViewMultiWUControl.PlannedWorkingTimeMin = planedWorkingTime;
                                    statisticReportViewMultiWUControl.RealizeWorkingTimeMin = realizedWorkingTime;
                                }
                                Graphics.Add(statisticReportViewMultiWUControl);
                            }
                            if (this.chbAbsenceDuringWorkingTime.Checked)
                            {
                                if (Graphics.Count > 0)
                                {
                                    countPlanedAndRealizedWorkingTime = false;
                                }
                                StatisticReportViewMultiWUControl statisticReportViewMultiWUControl = new StatisticReportViewMultiWUControl(Constants.absenceDuringWorkingTimeGraphType, ioPairsList, emplTimeScedules, timeSchemaList, employeesList, selectedWUList, holidays, passTypes, countPlanedAndRealizedWorkingTime, colors, this.dtpFrom.Value.Date, this.dtpTo.Value.Date);
                                statisticReportViewMultiWUControl.DateFrom = this.dtpFrom.Value.Date;
                                statisticReportViewMultiWUControl.DateTo = this.dtpTo.Value.Date;
                                statisticReportViewMultiWUControl.GraphNameString = rm.GetString("chbAbsenceDuringWorkingTime", culture);
                                if (countPlanedAndRealizedWorkingTime)
                                {
                                    planedWorkingTime = statisticReportViewMultiWUControl.PlannedWorkingTimeMin;
                                    realizedWorkingTime = statisticReportViewMultiWUControl.RealizeWorkingTimeMin;
                                }
                                else
                                {
                                    statisticReportViewMultiWUControl.PlannedWorkingTimeMin = planedWorkingTime;
                                    statisticReportViewMultiWUControl.RealizeWorkingTimeMin = realizedWorkingTime;
                                }
                                Graphics.Add(statisticReportViewMultiWUControl);
                            }
                            if (this.chbExtraHours.Checked)
                            {
                                if (Graphics.Count > 0)
                                {
                                    countPlanedAndRealizedWorkingTime = false;
                                }
                                StatisticReportViewMultiWUControl statisticReportViewMultiWUControl = new StatisticReportViewMultiWUControl(Constants.extraHoursGraphType, ioPairsList, emplTimeScedules, timeSchemaList, employeesList, selectedWUList, holidays, passTypes, countPlanedAndRealizedWorkingTime, colors, this.dtpFrom.Value.Date, this.dtpTo.Value.Date);
                                statisticReportViewMultiWUControl.DateFrom = this.dtpFrom.Value.Date;
                                statisticReportViewMultiWUControl.DateTo = this.dtpTo.Value.Date;
                                statisticReportViewMultiWUControl.GraphNameString = rm.GetString("chbExtraHours", culture);
                                if (countPlanedAndRealizedWorkingTime)
                                {
                                    planedWorkingTime = statisticReportViewMultiWUControl.PlannedWorkingTimeMin;
                                    realizedWorkingTime = statisticReportViewMultiWUControl.RealizeWorkingTimeMin;
                                }
                                else
                                {
                                    statisticReportViewMultiWUControl.PlannedWorkingTimeMin = planedWorkingTime;
                                    statisticReportViewMultiWUControl.RealizeWorkingTimeMin = realizedWorkingTime;
                                }
                                Graphics.Add(statisticReportViewMultiWUControl);
                            }
                            setStatisticReportViewMultiWUControlsBounds(Graphics);                            
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("selGraphType",culture));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
               
            }
        }

        private Hashtable getColorScheme()
        {
            Random rand = new Random();
            Hashtable colors = new Hashtable();
            try 
            {

                foreach (WorkingUnitTO wu in selectedWUList)
                {
                    Color color = Color.FromArgb(80, rand.Next(256),
                                               rand.Next(256),
                                               rand.Next(256));
                    colors.Add(wu.WorkingUnitID, color);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.getColorScheme(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return colors;
        }

        private void setStatisticReportViewMultiWUControlsBounds(ArrayList Graphics)
        {
            StatisticReportViewMultiWUControl statisticReportViewMultiWUControl = new StatisticReportViewMultiWUControl();
            switch (Graphics.Count)
            { 
                case 1:
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[0];
                    statisticReportViewMultiWUControl.SetBounds(10,15,this.gbStatisticReport.Width-20,this.gbStatisticReport.Height-30);
                    break;
                case 2:
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[0];
                    statisticReportViewMultiWUControl.SetBounds(10, 15, (this.gbStatisticReport.Width - 20) / 2, this.gbStatisticReport.Height - 30);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[1];
                    statisticReportViewMultiWUControl.SetBounds(10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, this.gbStatisticReport.Height - 30);
                    break;        
                case 3:
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[0];
                    statisticReportViewMultiWUControl.SetBounds(10, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 30) / 2);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[1];
                    statisticReportViewMultiWUControl.SetBounds(10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[2];
                    statisticReportViewMultiWUControl.SetBounds(10, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                    break; 
                case 4:
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[0];
                    statisticReportViewMultiWUControl.SetBounds(10, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 30) / 2);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[1];
                    statisticReportViewMultiWUControl.SetBounds(10 + (this.gbStatisticReport.Width - 20) / 2, 15, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[2];
                    statisticReportViewMultiWUControl.SetBounds(10, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                    statisticReportViewMultiWUControl = (StatisticReportViewMultiWUControl)Graphics[3];
                    statisticReportViewMultiWUControl.SetBounds(10 + (this.gbStatisticReport.Width - 20) / 2, 20 + (this.gbStatisticReport.Height - 35) / 2, (this.gbStatisticReport.Width - 20) / 2, (this.gbStatisticReport.Height - 35) / 2);
                    break; 
            }
            foreach (StatisticReportViewMultiWUControl statReport in Graphics)
            {
                this.gbStatisticReport.Controls.Add(statReport);
            }
        }
        
        public void setStatisticReportView(StatisticReportViewSingleWUControl statisticGraphicReports, int itemNum, int x, int y, int width, int height)
        {
            statisticGraphicReports.SetBounds(x, y, width, height);
            statisticGraphicReports.DateFrom = dtpFrom.Value.Date;
            statisticGraphicReports.DateTo = dtpTo.Value.Date;
            statisticGraphicReports.GraphNameString = this.lvWorkingUnits.SelectedItems[itemNum].SubItems[0].Text.ToString();
            this.gbStatisticReport.Controls.Add(statisticGraphicReports);
        }

        public StatisticReportViewSingleWUControl getStatisticReportView(Dictionary<int, Dictionary<int, List<IOPairTO>>> ioPairsList, Dictionary<int, List<EmployeeTimeScheduleTO>> employeesTimeScheduleList, int itemNum)
        {
            StatisticReportViewSingleWUControl statisticReportViewSingleWUControl = new StatisticReportViewSingleWUControl();
            try
            {
                int wuID = int.Parse(this.lvWorkingUnits.SelectedItems[itemNum].Tag.ToString());
                Dictionary<int, List<IOPairTO>> ioPairsForSelectedWU = ioPairsList[wuID];
                List<EmployeeTO> employeesForSelectedWU = employeesList[wuID];

                statisticReportViewSingleWUControl = new StatisticReportViewSingleWUControl(ioPairsForSelectedWU, employeesTimeScheduleList, timeSchemaList, employeesForSelectedWU, holidays, passTypes);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return statisticReportViewSingleWUControl;
        }

        public List<WorkTimeSchemaTO> getTimeSchema(List<EmployeeTimeScheduleTO> employeesTimeScheduleList)
        {
            List<WorkTimeSchemaTO> timeSchemaList = new List<WorkTimeSchemaTO>();
            try
            {
                string schemaID = "";
                
                foreach (EmployeeTimeScheduleTO employeeTimeSchedule in employeesTimeScheduleList)
                {
                    schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                }
                if (!schemaID.Equals(""))
                {
                    schemaID = schemaID.Substring(0, schemaID.Length - 2);
                }

                timeSchemaList = new TimeSchema().Search(schemaID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.getTimeSchema(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return timeSchemaList;
        }

        public Dictionary<int, List<EmployeeTimeScheduleTO>> getEmplTimeSchedules(List<EmployeeTimeScheduleTO> employeesTimeScheduleList)
        {
            Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeScedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();
            try
            {
                foreach (EmployeeTimeScheduleTO employeesTimeSchedule in employeesTimeScheduleList)
                {
                    if (emplTimeScedules.ContainsKey(employeesTimeSchedule.EmployeeID))
                    {
                        emplTimeScedules[employeesTimeSchedule.EmployeeID].Add(employeesTimeSchedule);
                    }
                    else
                    {
                        List<EmployeeTimeScheduleTO> list = new List<EmployeeTimeScheduleTO>();
                        list.Add(employeesTimeSchedule);
                        emplTimeScedules.Add(employeesTimeSchedule.EmployeeID, list);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.getEmplTimeSchedules(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return emplTimeScedules;
        }

        public Dictionary<int, Dictionary<int, List<IOPairTO>>> getIOPairsForSelectedWU(int locID, int isWrkHrs)
        {
            Dictionary<int, Dictionary<int, List<IOPairTO>>> iopairsWU = new Dictionary<int, Dictionary<int, List<IOPairTO>>>(); //key is WUID, member is hashtable with IOPirs
            this.employeesList = new Dictionary<int, List<EmployeeTO>>();//key is WUID, member is list of employees

            try
            {
                selectedWUList = new List<WorkingUnitTO>();
                //geting IOPairs for selected working units
                foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                {
                    wuString = "";
                    List<WorkingUnitTO> selectedWU = new List<WorkingUnitTO>();

                    Dictionary<int, List<IOPairTO>> IOPairsEmpl = new Dictionary<int, List<IOPairTO>>();
                    foreach (WorkingUnitTO wu in currentWorkingUnitsList)
                    {
                        if (wu.WorkingUnitID == int.Parse(item.Tag.ToString()))
                        {
                            selectedWU.Add(wu);
                            selectedWUList.Add(wu);
                        }
                    }

                    if (chbHierarhicly.Checked)
                    {
                        selectedWU = new WorkingUnit().FindAllChildren(selectedWU);
                    }
                    foreach (WorkingUnitTO wu in selectedWU)
                    {
                        wuString += wu.WorkingUnitID + ", ";
                    }

                    if (wuString.Length > 0)
                    {
                        wuString = wuString.Substring(0, wuString.Length - 2);
                    }
                    if (!employeesList.ContainsKey(int.Parse(item.Tag.ToString())))
                    {
                        employeesList.Add(int.Parse(item.Tag.ToString()), new Employee().SearchByWU(wuString));
                    }
                    IOPair ioPair = new IOPair();
                    List<IOPairTO> ioPairsList = new List<IOPairTO>();
                    int count = ioPair.SearchForWUCount(-1, wuString, dtpFrom.Value, dtpTo.Value);
                    if (count != 0)
                    {
                        ioPairsList = ioPair.SearchForWUDate(-1, wuString, dtpFrom.Value, dtpTo.Value, locID,isWrkHrs);
                    }
                    IOPairsEmpl = this.getIOPairsEmpl(ioPairsList);
                    if (!iopairsWU.ContainsKey(int.Parse(item.Tag.ToString())))
                    {
                        iopairsWU.Add(int.Parse(item.Tag.ToString()), IOPairsEmpl);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return iopairsWU;
        }

        private Dictionary<int, List<IOPairTO>> getIOPairsEmpl(List<IOPairTO> ioPairsList)
        {
            Dictionary<int, List<IOPairTO>> IOPairsEmpl = new Dictionary<int,List<IOPairTO>>();
            try
            {
                foreach (IOPairTO iopairTO in ioPairsList)
                {
                    if (IOPairsEmpl.ContainsKey(iopairTO.EmployeeID))
                    {
                        IOPairsEmpl[iopairTO.EmployeeID].Add(iopairTO);
                    }
                    else
                    {
                        List<IOPairTO> iopairs = new List<IOPairTO>();
                        iopairs.Add(iopairTO);
                        IOPairsEmpl.Add(iopairTO.EmployeeID, iopairs);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.getIOPairsEmpl(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return IOPairsEmpl;
        }

        protected void lvWorkingUnits_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvWorkingUnits.Sorting;
                lvWorkingUnits.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvWorkingUnits.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvWorkingUnits.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvWorkingUnits.Sorting = SortOrder.Ascending;
                }
                lvWorkingUnits.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.lvWorkingUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

   
        protected string getEmployeesString()
        {
            
            string emplString = "";
            try
            {
                List<EmployeeTO> listOfAllEmpl = new List<EmployeeTO>();
                foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                {
                    int wuID = int.Parse(item.Tag.ToString());
                    List<EmployeeTO> emplList = employeesList[wuID];
                    foreach (EmployeeTO emp in emplList)
                    {
                        emplString += emp.EmployeeID + ", ";
                        if (!listOfAllEmpl.Contains(emp))
                        {                            
                            listOfAllEmpl.Add(emp);
                        }
                    }
                }
                if (emplString.Length > 0)
                {
                    emplString = emplString.Substring(0, emplString.Length - 2);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.getEmployeesString(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return emplString;
        }
        protected class ListViewItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }

            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    ListViewItem temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }
            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
                // Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

                // Return value based on sort column	
                switch (SortColumn)
                {
                    case  StatisticGraphicReports.ParentWUID:
                    case StatisticGraphicReports.WUName:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        private void rbMultiWorkingUnit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.chbHierarhicly.Checked = false;
                this.chbHierarhicly.Enabled = false;
                this.gbGraphic.Enabled = true;
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.rbMultiWorkingUnit_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbSingleWorkingUnit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.chbHierarhicly.Checked = true;
                this.chbHierarhicly.Enabled = true;
                this.gbGraphic.Enabled = false;
                this.chbPhysicalAttendance.Checked = true;               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.rbSingleWorkingUnit_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
       
        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }
        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
       
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            CaptureScreen();
           
            printDocument1.DefaultPageSettings = pgSettings;
            printDocument1.DefaultPageSettings.Landscape = true;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDocument1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
               printDocument1.Print();
            }
          
        }

        private void StatisticGraphicReports_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " StatisticGraphicReports.StatisticGraphicReports_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string wuStr = "";
                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuStr += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuStr= wuString.Substring(0, wuString.Length - 1);
                }

                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuStr);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbParentWorkingUnit.SelectedIndex = cbParentWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " StatisticGraphicReports.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(locArray);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}