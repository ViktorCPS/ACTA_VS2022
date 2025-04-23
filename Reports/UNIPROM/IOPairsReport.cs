using System;
using System.Collections;
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

namespace Reports.UNIPROM
{
    public partial class IOPairsReport : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog debug;
        string wuString = "";
        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;

        //key is employeeID vaule is employee object
        Dictionary<int, EmployeeTO> employeesTracks = new Dictionary<int,EmployeeTO>();
        Dictionary<int, EmployeeTO> employeesDrivers = new Dictionary<int,EmployeeTO>();

        //key is tagID value is employeeID
        Dictionary<ulong, int> tagEmpl = new Dictionary<ulong,int>();

        //key is employeeID value is tagID
        Dictionary<int, uint> emplTag = new Dictionary<int,uint>();
        ArrayList wuIDs = new ArrayList();
        ArrayList trackWUIDs = new ArrayList();

        List<PassTO> currentTrackPassesList = new List<PassTO>();

        // List View indexes
        const int VechicleIndex = 0;
        const int PersoneIndex = 1;
        const int DirectionIndex =3;
        const int EventTimeIndex = 2;

        const int StartTimeIndex = 0;
        const int EndTimeIndex = 1;

        private int sortOrderWithout;
        private int sortFieldWithout;

        public IOPairsReport()
        {
            InitializeComponent();

            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePresenceType).Assembly);
            setLanguage();
        }

        private void setLanguage()
        {
            try
            {
                //form text
                this.Text = rm.GetString("UNIPROMIOPairsReport", culture);

                //radio button's text
                rbNo.Text = rm.GetString("No", culture);
                rbYes.Text = rm.GetString("Yes", culture);

                //label's text
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblStart.Text = rm.GetString("lblStart", culture);
                lblEnd.Text = rm.GetString("lblEnd", culture);
                lblDirection.Text = rm.GetString("lblDirection", culture);
                lblVechicle.Text = rm.GetString("lblVechicle", culture);
                lblPersone.Text = rm.GetString("lblPersone", culture);

                //group box's text
                gbSearch.Text = rm.GetString("timeInterval", culture);
                gbTime.Text = rm.GetString("gbTime", culture);
                gbDate.Text = rm.GetString("gbDate", culture);
                gbGroupByVechicle.Text = rm.GetString("gbGroupByVechicle", culture);
                
                //button's text
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);

                //table's text
                lvPasses.BeginUpdate();
                lvPasses.Columns.Add(rm.GetString("hdrVechicle", culture),lvPasses.Width/4-7);
                lvPasses.Columns.Add(rm.GetString("hdrPersone", culture),lvPasses.Width/4-7);
                lvPasses.Columns.Add(rm.GetString("hdrDateTime", culture),lvPasses.Width/4-7);
                lvPasses.Columns.Add(rm.GetString("hdrDirection", culture),lvPasses.Width/4-7);
                lvPasses.EndUpdate();
                
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {

                if (currentTrackPassesList.Count <= 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
                dtFrom.Value = new DateTime(dtFrom.Value.Year, dtFrom.Value.Month, dtFrom.Value.Day, dtFrom.Value.Hour, dtFrom.Value.Minute, 0);
                dtTo.Value = new DateTime(dtTo.Value.Year, dtTo.Value.Month, dtTo.Value.Day, dtTo.Value.Hour, dtTo.Value.Minute, 59);

                // Table Definition for Crystal Reports
                DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("iopairs");
                DataTable tableCRTotals = new DataTable("totals");
                DataTable tableI = new DataTable("images");

                tableCR.Columns.Add("direction", typeof(System.String));
                tableCR.Columns.Add("event_time", typeof(System.DateTime));
                tableCR.Columns.Add("employee", typeof(System.String));
                tableCR.Columns.Add("licence_num", typeof(System.String));
                tableCR.Columns.Add("imageID", typeof(byte));


                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                dataSetCR.Tables.Add(tableCR);
                dataSetCR.Tables.Add(tableI);

                List<int> emplIDs = new List<int>();

                //table of vechicle passes
                foreach (PassTO p in currentTrackPassesList)
                {
                    DataRow row = tableCR.NewRow();

                    row["direction"] = p.Direction;
                    //string eventTime = "";
                    //if (p.EventTime != new DateTime())
                    //    eventTime = p.EventTime;
                    row["event_time"] = p.EventTime;
                    row["licence_num"] = p.EmployeeName;
                    row["employee"] = p.GateName;

                    row["imageID"] = 1;

                    tableCR.Rows.Add(row);
                    tableCR.AcceptChanges();

                    if (!emplIDs.Contains(p.EmployeeID))
                    {
                        emplIDs.Add(p.EmployeeID);
                    }
                }
                if (tableCR.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                //selected values
                string selFromDate = dtpFromDate.Value.ToString("dd.MM.yyyy");
                string selFromTime = dtFrom.Value.ToString("HH:mm");
                string selToDate = dtpToDate.Value.ToString("dd.MM.yyyy");
                string selToTime = dtTo.Value.ToString("HH:mm");
                string selVechicle = "*";
                string selDriver = "*";
                string selDirection = "*";
                if (cbVechicle.SelectedIndex > 0)
                    selVechicle = cbVechicle.Text.ToString();
                if (cbPersone.SelectedIndex > 0)
                    selDriver = cbPersone.Text.ToString();
                if (cbDirection.SelectedIndex > 0)
                    selDirection = cbDirection.SelectedItem.ToString();
                if (rbNo.Checked)//if we do not need to group by vechicles
                {
                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.UNIPROM.UNIPROM_sr.UNIPROMIOPairsCRView view = new Reports.UNIPROM.UNIPROM_sr.UNIPROMIOPairsCRView(dataSetCR, selFromDate, selFromTime, selToDate, selToTime, selDriver, selVechicle, selDirection);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.UNIPROM.UNIPROM_en.UNIPROMIOPairsCRView_en view = new Reports.UNIPROM.UNIPROM_en.UNIPROMIOPairsCRView_en(dataSetCR, selFromDate, selFromTime, selToDate, selToTime, selDriver, selVechicle, selDirection);
                        view.ShowDialog(this);
                    }
                }
                else// if (rbYes.Checked)
                {
                    //table of time spend inside and outside of company field
                    tableCRTotals.Columns.Add("inside", typeof(System.String));
                    tableCRTotals.Columns.Add("outside", typeof(System.String));
                    tableCRTotals.Columns.Add("without_driver", typeof(System.String));
                    tableCRTotals.Columns.Add("licence_num", typeof(System.String));
                    tableCRTotals.Columns.Add("open_pairs", typeof(System.String));
                    tableCRTotals.Columns.Add("imageID", typeof(byte));

                    dataSetCR.Tables.Add(tableCRTotals);

                    IOPair pair = new IOPair();
                    int empID = -1;
                    //if vechicle is selected take ioPairs just for one vechicle
                    if (cbVechicle.SelectedIndex > 0)
                        empID = (int)cbVechicle.SelectedValue;

                    //or take ioPairs for vechicles with passes
                    if (empID != -1)
                    {
                        emplIDs = new List<int>();
                        emplIDs.Add(empID);
                    }

                    //total for all vechicles
                    TimeSpan totalIN = new TimeSpan();
                    TimeSpan totalOUT = new TimeSpan();

                    foreach (int id in emplIDs)
                    {
                        TimeSpan insideTime = new TimeSpan();
                        TimeSpan outsideTime = new TimeSpan();
                        TimeSpan withoutDriver = new TimeSpan();
                        string licenceNumber = employeesTracks[id].LastName;
                        string openPairs = "";

                        IOPair iop = new IOPair();
                        iop.PairTO.EmployeeID = id;
                        List<IOPairTO> trackPairsList = iop.Search(dtpFromDate.Value.Date, dtpToDate.Value.Date, wuString, -1);
                        
                        List<IOPairTO> newPairsList = new List<IOPairTO>();

                        Dictionary<DateTime, IOPairTO> firstOpenPair = new Dictionary<DateTime, IOPairTO>();
                        Dictionary<DateTime, IOPairTO> lastOpenPair = new Dictionary<DateTime, IOPairTO>();
                        Dictionary<DateTime, List<IOPairTO>> datePairs = new Dictionary<DateTime, List<IOPairTO>>();

                        //create hashtable key is date value is list of io pairs
                        foreach (IOPairTO trackPair in trackPairsList)
                        {
                            //count just iopairs in selected period
                            if (cbPersone.SelectedIndex == 0 && (trackPair.StartTime.TimeOfDay <= dtTo.Value.TimeOfDay || trackPair.StartTime == new DateTime())
                                && (trackPair.EndTime.TimeOfDay >= dtFrom.Value.TimeOfDay || trackPair.EndTime == new DateTime()))
                            {
                                if (!datePairs.ContainsKey(trackPair.IOPairDate.Date))
                                {
                                    List<IOPairTO> iopairList = new List<IOPairTO>();
                                    iopairList.Add(trackPair);
                                    datePairs.Add(trackPair.IOPairDate.Date, iopairList);
                                    if (trackPair.StartTime == new DateTime())
                                    {
                                        firstOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                    }
                                    if (trackPair.EndTime == new DateTime())
                                    {
                                        lastOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                    }
                                }
                                else
                                {
                                    datePairs[trackPair.IOPairDate.Date].Add(trackPair);
                                    if (trackPair.StartTime == new DateTime())
                                    {
                                        if (!firstOpenPair.ContainsKey(trackPair.IOPairDate.Date))
                                        {
                                            firstOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                        }
                                        else
                                        {
                                            if (firstOpenPair[trackPair.IOPairDate.Date].EndTime > trackPair.EndTime)
                                            {
                                                firstOpenPair[trackPair.IOPairDate.Date] = trackPair;
                                            }
                                        }
                                    }//if (trackPair.StartTime == new DateTime())
                                    if (trackPair.EndTime == new DateTime())
                                    {
                                        if (!lastOpenPair.ContainsKey(trackPair.IOPairDate.Date))
                                        {
                                            lastOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                        }
                                        else
                                        {
                                            if (lastOpenPair[trackPair.IOPairDate.Date].StartTime < trackPair.StartTime)
                                            {
                                                lastOpenPair[trackPair.IOPairDate.Date] = trackPair;
                                            }
                                        }
                                    }//if (trackPair.EndTime == new DateTime())
                                }//if (datePairs.ContainsKey(trackPair.IOPairDate.Date))
                            }//if ((trackPair.StartTime.TimeOfDay <= dtTo.Value.TimeOfDay || trackPair.StartTime == new DateTime())
                            //&& (trackPair.EndTime.TimeOfDay >= dtFrom.Value.TimeOfDay || trackPair.EndTime == new DateTime()))
                            else // if we count for each employee we need all pairs
                            {
                                if (!datePairs.ContainsKey(trackPair.IOPairDate.Date))
                                {
                                    List<IOPairTO> iopairList = new List<IOPairTO>();
                                    iopairList.Add(trackPair);
                                    datePairs.Add(trackPair.IOPairDate.Date, iopairList);
                                }
                                else
                                    datePairs[trackPair.IOPairDate.Date].Add(trackPair);
                                if (trackPair.StartTime == new DateTime())
                                {
                                    if (!firstOpenPair.ContainsKey(trackPair.IOPairDate.Date))
                                    {
                                        firstOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                    }
                                    else
                                    {
                                        if (firstOpenPair[trackPair.IOPairDate.Date].EndTime > trackPair.EndTime)
                                        {
                                            firstOpenPair[trackPair.IOPairDate.Date] = trackPair;
                                        }
                                    }
                                }//if (trackPair.StartTime == new DateTime())
                                if (trackPair.EndTime == new DateTime())
                                {
                                    if (!lastOpenPair.ContainsKey(trackPair.IOPairDate.Date))
                                    {
                                        lastOpenPair.Add(trackPair.IOPairDate.Date, trackPair);
                                    }
                                    else
                                    {
                                        if (lastOpenPair[trackPair.IOPairDate.Date].StartTime < trackPair.StartTime)
                                        {
                                            lastOpenPair[trackPair.IOPairDate.Date] = trackPair;
                                        }
                                    }
                                }//if (trackPair.EndTime == new DateTime())
                            }
                        }//foreach (IOPair trackPair in trackPairsList)

                        if (cbPersone.SelectedIndex == 0)
                        {
                            DateTime date = dtpFromDate.Value.Date;

                            while (date <= dtpToDate.Value.Date)
                            {
                                if (datePairs.ContainsKey(date))
                                {
                                    outsideTime += dtTo.Value.TimeOfDay - dtFrom.Value.TimeOfDay;
                                    foreach (IOPairTO p1 in datePairs[date])
                                    {
                                        //if closed pair count
                                        if (p1.StartTime != new DateTime() && p1.EndTime != new DateTime())
                                        {
                                            if (p1.StartTime.TimeOfDay < dtFrom.Value.TimeOfDay)
                                                p1.StartTime = dtFrom.Value;
                                            if (p1.EndTime.TimeOfDay > dtTo.Value.TimeOfDay)
                                                p1.EndTime = dtTo.Value;
                                            insideTime += p1.EndTime.TimeOfDay - p1.StartTime.TimeOfDay;

                                        }
                                        //if open pair count first and last in day
                                        else
                                        {
                                            if (firstOpenPair.ContainsKey(date) && firstOpenPair[date].IOPairID == p1.IOPairID)
                                            {
                                                //count first open pair if time is in selected interval
                                                if (firstOpenPair[date].EndTime.TimeOfDay > dtFrom.Value.TimeOfDay || firstOpenPair[date].EndTime.TimeOfDay <= dtTo.Value.TimeOfDay)
                                                {
                                                    insideTime += firstOpenPair[date].EndTime.TimeOfDay - dtFrom.Value.TimeOfDay;
                                                }
                                            }
                                            else if (lastOpenPair.ContainsKey(date) && lastOpenPair[date].IOPairID == p1.IOPairID)
                                            {
                                                //count first open pair if time is in selected interval
                                                if (lastOpenPair[date].StartTime.TimeOfDay >= dtFrom.Value.TimeOfDay || lastOpenPair[date].EndTime.TimeOfDay < dtTo.Value.TimeOfDay)
                                                {
                                                    insideTime += dtTo.Value.TimeOfDay - lastOpenPair[date].StartTime.TimeOfDay;
                                                }
                                            }
                                            else
                                            {
                                                openPairs = "+";
                                            }
                                        }
                                    }//foreach (IOPair p1 in datePairs[date])
                                }//if(datePairs.ContainsKey(date))                                   
                                date = date.AddDays(1);
                            }//while (date <= dtpToDate.Value.Date)
                            outsideTime -= insideTime;
                        }// if (cbPersone.SelectedIndex == 0)
                        else
                        {
                            //if we count time for one driver we need every ioPair for each day
                            DateTime date = dtpFromDate.Value.Date;
                            while (date <= dtpToDate.Value.Date)
                            {
                                //time vechicle was inside or outside without selected driver
                                withoutDriver += dtTo.Value.TimeOfDay - dtFrom.Value.TimeOfDay;

                                int selEmployee = (int)cbPersone.SelectedValue;
                                if (datePairs.ContainsKey(date))
                                {
                                    List<IOPairTO> dayList = new List<IOPairTO>();
                                    IOPairTO firstPair = new IOPairTO();
                                    IOPairTO lastPair = new IOPairTO();
                                    if (firstOpenPair.ContainsKey(date))
                                    {
                                        firstPair = firstOpenPair[date];
                                    }
                                    if (lastOpenPair.ContainsKey(date))
                                    {
                                        lastPair = lastOpenPair[date];
                                    }

                                    foreach (IOPairTO iopair in datePairs[date])
                                    {
                                        if ((iopair.StartTime != new DateTime() && iopair.EndTime != new DateTime())
                                            || firstPair.IOPairID == iopair.IOPairID || lastPair.IOPairID == iopair.IOPairID)
                                        {
                                            dayList.Add(iopair);
                                        }

                                    }

                                    dayList.Sort(new ArrayListSort1(Constants.sortAsc, StartTimeIndex));
                                    for (int i = 0; i < dayList.Count; i++)
                                    {
                                        IOPairTO currentPair = dayList[i];
                                        IOPairTO nextPair = new IOPairTO();
                                        if (i + 1 < dayList.Count)
                                            nextPair = dayList[i + 1];

                                        //if pair is closed and inside interval
                                        if ((currentPair.EndTime != new DateTime() && currentPair.EndTime.TimeOfDay > dtFrom.Value.TimeOfDay) &&
                                            (currentPair.StartTime != new DateTime() && currentPair.StartTime.TimeOfDay < dtTo.Value.TimeOfDay))
                                        {
                                            EmployeeTO driver = new EmployeeTO();
                                            LogTO l = getLogForTime(currentPair.StartTime, currentPair.EmployeeID, out driver);
                                            EmployeeTO driver1 = new EmployeeTO();
                                            LogTO l1 = getLogForTime(currentPair.EndTime, currentPair.EmployeeID, out driver1);
                                            if (selEmployee == driver.EmployeeID || selEmployee == driver1.EmployeeID)
                                            {
                                                if (currentPair.StartTime.TimeOfDay < dtFrom.Value.TimeOfDay)
                                                    currentPair.StartTime = dtFrom.Value;
                                                if (currentPair.EndTime.TimeOfDay > dtTo.Value.TimeOfDay)
                                                    currentPair.EndTime = dtTo.Value;
                                                if (nextPair.StartTime.TimeOfDay > currentPair.EndTime.TimeOfDay && nextPair.StartTime.TimeOfDay > dtTo.Value.TimeOfDay)
                                                    nextPair.StartTime = dtTo.Value;

                                                if (selEmployee == driver.EmployeeID)
                                                    insideTime += currentPair.EndTime.TimeOfDay - currentPair.StartTime.TimeOfDay;
                                                if (selEmployee == driver1.EmployeeID)
                                                {
                                                    if (nextPair.StartTime == new DateTime())
                                                    {
                                                        foreach (IOPairTO p in dayList)
                                                        {
                                                            if (nextPair.StartTime == new DateTime() || (nextPair.StartTime.TimeOfDay > p.StartTime.TimeOfDay && p.StartTime.TimeOfDay > currentPair.EndTime.TimeOfDay))
                                                            {
                                                                nextPair = p;
                                                            }
                                                        }
                                                    }
                                                    if (nextPair.StartTime.TimeOfDay > currentPair.EndTime.TimeOfDay && nextPair.StartTime.TimeOfDay <= dtTo.Value.TimeOfDay)
                                                    {
                                                        outsideTime += nextPair.StartTime.TimeOfDay - currentPair.EndTime.TimeOfDay;
                                                    }
                                                    if (nextPair.StartTime == new DateTime())
                                                    {
                                                        outsideTime += dtTo.Value.TimeOfDay - currentPair.EndTime.TimeOfDay;
                                                    }
                                                }
                                            }
                                        }
                                        else if (currentPair.StartTime == new DateTime() || currentPair.EndTime == new DateTime())
                                        {
                                            if (firstOpenPair.ContainsKey(date) && firstOpenPair[date].IOPairID == currentPair.IOPairID)
                                            {
                                                //count first open pair if time is in selected interval
                                                if (firstOpenPair[date].EndTime.TimeOfDay > dtFrom.Value.TimeOfDay)
                                                {
                                                    EmployeeTO driver = new EmployeeTO();
                                                    LogTO l = getLogForTime(currentPair.EndTime, currentPair.EmployeeID, out driver);
                                                    if (driver.EmployeeID == selEmployee)
                                                    {
                                                        nextPair = new IOPairTO();
                                                        foreach (IOPairTO p in dayList)
                                                        {
                                                            if ((nextPair.StartTime == new DateTime() || nextPair.StartTime.TimeOfDay > p.StartTime.TimeOfDay) && p.StartTime.TimeOfDay > currentPair.EndTime.TimeOfDay)
                                                            {
                                                                nextPair = p;
                                                            }
                                                        }
                                                        if (currentPair.EndTime.TimeOfDay < dtFrom.Value.TimeOfDay)
                                                            currentPair.EndTime = dtFrom.Value;
                                                        //no closed pairs after first open pair
                                                        if (nextPair.StartTime == new DateTime())
                                                        {
                                                            outsideTime += dtTo.Value.TimeOfDay - currentPair.EndTime.TimeOfDay;
                                                        }
                                                        else
                                                        {
                                                            outsideTime += nextPair.StartTime.TimeOfDay - currentPair.EndTime.TimeOfDay;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (lastOpenPair.ContainsKey(date) && lastOpenPair[date].IOPairID == currentPair.IOPairID)
                                            {
                                                //count first open pair if time is in selected interval
                                                if (lastOpenPair[date].StartTime.TimeOfDay >= dtFrom.Value.TimeOfDay && lastOpenPair[date].StartTime.TimeOfDay < dtTo.Value.TimeOfDay)
                                                {
                                                    EmployeeTO driver = new EmployeeTO();
                                                    LogTO l = getLogForTime(currentPair.StartTime, currentPair.EmployeeID, out driver);
                                                    if (driver.EmployeeID == selEmployee)
                                                    {
                                                        insideTime += dtTo.Value.TimeOfDay - lastOpenPair[date].StartTime.TimeOfDay;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                openPairs = "+";
                                            }
                                        }
                                    }
                                }

                                date = date.AddDays(1);
                            }
                            withoutDriver -= insideTime;
                            withoutDriver -= outsideTime;
                        }

                        DataRow rowTotal = tableCRTotals.NewRow();

                        rowTotal["inside"] = insideTime.Days + "d " + insideTime.Hours + "h " + insideTime.Minutes + "min";
                        rowTotal["outside"] = outsideTime.Days + "d " + outsideTime.Hours + "h " + outsideTime.Minutes + "min";
                        rowTotal["open_pairs"] = openPairs;
                        rowTotal["licence_num"] = licenceNumber;
                        if (withoutDriver.Minutes == 0)
                            rowTotal["without_driver"] = "";
                        else
                            rowTotal["without_driver"] = withoutDriver.Days + "d " + withoutDriver.Hours + "h " + withoutDriver.Minutes + "min";

                        rowTotal["imageID"] = 1;

                        tableCRTotals.Rows.Add(rowTotal);
                        tableCRTotals.AcceptChanges();

                        totalIN += insideTime;
                        totalOUT += outsideTime;

                    }//foreach (int id in emplIDs)

                    string TotalIN = totalIN.Days + "d " + totalIN.Hours + "h " + totalIN.Minutes + "min";
                    string TotalOUT = totalOUT.Days + "d " + totalOUT.Hours + "h " + totalOUT.Minutes + "min";

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.UNIPROM.UNIPROM_sr.UNIPROMIOPairsTotalsCRView view = new Reports.UNIPROM.UNIPROM_sr.UNIPROMIOPairsTotalsCRView(dataSetCR, selFromDate, selFromTime, selToDate, selToTime, selDriver, selVechicle, selDirection, TotalIN, TotalOUT);
                        view.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }



        private LogTO getLogForTime(DateTime dateTime, int EmployeeID, out EmployeeTO driver)
        {
            driver = new EmployeeTO();
            try
            {
                LogTO logFound = new LogTO();

                if (dateTime != new DateTime())
                {
                    uint tag = 0;
                    if (emplTag.ContainsKey(EmployeeID))
                        tag = (uint)emplTag[EmployeeID];
                    LogTO lto = new LogTO();
                    lto.TagID = tag;
                    lto.EventTime = dateTime;
                    lto.PassGenUsed = (int)Constants.PassGenUsed.Used;
                    Log log = new Log();
                    log.LgTO = lto;
                    List<LogTO> logList = log.Search();

                    if (logList.Count > 0)
                    {
                        lto = logList[0];
                        DateTime form = lto.EventTime;
                        DateTime to = lto.EventTime;

                        log.LgTO.PassGenUsed = (int)Constants.PassGenUsed.Used;
                        log.LgTO.TagID = 0;
                        logList = log.SearchForLogPeriod(form, to);
                        if (logList.Count == 2)
                        {
                            foreach (LogTO log1 in logList)
                            {
                                if (log1.TagID != lto.TagID)
                                {
                                    Employee persone = new Employee();
                                    logFound = lto;
                                    if (tagEmpl.ContainsKey(log1.TagID) && employeesDrivers.ContainsKey(tagEmpl[log1.TagID]))
                                    {
                                        driver = employeesDrivers[tagEmpl[log1.TagID]];
                                    }
                                }
                            }
                        }
                    }
                }

                return logFound;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.getNearestLog(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                return new LogTO();
            }
        }
    
        private LogTO getNearestLog(LogTO log, List<LogTO> logList)
        {
            LogTO nearest = new LogTO();
            try
            {
                nearest = logList[0];
                if (logList.Count == 1)
                    return nearest;
                else
                {
                    if (log.EventTime < nearest.EventTime)
                    {
                        foreach (LogTO l in logList)
                        {
                            if (l.EventTime < nearest.EventTime)
                                nearest = l;
                        }
                    }
                    else
                    {
                        foreach (LogTO l in logList)
                        {
                            if (l.EventTime > nearest.EventTime)
                                nearest = l;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.getNearestLog(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return nearest;
        }

        private void IOPairsReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sortOrderWithout = Constants.sortAsc;
                sortFieldWithout = VechicleIndex;
                lblTotal.Visible = false;

                dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpToDate.Value = DateTime.Now;
                this.CenterToScreen();
                logInUser = NotificationController.GetLogInUser();

                //List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                //find all tracks
                if (logInUser != null)
                {
                    //wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }
                bool canViewReport = false;
                foreach (WorkingUnitTO wu1 in wUnits)
                {
                    wuIDs.Add(wu1.WorkingUnitID);
                    if (wu1.WorkingUnitID == Constants.tracksWURootID)
                        canViewReport = true;
                }
                if (canViewReport)
                {
                    int selectedWorkingUnit = Constants.tracksWURootID;

                    WorkingUnit wu1 = new WorkingUnit();
                    if (selectedWorkingUnit != -1)
                    {
                        wu1.WUTO.WorkingUnitID = selectedWorkingUnit;
                        wUnits = wu1.Search();
                        wUnits = wu1.FindAllChildren(wUnits);
                    }

                    wuString = "";
                    foreach (WorkingUnitTO wu2 in wUnits)
                    {
                        if (wuIDs.Contains(wu2.WorkingUnitID))
                        {
                            wuString += wu2.WorkingUnitID.ToString().Trim() + ",";
                            trackWUIDs.Add(wu2.WorkingUnitID);
                        }
                    }

                    if (wuString.Length > 0)
                    {
                        wuString = wuString.Substring(0, wuString.Length - 1);
                    }
                    //list of all tracks
                    List<EmployeeTO> employeeList = new Employee().SearchByWU(wuString);
                    populateVechicleCombo(employeeList);
                    foreach (EmployeeTO empl in employeeList)
                    {
                        if (!employeesTracks.ContainsKey(empl.EmployeeID))
                            employeesTracks.Add(empl.EmployeeID, empl);
                    }

                    //find all drivers
                    findAllDrivers();

                    Tag tag = new Tag();
                    tag.TgTO.Status = Constants.statusActive;
                    List<TagTO> tags = tag.Search();
                    foreach (TagTO t in tags)
                    {
                        if (!tagEmpl.ContainsKey(t.TagID))
                            tagEmpl.Add(t.TagID, t.OwnerID);
                        if (!emplTag.ContainsKey(t.OwnerID))
                            emplTag.Add(t.OwnerID, t.TagID);
                    }
                    populateDirectionCombo();
                }
                else
                {
                    MessageBox.Show(rm.GetString("canNotSeeReport", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.IOPairsReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { this.Cursor = Cursors.Arrow; }
        }

        private void findAllDrivers()
        {
            try
            {
                //int selectedWorkingUnit = Constants.driversWURootID;

                WorkingUnit wu1 = new WorkingUnit();
                //if (selectedWorkingUnit != -1)
                //{
                //    wUnits = wu1.Search(selectedWorkingUnit.ToString(), "", "", "", "");
                //    wUnits = wu1.FindAllChildren(wUnits);
                //}

                string wuStr = "";
                //foreach (WorkingUnit wu2 in wUnits)
                //{
                //    if (wuIDs.Contains(wu2.WorkingUnitID))
                //        wuStr += wu2.WorkingUnitID.ToString().Trim() + ",";
                //}

                //04.09.2009 Natasa
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //drivers are employees from wu which isn't track wu
                List<WorkingUnitTO> list = wu1.Search();
                

                foreach (WorkingUnitTO wu2 in list)
                {
                    if (wuIDs.Contains(wu2.WorkingUnitID)&&!trackWUIDs.Contains(wu2.WorkingUnitID))
                        wuStr += wu2.WorkingUnitID.ToString().Trim() + ",";
                }
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                if (wuStr.Length > 0)
                {
                    wuStr = wuStr.Substring(0, wuStr.Length - 1);
                }

                //list of all tracks
                List<EmployeeTO> employeeList = new Employee().SearchByWU(wuStr);
                populatePersoneCombo(employeeList);
                foreach (EmployeeTO empl in employeeList)
                {
                    if(!employeesDrivers.ContainsKey(empl.EmployeeID))
                    employeesDrivers.Add(empl.EmployeeID, empl);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " UNIPROM.IOPairsReport.IOPairsReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Inner Class for sorting Array List of IOPairs

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort1 : IComparer<IOPairTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort1(int sortOrder,int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(IOPairTO x, IOPairTO y)
            {
                IOPairTO iop1 = null;
                IOPairTO iop2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    iop1 = x;
                    iop2 = y;
                }
                else
                {
                    iop1 = y;
                    iop2 = x;
                }

                if (compField == StartTimeIndex)
                    return iop1.StartTime.CompareTo(iop2.StartTime);
                else
                {
                    return iop1.EndTime.CompareTo(iop2.EndTime);
                }
            }
        }

        #endregion       

        private void populateVechicleCombo(List<EmployeeTO> array)
        {
            try
            {
                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                array.Insert(0, empl);

                foreach (EmployeeTO employee in array)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                cbVechicle.DataSource = array;
                cbVechicle.DisplayMember = "LastName";
                cbVechicle.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairs.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populatePersoneCombo(List<EmployeeTO> array)
        {
            try
            {
                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                array.Insert(0, empl);

                foreach (EmployeeTO employee in array)
                {
                    employee.LastName += " " + employee.FirstName;
                }
                cbPersone.DataSource = array;
                cbPersone.DisplayMember = "LastName";
                cbPersone.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairs.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateDirectionCombo()
        {
            try
            {
                cbDirection.Items.Add(rm.GetString("all", culture));
                cbDirection.Items.Add(Constants.DirectionIn);
                cbDirection.Items.Add(Constants.DirectionOut);
                //cbDirection.Items.Add(Constants.DirectionInOut);

                cbDirection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairsReport.populateDirectionCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Pass pass = new Pass();
                int vechicleID = -1;
                string direction = "";

                if (cbVechicle.SelectedIndex > 0)
                {
                    vechicleID = (int)cbVechicle.SelectedValue;
                }
                if (cbDirection.SelectedIndex > 0)
                {
                    direction = cbDirection.SelectedItem.ToString();

                }

                Pass p = new Pass();
                p.PssTO.EmployeeID = vechicleID;
                p.PssTO.Direction = direction;
                List<PassTO> trackPassesList = p.SearchInterval(dtpFromDate.Value.Date, dtpToDate.Value.Date, wuString, new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23,59, 0));

                List<PassTO> newPassesList = new List<PassTO>();

                foreach (PassTO trackPass in trackPassesList)
                {
                    if (trackPass.EventTime.TimeOfDay < dtFrom.Value.TimeOfDay || trackPass.EventTime.TimeOfDay > dtTo.Value.TimeOfDay)
                        continue;
                    Log log = new Log();
                    if (!emplTag.ContainsKey(trackPass.EmployeeID) || !employeesTracks.ContainsKey(trackPass.EmployeeID))
                        continue;
                    if (trackPass.EventTime != new DateTime())
                    {
                        uint tag = 0;
                        if (emplTag.ContainsKey(trackPass.EmployeeID))
                            tag = emplTag[trackPass.EmployeeID];
                        LogTO lto = new LogTO();
                        lto.TagID = tag;
                        lto.EventTime = trackPass.EventTime;
                        lto.PassGenUsed = (int)Constants.PassGenUsed.Used;
                        log.LgTO = lto;
                        List<LogTO> logList = log.Search();

                        if (logList.Count > 0)
                        {
                            lto = logList[0];
                            DateTime form = lto.EventTime;
                            DateTime to = lto.EventTime;

                            log.LgTO.TagID = 0;
                            log.LgTO.PassGenUsed = (int)Constants.PassGenUsed.Used;
                            logList = log.SearchForLogPeriod(form, to);
                            if (logList.Count == 2)
                            {
                                foreach (LogTO log1 in logList)
                                {
                                    if (log1.TagID != lto.TagID)
                                    {
                                        EmployeeTO persone = new EmployeeTO();
                                        if (tagEmpl.ContainsKey(log1.TagID) && employeesDrivers.ContainsKey((int)tagEmpl[log1.TagID]))
                                        {
                                            persone = employeesDrivers[tagEmpl[log1.TagID]];
                                        }

                                        trackPass.GateName = persone.LastName;
                                        trackPass.GateID = persone.EmployeeID;
                                    }
                                }
                            }
                        }
                        if (cbPersone.SelectedIndex == 0 || (int)cbPersone.SelectedValue == trackPass.GateID)
                        {
                            newPassesList.Add(trackPass);
                        }
                    }
                }
                currentTrackPassesList = newPassesList;
                currentTrackPassesList.Sort(new ArrayListSort(Constants.sortAsc, VechicleIndex));
                populateListView(currentTrackPassesList);
                this.lblTotal.Visible = true;
                this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentTrackPassesList.Count.ToString().Trim();

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairsReport.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        /// <summary>
        /// Populate List View with IOPairsReport found
        /// </summary>
        /// <param name="locationsList"></param>
        private void populateListView(List<PassTO> passesList)
        {
            try
            {
                lvPasses.BeginUpdate();
                lvPasses.Items.Clear();

                if (passesList.Count > 0)
                {
                    for (int i = 0; i < passesList.Count; i++)
                    {
                        PassTO pass = passesList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = pass.EmployeeName.ToString().Trim();
                        item.SubItems.Add(pass.GateName.Trim());
                        if (!pass.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                        {
                            item.SubItems.Add(pass.EventTime.ToString("dd/MM/yyyy   HH:mm"));
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }
                        item.SubItems.Add(pass.Direction.Trim());
                        item.Tag = pass;

                        lvPasses.Items.Add(item);
                    }
                }

                lvPasses.EndUpdate();
                lvPasses.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairsReport.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        #region Inner Class for sorting Array List of Passes

        /*
		 *  Class used for sorting Array List of Passes
		*/

        private class ArrayListSort : IComparer<PassTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(PassTO x, PassTO y)
            {
                PassTO pass1 = null;
                PassTO pass2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    pass1 = x;
                    pass2 = y;
                }
                else
                {
                    pass1 = y;
                    pass2 = x;
                }

                switch (compField)
                {
                    case IOPairsReport.VechicleIndex:
                        return pass1.EmployeeName.CompareTo(pass2.EmployeeName);
                    case IOPairsReport.PersoneIndex:
                        return pass1.GateName.CompareTo(pass2.GateName);
                    case IOPairsReport.DirectionIndex:
                        return pass1.Direction.CompareTo(pass2.Direction);
                    case IOPairsReport.EventTimeIndex:
                        return pass1.EventTime.CompareTo(pass2.EventTime);
                    default:
                        return pass1.EventTime.CompareTo(pass2.EventTime);
                }
            }
        }

        #endregion

        private void lvPasses_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrderWithout;

                if (e.Column == sortFieldWithout)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrderWithout = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrderWithout = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrderWithout = Constants.sortAsc;
                }

                sortFieldWithout = e.Column;

                currentTrackPassesList.Sort(new ArrayListSort(sortOrderWithout, sortFieldWithout));               
                populateListView(currentTrackPassesList);


            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " IOPairsReport.lvPasses_ColumnClick(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}