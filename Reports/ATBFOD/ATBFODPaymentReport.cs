using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports.ATBFOD
{
    public partial class ATBFODPaymentReport : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;

        DebugLog debug;
        List<WorkingUnitTO> wUnits;

        //shift's
        private const int firstShiftHrs = 7;
        private const int secondShiftHrs = 15;
        private const int thirdShiftHrs = 0;
        private const int firstShift = 1;
        private const int secondShift = 2;
        private const int thirdShift = 3;

        private const int TWO_HOURS = 120;
        
        // pass type IDs
        public const int RAD_I = 0101;
        public const int RAD_II = 0134;
        public const int RAD_III = 0106;
        public const int PREKOVR_DO2 = 0102;
        public const int PREKOVR_PREKO2 = 0103;
        //public const int PREKOVR_III = 0108;
       // public const int NED_I = 0128;
        public const int NED = 0105;
        //public const int NED_III = 0129;
        //public const int NED_PREKOVR_I = 0110;
        //public const int NED_PREKOVR_II = 0104;
        //public const int NED_PREKOVR_III = 0103;
        //public const int PRAZ_I = 0131;
        //public const int PRAZ_II = 0109;
        public const int PRAZ = 0132;
        //public const int DRUS_AKTIVNOST = 0203;
        public const int GOD_ODMOR = 0204;
        public const int PL_ODSUSTVO100 = 0205;
        public const int PL_ODSUSTVO65 = 0262;
        public const int BOL_DO30 = 0206;
        public const int NEPL_ODSUSTVO = 0209;
        public const int SL_PUT = 0210;
        //public const int VOJSKA = 0213;
        //public const int NEOPR_IZOST = 0217;
        public const int DRZ_PRAZNIK = 0233;
        //public const int BOL_ZAVOD = 0237;
        //public const int DA_KRVI = 0256;
        public const int BOL_100 = 0505;
       //public const int BOL_PREKO30_100 = 0614;
        public const int BOL_ZAVOD65 = 0615;
        //public const int N_DETETA = 0734;
        public const int PORODILJ = 0735;
        public const int PORODILJ_PREKO3 = 0736;
       // int[] passTypeIDs = { RAD_I, RAD_II, RAD_III, PREKOVR_I, PREKOVR_II, PREKOVR_III, NED_I, NED_II, NED_III, NED_PREKOVR_I, NED_PREKOVR_II, NED_PREKOVR_III, PRAZ_I, PRAZ_II, PRAZ_III,DRUS_AKTIVNOST, GOD_ODMOR, PL_ODSUSTVO, BOL_DO30,
       //NEPL_ODSUSTVO,SL_PUT,VOJSKA,NEOPR_IZOST,DRZ_PRAZNIK,BOL_ZAVOD,DA_KRVI, BOL_65,BOL_DO30_100,BOL_PREKO30_100,BOL_ZAVOD65,N_DETETA,PORODILJ};
        int[] passTypeIDs = { RAD_I, RAD_II, RAD_III, PREKOVR_DO2, PREKOVR_PREKO2,  NED,  PRAZ, GOD_ODMOR, PL_ODSUSTVO100,PL_ODSUSTVO65, BOL_DO30,
        NEPL_ODSUSTVO,SL_PUT,DRZ_PRAZNIK, BOL_100,BOL_ZAVOD65,PORODILJ,PORODILJ_PREKO3};

        // list of pairs for report
        List<IOPairTO> ioPairList = new List<IOPairTO>();

        // list of Time Schema for selected Employee and selected Time Interval
        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

        // list of Time Schedule for one month
        List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();

        // Key is Pass Type Id, Value is Pass Type
        Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();

        // all records for report
        ArrayList rowList = new ArrayList();

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();

        // all employee time schedules
        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        // Date of previous pair
        DateTime oldDate = new DateTime(0);
        
        // Counters processed employees 
        int empCount;

        Filter filter;

        public ATBFODPaymentReport()
        {
            InitializeComponent();
            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(WorkingUnitsReports).Assembly);
            setLanguage();
            logInUser = NotificationController.GetLogInUser();
            populateWorkigUnitCombo();

            this.CenterToScreen();

            // get all Pass Types
            List<PassTypeTO> passTypesAll = new PassType().Search();
            foreach (PassTypeTO pt in passTypesAll)
            {
                passTypes.Add(pt.PassTypeID, pt);
            }

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }

            // get all time schemas
            timeSchemas = new TimeSchema().Search();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReport.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("ATBFODPaymentReport", culture);

                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                gbTimeInterval.Text = rm.GetString("timeInterval", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
				
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<EmployeeTO> employees = new List<EmployeeTO>();
                List<int> employeesID = new List<int>();
                IOPair ioPair = new IOPair();
                int wuID = -1;
                string wUnits = "";
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (this.dtpFromDate.Value <= this.dtpToDate.Value)
                {
                    if (cbWorkingUnit.SelectedIndex > 0)
                    {
                        wuID = (int)cbWorkingUnit.SelectedValue;

                        if (this.chbHierarhicly.Checked)
                        {
                            WorkingUnit wu = new WorkingUnit();
                            wu.WUTO.WorkingUnitID = wuID;
                            wuArray = wu.Search();

                            wuArray = wu.FindAllChildren(wuArray);
                            wuID = -1;
                        }

                        foreach (WorkingUnitTO wu in wuArray)
                        {
                            wUnits += wu.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wUnits.Length > 0)
                        {
                            wUnits = wUnits.Substring(0, wUnits.Length - 1);
                        }
                        else
                        {
                            wUnits = wuID.ToString().Trim();
                        }

                        // find Employees for selected Working Unit
                        employees = new Employee().SearchByWU(wUnits.Trim());
                    }
                    else
                    {
                        // find all Employees
                        employees = new Employee().Search();
                        WorkingUnit workUnit = new WorkingUnit();
                        workUnit.WUTO.Status = Constants.DefaultStateActive;
                        List<WorkingUnitTO> wunit = workUnit.Search();
                        foreach (WorkingUnitTO wu in wunit)
                        {
                            wUnits += wu.WorkingUnitID.ToString().Trim() + ",";
                        }
                        if (wUnits.Length > 0)
                        {
                            wUnits = wUnits.Substring(0, wUnits.Length - 1);
                        }
                    }

                    foreach (EmployeeTO empl in employees)
                    {
                        if(empl.Status.Equals(Constants.statusActive)||empl.Status.Equals(Constants.statusBlocked))
                        // Employee IDs
                        employeesID.Add(empl.EmployeeID);
                    }

                    // get all valid IO Pairs for selected Working Unit and time interval
                    // get iopairs for one day more, becouse if employee start night shift in last day,
                    // hours of night shift from next are calculated together with hours from last day
                    // io pairs are sorted by wu_name, empl_last_name, empl_first_name, io_pair_date ascending
                    ioPairList = ioPair.SearchForWU(wuID, wUnits, dtpFromDate.Value, dtpToDate.Value.AddDays(1));

                    // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
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
                        emplTimeSchedules.Add(emplID, GetEmployeeTimeSchedules(emplID, dtpFromDate.Value, dtpToDate.Value.AddDays(1)));
                    }

                    this.label2.Text = "/";
                    this.label2.Refresh();
                    prbEmployee.Maximum = employeesID.Count;
                    this.lblEmployee.Text = employeesID.Count.ToString();
                    this.lblEmployee.Refresh();
                    this.lblCount.Text = "0";
                    this.lblCount.Refresh();
                  
                    ArrayList dataTable = new ArrayList();
                   
                    int employeeIDindex = -1;
                    foreach (int employeeID in employeesID)
                    {
                        employeeIDindex++;

                        // add rows with employee ID, and hours for every payment code

                        Hashtable paymentCodesHours = new Hashtable();
                        for (DateTime day = dtpFromDate.Value.Date; day <= dtpToDate.Value.Date; day = day.AddDays(1))
                        {
                            bool isRegularSchema = true;
                            Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
                            if (edi == null) continue;
                            Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                            IDictionaryEnumerator ediEnum = edi.GetEnumerator();
                            while (ediEnum.MoveNext())
                            {
                                employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
                            }

                            List<IOPairTO> edp = GetEmployeeDayPairs(emplPairs[employeeID], isRegularSchema, day);
                            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
                            foreach (IOPairTO ioPairTO in edp)
                            {
                                employeeDayPairs.Add(new IOPairTO(ioPairTO));
                            }

                            Hashtable dayPaymentCodesHours = null;
                            if (isRegularSchema)
                            {
                                dayPaymentCodesHours = CalculatePaymentPerRegularEmployeeDay(employeeID, employeeDayPairs, employeeDayIntervals, day);
                            }
                            else
                            {
                                dayPaymentCodesHours = CalculatePaymentPerBrigadeEmployeeDay(employeeID, employeeDayPairs, employeeDayIntervals, day);
                            }

                            IDictionaryEnumerator dayPaymentCodesHoursEnum = dayPaymentCodesHours.GetEnumerator();
                            while (dayPaymentCodesHoursEnum.MoveNext())
                            {
                                if (!paymentCodesHours.ContainsKey(dayPaymentCodesHoursEnum.Key))
                                {
                                    paymentCodesHours.Add(dayPaymentCodesHoursEnum.Key, dayPaymentCodesHoursEnum.Value);
                                }
                                else
                                {
                                    paymentCodesHours[dayPaymentCodesHoursEnum.Key] =
                                        ((TimeSpan)paymentCodesHours[dayPaymentCodesHoursEnum.Key]).Add((TimeSpan)dayPaymentCodesHoursEnum.Value);
                                }
                            }
                        }

                        ArrayList rowData = getRow(paymentCodesHours, getEmployee(employeeID, employees));
                        if (rowData.Count > 0)
                        {
                            dataTable.Add(rowData);
                        }
                            prbEmployee.Value++;
                            empCount++;
                            this.lblCount.Text = empCount.ToString();
                        
                        this.lblCount.Refresh();
                    }
                    DataTable table = new DataTable();
                    table.Columns.Add("sifraRadnika", typeof(System.String));
                    //table.Columns.Add("sifraRadnika", typeof(System.String));
                    //table.Columns.Add("ImeIPrezime", typeof(System.String));
                    table.Columns.Add("sifra101", typeof(System.String));
                    table.Columns.Add("sifra102", typeof(System.String));
                    table.Columns.Add("sifra103", typeof(System.String));
                    table.Columns.Add("sifra210", typeof(System.String));
                    table.Columns.Add("sifra134", typeof(System.String));
                    table.Columns.Add("sifra105", typeof(System.String));
                    table.Columns.Add("sifra106", typeof(System.String));
                    table.Columns.Add("sifra131", typeof(System.String));
                    table.Columns.Add("sifra132", typeof(System.String));
                    table.Columns.Add("sifra204", typeof(System.String));
                    table.Columns.Add("sifra206", typeof(System.String));
                    table.Columns.Add("sifra505", typeof(System.String));
                    table.Columns.Add("sifra615", typeof(System.String));
                    table.Columns.Add("sifra735", typeof(System.String));
                    table.Columns.Add("sifra736", typeof(System.String));
                    table.Columns.Add("sifra209", typeof(System.String));
                    table.Columns.Add("sifra205", typeof(System.String));
                    table.Columns.Add("sifra262", typeof(System.String));
                    //table.Columns.Add("sifra205", typeof(System.String));
                    //table.Columns.Add("sifra206", typeof(System.String));
                    //table.Columns.Add("sifra209", typeof(System.String));
                    //table.Columns.Add("sifra210", typeof(System.String));
                    //table.Columns.Add("sifra213", typeof(System.String));
                    //table.Columns.Add("sifra217", typeof(System.String));
                    //table.Columns.Add("sifra237", typeof(System.String));
                    //table.Columns.Add("sifra256", typeof(System.String));
                    //table.Columns.Add("sifra505", typeof(System.String));
                    //table.Columns.Add("sifra507", typeof(System.String));
                    //table.Columns.Add("sifra614", typeof(System.String));
                    //table.Columns.Add("sifra615", typeof(System.String));
                    //table.Columns.Add("sifra734", typeof(System.String));
                    //table.Columns.Add("sifra735", typeof(System.String));
                    //table.Columns.Add("SUMA", typeof(System.String));
                    if (dataTable.Count > 0)
                    {
                        foreach (ArrayList rowData in dataTable)
                        {
                            DataRow row = table.NewRow();
                            for (int i = 0; i < rowData.Count; i++)
                            {
                                string st = (string)rowData[i];
                                //if (st.Equals("0"))
                                //    st = "";
                                row[i] = st;
                            }

                            table.Rows.Add(row);                            
                        }

                        table.AcceptChanges();
                    }

                    generateCSVReport(table);
                }
                else
                {
                    MessageBox.Show(rm.GetString("wrongDatePickUp", culture));
                    return;
                }

                this.label2.Text = "";                
                
                this.Close();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.btnGenerate_Click(): " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private EmployeeTO getEmployee(int employeeID, List<EmployeeTO> emplList)
        {
            EmployeeTO employee = new EmployeeTO();
            try 
            {
                foreach (EmployeeTO empl in emplList)
                {
                    if (empl.EmployeeID == employeeID)
                        employee = empl;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.getEmployee(): " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
            return employee;
        }

        private ArrayList getRow(Hashtable paymentCodesHours, EmployeeTO empl)
        {
            ArrayList list = new ArrayList();
            try
            {
                if (paymentCodesHours.Count > 0)
                {
                    //list.Add(empl.WorkingUnitID.ToString());
                    list.Add(empl.EmployeeID.ToString().Substring(0,empl.EmployeeID.ToString().Length-1));
                    //list.Add(empl.LastName+" " + empl.FirstName);
                    list.Add(((TimeSpan)paymentCodesHours[RAD_I]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PREKOVR_DO2]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PREKOVR_PREKO2]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[SL_PUT]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[RAD_II]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[NED]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[RAD_III]).Hours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[DRZ_PRAZNIK]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PRAZ]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[GOD_ODMOR]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[BOL_DO30]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[BOL_100]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[BOL_ZAVOD65]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PORODILJ]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PORODILJ_PREKO3]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[NEPL_ODSUSTVO]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PL_ODSUSTVO100]).TotalHours.ToString());
                    list.Add(((TimeSpan)paymentCodesHours[PL_ODSUSTVO65]).TotalHours.ToString());                    
                    //TimeSpan total = new TimeSpan();
                    //foreach(int i in paymentCodesHours.Keys)
                    //{
                    //    TimeSpan ts = (TimeSpan)paymentCodesHours[i];
                    //    total += ts;
                    //}
                    //list.Add(total.TotalHours.ToString());
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODPaymentReports.getRow(): " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
            return list;
        }

        private Hashtable CalculatePaymentPerBrigadeEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            int shift = 0;
            if (dayIntervals.Count > 0)
                shift = getShift(dayIntervals);

            Hashtable paymentCodesHours = new Hashtable();

            InitializePaymentCodesHours(paymentCodesHours);

            // sumiraj predvidjeni broj radnih sati za dan
            TimeSpan totalWorkingHours = CalculateTotalWorkingHours(dayIntervals);

            // izbaci parove koji se ne odnose na radne sate
            IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if (iopair.IsWrkHrsCount == 0)
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            //// ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
            //if (ioPairs.Count <= 0 && !isHoliday(day))
            //{
            //    paymentCodesHours[NEOPR_IZOST] = (TimeSpan)paymentCodesHours[NEOPR_IZOST] + totalWorkingHours;
            //    return paymentCodesHours;
            //}
            
            // pozovi metodu koja vraca prekovremeni rad u minutima za tog zaposlenog za taj dan iz tabele extra_hours_used i izracunati totalPrekRad
            TimeSpan totalPrekRad = new TimeSpan(0, 0, 0);
            int totalPrekRadMinutes = (new ExtraHourUsed()).SearchEmployeeUsedSumByType(employeeID, day, day, Constants.extraHoursUsedOvertime);
            totalPrekRad += new TimeSpan(0, totalPrekRadMinutes, 0);
            // izracunaj prekovremeno i u zavisnosti od toga da li je vikend i koja je smena dodeljena radniku dodeli sate odgovarajucoj koloni
            if (totalPrekRadMinutes > 0)
            {
                if (!isWeekend(day))
                {
                    if (totalPrekRadMinutes <= TWO_HOURS)
                        paymentCodesHours[PREKOVR_DO2] = totalPrekRad;
                    else
                        paymentCodesHours[PREKOVR_PREKO2] = totalPrekRad;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[PREKOVR_I] = totalPrekRad;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[PREKOVR_II] = totalPrekRad;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[PREKOVR_III] = totalPrekRad;
                    //        break;
                    //}
                }
                else
                {
                    if (totalPrekRadMinutes <= TWO_HOURS)
                        paymentCodesHours[PREKOVR_DO2] = totalPrekRad;
                    else
                        paymentCodesHours[PREKOVR_PREKO2] = totalPrekRad;
                    //paymentCodesHours[NED] = (TimeSpan)paymentCodesHours[NED] + totalPrekRad;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[NED_PREKOVR_I] = totalPrekRad;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[NED_PREKOVR_II] = totalPrekRad;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[NED_PREKOVR_III] = totalPrekRad;
                    //        break;
                    //}
                }
            }

            // ako je broj predvidjenih radnih sati za dan 0, zavrsi obradu
            if (totalWorkingHours == new TimeSpan(0, 0, 0)) return paymentCodesHours;

            // izbaci nepotrebne parove i podesi dolazak i odlazak uzimajuci u obzir pravila dolaska i odlaska
            if (!isNightShiftDay(dayIntervals))
            {
                CleanUpDayPairs(ioPairs, dayIntervals, day);
            }
            else
            {
                CleanUpNightPairs(employeeID, ioPairs, dayIntervals, day);
            }

            // ako je bolovanje dodaj predvidjen broj radnih sati na odgovarajuci paymentCode i zavrsi obradu
            foreach (IOPairTO iopair in ioPairs)
            {
                PassTypeTO passType = passTypes[iopair.PassTypeID];
                if (Constants.sickLeaveTable.ContainsKey(iopair.PassTypeID))
                {
                    if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                    {
                        paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + totalWorkingHours;
                        return paymentCodesHours;
                    }
                }
            }

            // ako je praznik i nije vikend dodaj predvidjen broj radnih sati na DRZ_PRAZNIK (drzavni praznik) i zavrsi obradu            
            if (isHoliday(day))
            {
                if (ioPairs.Count <= 0)
                {
                    return paymentCodesHours;

                }
                    //ako ima parova proveri da li je godisnji odmor i dodaj na naknadu za praznik
                    //ako ima parova dodaj trajanje u rad na drzavni praznik u zavisnosti od smene
                else
                {
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        PassTypeTO passType = passTypes[iopair.PassTypeID];
                        if (Constants.vacationTable.ContainsKey(iopair.PassTypeID))
                        {
                            if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                            {
                                paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + totalWorkingHours;
                                return paymentCodesHours;
                            }
                        }
                    }

                    // sumiraj trajanje svih parova oni predstavljaju rad na praznik
                    TimeSpan totalDuration = new TimeSpan(0, 0, 0);
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                       (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut))
                        {
                            totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                               totalDuration = totalDuration.Add(new TimeSpan(0, 1, 0));
                        }
                    }
                    totalDuration = roundHours(totalDuration);
                    paymentCodesHours[PRAZ] = (TimeSpan)paymentCodesHours[PRAZ] + totalDuration;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[PRAZ_I] = (TimeSpan)paymentCodesHours[PRAZ_I] + totalDuration;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[PRAZ_II] = (TimeSpan)paymentCodesHours[PRAZ_II] + totalDuration;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[PRAZ_III] = (TimeSpan)paymentCodesHours[PRAZ_III] + totalDuration;
                    //        break;
                    //}
                }
                //if (!isWeekend(day))
                //{
                //    return paymentCodesHours;
                //}
                 //ako je nedelje dodaj trajanje u rad nedeljom u zavisnosti od smene
                if (isWeekend(day))
                {
                    // sumiraj trajanje svih parova oni predstavljaju rad nedeljom
                    TimeSpan totalDuration = new TimeSpan(0, 0, 0);
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                       (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut))
                        {
                            totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                totalDuration = totalDuration.Add(new TimeSpan(0, 1, 0));
                        }
                    }
                    totalDuration = roundHours(totalDuration);
                    paymentCodesHours[NED] = (TimeSpan)paymentCodesHours[NED] + totalDuration;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[NED_I] = (TimeSpan)paymentCodesHours[NED_I] + totalDuration;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[NED_II] = (TimeSpan)paymentCodesHours[NED_II] + totalDuration;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[NED_III] = (TimeSpan)paymentCodesHours[NED_III] + totalDuration;
                    //        break;
                    //}                   
                    //return paymentCodesHours;
                }
            }            

            // ako je celodnevno odsustvo dodaj predvidjen broj radnih sati na odgovarajuci paymentCode i zavrsi obradu
            foreach (IOPairTO iopair in ioPairs)
            {
                PassTypeTO passType = passTypes[iopair.PassTypeID];
                if (passType.IsPass == Constants.wholeDayAbsence)
                {
                    if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                    {
                        paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + totalWorkingHours;
                        return paymentCodesHours;
                    }
                }
            }

            //ako je nedelje i nema parova zavrsi obradu
            if (isWeekend(day))
            {
                if (ioPairs.Count <= 0)
                {
                    return paymentCodesHours;
                }

                //ako ima parova dodaj trajanje u rad nedeljom u zavisnosti od smene
                else
                {
                    // sumiraj trajanje svih parova oni predstavljaju rad nedeljom
                    TimeSpan totalDuration = new TimeSpan(0, 0, 0);
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                       (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut))
                        {
                            totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                totalDuration = totalDuration.Add(new TimeSpan(0, 1, 0));
                        }
                    }
                    totalDuration = roundHours(totalDuration);
                    paymentCodesHours[NED] = (TimeSpan)paymentCodesHours[NED] + totalDuration;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[NED_I] = (TimeSpan)paymentCodesHours[NED_I] + totalDuration;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[NED_II] = (TimeSpan)paymentCodesHours[NED_II] + totalDuration;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[NED_III] = (TimeSpan)paymentCodesHours[NED_III] + totalDuration;
                    //        break;
                    //}
                }
               // return paymentCodesHours;
            }

            //// ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
            //if (ioPairs.Count <= 0)
            //{
            //    paymentCodesHours[NEOPR_IZOST] = (TimeSpan)paymentCodesHours[NEOPR_IZOST] + totalWorkingHours;
            //    return paymentCodesHours;
            //}


            //izracunaj redovan rad po smenama
            TimeSpan totalRegularWork = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                    (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut)||(iopair.PassTypeID == Constants.extraHours))
                {
                    totalRegularWork += (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                       totalRegularWork = totalRegularWork.Add(new TimeSpan(0, 1, 0));
                }
                else
                {
                    PassTypeTO passType = passTypes[iopair.PassTypeID];
                    if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                    {
                        paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    }
                    else
                    {
                        MessageBox.Show("Tip prolaska " + iopair.PassTypeID.ToString() + " ne moze biti obradjen! Proverite podatke za zaposlenog " + employeeID.ToString() +
                                        " za dan " + day.ToShortDateString(), "ACTA Info");
                        return paymentCodesHours;
                    }
                }
            }

            totalRegularWork = roundHours(totalRegularWork);
            if (totalRegularWork.TotalMinutes > 0)
            {
                paymentCodesHours[RAD_I] = (TimeSpan)paymentCodesHours[RAD_I] + totalRegularWork;
                switch (shift)
                {
                    case firstShift:                        
                        break;
                    case secondShift:
                        paymentCodesHours[RAD_II] = (TimeSpan)paymentCodesHours[RAD_II] + totalRegularWork;
                        break;
                    case thirdShift:
                        paymentCodesHours[RAD_III] = (TimeSpan)paymentCodesHours[RAD_III] + totalRegularWork;
                        break;
                }
            }

            return paymentCodesHours;
        }

        private TimeSpan roundHours(TimeSpan totalRegularWork)
        {
            TimeSpan ts = totalRegularWork;
            try
            {
                if (totalRegularWork.Minutes > 0||totalRegularWork.Seconds>0)
                    ts = new TimeSpan(totalRegularWork.Hours,0,0);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WorkingUnitsReports.roundHours(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return ts;
        }

        //private void CleanUpNightPairs(int employeeID, ArrayList ioPairs, Hashtable dayIntervals, DateTime day)
        //{
        //    // night shift day can have 1 or 2 intervals
        //    if (dayIntervals.Count == 1)
        //    {
        //        // if night shift day has only 00:00- interval it's already calculated as yesterday
        //        // and should be dropped
        //        if (((TimeSchemaInterval)dayIntervals[0]).StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
        //        {
        //            ioPairs.Clear();
        //            dayIntervals.Clear();
        //            return;
        //        }
        //        // if night shift day has only -23:59 interval make it normal night shift day adding
        //        // tomorrow's first interval
        //        else
        //        {
        //            dayIntervals.Add(1, (TimeSchemaInterval)dayIntervals[0]);
        //            bool dummy = true;
        //            Hashtable tomorrowIntervals = GetDayTimeSchemaIntervals((ArrayList)emplTimeSchedules[employeeID], day.AddDays(1), ref dummy);
        //            if (tomorrowIntervals != null) dayIntervals[0] = tomorrowIntervals[0];
        //            else
        //            {
        //                debug.writeLog(DateTime.Now + " ATBFODPaymentReports.CleanUpNightPairs(): Tomorrow intervals cannot be found! Employee: " + employeeID.ToString() + " Day: " + day.ToShortDateString() + "\n");
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
        //            (iopair.StartTime.TimeOfDay > ((TimeSchemaInterval)dayIntervals[0]).EndTime.TimeOfDay) &&
        //            (iopair.StartTime.TimeOfDay < ((TimeSchemaInterval)dayIntervals[1]).StartTime.TimeOfDay) &&
        //            (iopair.EndTime.TimeOfDay > ((TimeSchemaInterval)dayIntervals[0]).EndTime.TimeOfDay) &&
        //            (iopair.EndTime.TimeOfDay < ((TimeSchemaInterval)dayIntervals[1]).StartTime.TimeOfDay)
        //            )
        //            )
        //        {
        //            ioPairs.Remove(ioPairsEnum.Current);
        //            ioPairsEnum = ioPairs.GetEnumerator();
        //        }
        //    }
        //    if (ioPairs.Count <= 0) return;

        //    // remove morning pairs belonging to the previous day
        //    ioPairsEnum = ioPairs.GetEnumerator();
        //    while (ioPairsEnum.MoveNext())
        //    {
        //        IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
        //        if ((iopair.IOPairDate == day) && (iopair.StartTime.TimeOfDay < new TimeSpan(firstShiftHrs,0,0)))//((TimeSchemaInterval)dayIntervals[0]).EndTime.TimeOfDay))
        //        {
        //            ioPairs.Remove(ioPairsEnum.Current);
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
        //    if (firstPair.StartTime.TimeOfDay <= ((TimeSchemaInterval)dayIntervals[firstPairInterval]).LatestArrivaed.TimeOfDay)
        //    {
        //        firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + ((TimeSchemaInterval)dayIntervals[firstPairInterval]).StartTime.TimeOfDay);
        //    }
        //    //else
        //    //{
        //    //    if (firstPair.StartTime.Minute != 0)
        //    //    {
        //    //        firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1, -firstPair.StartTime.Minute, -firstPair.StartTime.Second));
        //    //    }
        //    //}
        //    if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

        //    // round down last pair end time to full hour, considering end tolerance
        //    if (lastPair.EndTime.TimeOfDay >= ((TimeSchemaInterval)dayIntervals[lastPairInterval]).EarliestLeft.TimeOfDay)
        //    {
        //        lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + ((TimeSchemaInterval)dayIntervals[lastPairInterval]).EndTime.TimeOfDay);
        //    }
        //    //else
        //    //{
        //    //    if (lastPair.EndTime.Minute != 0)
        //    //    {
        //    //        lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0, -lastPair.EndTime.Minute, -lastPair.EndTime.Second));
        //    //    }
        //    //}
        //    if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        //}

        void CleanUpNightPairs(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            // night shift day can have 1 or 2 intervals
            if (dayIntervals.Count == 1)
            {
                // if night shift day has only 00:00- interval it's already calculated as yesterday
                // and should be dropped
                if (dayIntervals[0].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    ioPairs.Clear();
                    dayIntervals.Clear();
                    return;
                }
                // if night shift day has only -23:59 interval make it normal night shift day adding
                // tomorrow's first interval
                else
                {
                    dayIntervals.Add(1, dayIntervals[0]);
                    bool dummy = true;
                    Dictionary<int, WorkTimeIntervalTO> tomorrowIntervals = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day.AddDays(1), ref dummy);
                    if (tomorrowIntervals != null) dayIntervals[0] = tomorrowIntervals[0];
                    else
                    {
                        debug.writeLog(DateTime.Now + " PaymentReports.CleanUpNightPairs(): Tomorrow intervals cannot be found! Employee: " + employeeID.ToString() + " Day: " + day.ToShortDateString() + "\n");
                        return;
                    }
                }
            }

            // remove invalid pairs and pairs outside of the interval
            IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if (
                    (iopair.StartTime > iopair.EndTime) ||
                    (
                    (iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) &&
                    (iopair.StartTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay) &&
                    (iopair.EndTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) &&
                    (iopair.EndTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay)
                    )
                    )
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return;

            // remove morning pairs belonging to the previous day
            ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if ((iopair.IOPairDate == day) && (iopair.StartTime.TimeOfDay < dayIntervals[0].EndTime.TimeOfDay))
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return;

            // find first and last pair
            IOPairTO firstPair = ioPairs[0];
            IOPairTO lastPair = firstPair;
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
                if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
            }

            // find the proper interval for the first and last pair considering the day
            int firstPairInterval = (firstPair.IOPairDate == day) ? 1 : 0;
            int lastPairInterval = (lastPair.IOPairDate == day) ? 1 : 0;

            // round up first pair start time to full hour, considering start tolerance
            if (firstPair.StartTime.TimeOfDay <= dayIntervals[firstPairInterval].LatestArrivaed.TimeOfDay)
            {
                firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[firstPairInterval].StartTime.TimeOfDay);
            }
            else
            {
                if (firstPair.StartTime.Minute != 0)
                {
                    firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1, -firstPair.StartTime.Minute, -firstPair.StartTime.Second));
                }
            }
            if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

            // round down last pair end time to full hour, considering end tolerance
            if (lastPair.EndTime.TimeOfDay >= dayIntervals[lastPairInterval].EarliestLeft.TimeOfDay)
            {
                lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[lastPairInterval].EndTime.TimeOfDay);
            }
            else
            {
                if (lastPair.EndTime.Minute != 0)
                {
                    lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0, -lastPair.EndTime.Minute, -lastPair.EndTime.Second));
                }
            }
            if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        }

        // gets employee's working intervals for the given day
        private Dictionary<int, WorkTimeIntervalTO> GetDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> employeeTimeScheduleList, DateTime day, ref bool isRegularSchema)
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
            foreach (WorkTimeSchemaTO timeSchema in timeSchemas)
            {
                if (timeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = timeSchema;
                    break;
                }
            }
            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + day.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(day.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            Dictionary<int, WorkTimeIntervalTO> intervals = actualTimeSchema.Days[dayNum];

            isRegularSchema = isRegularTimeSchema(actualTimeSchema);

            return intervals;
        }

        // Gets all the employee's io pairs for the given working day
        private List<IOPairTO> GetEmployeeDayPairs(List<IOPairTO> emplPairs, bool isRegularSchema, DateTime day)
        {
            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
            foreach (IOPairTO iopair in emplPairs)
            {
                if (iopair.IOPairDate.Date == day.Date)
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

        //vraca hashtable kojoj je kljuc sifra placanja a vrednost je interval
        private Hashtable CalculatePaymentPerRegularEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            int shift =0;
            if(dayIntervals.Count>0)
            shift = getShift(dayIntervals);
            
            Hashtable paymentCodesHours = new Hashtable();

            InitializePaymentCodesHours(paymentCodesHours);

            // sumiraj predvidjeni broj radnih sati za dan
            TimeSpan totalWorkingHours = CalculateTotalWorkingHours(dayIntervals);

            // izbaci parove koji se ne odnose na radne sate
            IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if (iopair.IsWrkHrsCount == 0)
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            //// ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
            //if (ioPairs.Count <= 0 && !isHoliday(day))
            //{
            //    paymentCodesHours[NEOPR_IZOST] = (TimeSpan)paymentCodesHours[NEOPR_IZOST] + totalWorkingHours;
            //    return paymentCodesHours;
            //}

            // pozovi metodu koja vraca prekovremeni rad u minutima za tog zaposlenog za taj dan iz tabele extra_hours_used i izracunati totalPrekRad
            TimeSpan totalPrekRad = new TimeSpan(0, 0, 0);
            int totalPrekRadMinutes = (new ExtraHourUsed()).SearchEmployeeUsedSumByType(employeeID, day, day, Constants.extraHoursUsedOvertime);
            totalPrekRad += new TimeSpan(0, totalPrekRadMinutes, 0);
            // izracunaj prekovremeno i u zavisnosti od toga da li je vikend i koja je smena dodeljena radniku dodeli sate odgovarajucoj koloni
            if(totalPrekRadMinutes>0)
            {               
                if (!isWeekend(day))
                {
                    if (totalPrekRadMinutes <= TWO_HOURS)
                        paymentCodesHours[PREKOVR_DO2] = totalPrekRad;
                    else
                        paymentCodesHours[PREKOVR_PREKO2] = totalPrekRad;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[PREKOVR_I] = totalPrekRad;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[PREKOVR_II] = totalPrekRad;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[PREKOVR_III] = totalPrekRad;
                    //        break;
                    //}
                }
                else
                {
                    if (totalPrekRadMinutes <= TWO_HOURS)
                        paymentCodesHours[PREKOVR_DO2] = totalPrekRad;
                    else
                        paymentCodesHours[PREKOVR_PREKO2] = totalPrekRad;
                    //paymentCodesHours[NED] = (TimeSpan)paymentCodesHours[NED] + totalPrekRad;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[NED_PREKOVR_I] = totalPrekRad;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[NED_PREKOVR_II] = totalPrekRad;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[NED_PREKOVR_III] = totalPrekRad;
                    //        break;
                    //}
                }

            }
            // ako je broj predvidjenih radnih sati za dan 0, zavrsi obradu
            if (totalWorkingHours == new TimeSpan(0, 0, 0)) return paymentCodesHours;

            // izbaci nepotrebne parove i podesi dolazak i odlazak uzimajuci u obzir pravila dolaska i odlaska
            if (!isNightShiftDay(dayIntervals))
            {
                CleanUpDayPairs(ioPairs, dayIntervals, day.Date);
            }
            else
            {
                CleanUpNightPairs(employeeID, ioPairs, dayIntervals, day.Date);
            }

            // ako je praznik i nije vikend dodaj predvidjen broj radnih sati na DRZ_PRAZNIK (drzavni praznik) i zavrsi obradu            
            if (isHoliday(day))
            {
                paymentCodesHours[DRZ_PRAZNIK] = (TimeSpan)paymentCodesHours[DRZ_PRAZNIK] + totalWorkingHours;

                if (!isWeekend(day))
                {
                    return paymentCodesHours;
                }
            }

            //ako je nedelje i nema parova zavrsi obradu
            if (isWeekend(day))
            {
                if (ioPairs.Count <= 0)
                {
                    return paymentCodesHours;
                }

                //ako ima parova dodaj trajanje u rad nedeljom u zavisnosti od smene
                else
                {
                    // sumiraj trajanje svih parova oni predstavljaju rad nedeljom
                    TimeSpan totalDuration = new TimeSpan(0, 0, 0);
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                       (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut))
                        {
                            totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                            if(iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                totalDuration.Add(new TimeSpan(0,1,0));
                        }
                    }
                    totalDuration = roundHours(totalDuration);
                    paymentCodesHours[NED] = (TimeSpan)paymentCodesHours[NED] + totalDuration;
                    //switch (shift)
                    //{
                    //    case firstShift:
                    //        paymentCodesHours[NED_I] = (TimeSpan)paymentCodesHours[NED_I] + totalDuration;
                    //        break;
                    //    case secondShift:
                    //        paymentCodesHours[NED_II] = (TimeSpan)paymentCodesHours[NED_II] + totalDuration;
                    //        break;
                    //    case thirdShift:
                    //        paymentCodesHours[NED_III] = (TimeSpan)paymentCodesHours[NED_III] + totalDuration;
                    //        break;
                    //}
                }
               // return paymentCodesHours;
            }

            // ako je celodnevno odsustvo dodaj predvidjen broj radnih sati na odgovarajuci paymentCode i zavrsi obradu
            foreach (IOPairTO iopair in ioPairs)
            {
                PassTypeTO passType = passTypes[iopair.PassTypeID];
                if (passType.IsPass == Constants.wholeDayAbsence)
                {
                     if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                    {
                        paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + totalWorkingHours;
                        return paymentCodesHours;
                    }                    
                }
            }
                       
            //// ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
            //if (ioPairs.Count <= 0)
            //{
            //    paymentCodesHours[NEOPR_IZOST] = (TimeSpan)paymentCodesHours[NEOPR_IZOST] + totalWorkingHours;
            //    return paymentCodesHours;
            //}

           
            //izracunaj redovan rad po smenama
            TimeSpan totalRegularWork = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                    (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut) || (iopair.PassTypeID == Constants.extraHours))
                {
                    totalRegularWork += (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                       totalRegularWork = totalRegularWork.Add(new TimeSpan(0, 1, 0));
                }
                else
                {
                    PassTypeTO passType = passTypes[iopair.PassTypeID];
                    if (paymentCodesHours.ContainsKey(int.Parse(passType.PaymentCode)))
                    {
                        paymentCodesHours[int.Parse(passType.PaymentCode)] = (TimeSpan)paymentCodesHours[int.Parse(passType.PaymentCode)] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    }
                    else
                    {
                        MessageBox.Show("Tip prolaska " + iopair.PassTypeID.ToString() + " ne moze biti obradjen! Proverite podatke za zaposlenog " + employeeID.ToString() +
                                        " za dan " + day.ToShortDateString(), "ACTA Info");
                        return paymentCodesHours;
                    }
                }
            }
            totalRegularWork = roundHours(totalRegularWork);
            if (totalRegularWork.TotalMinutes>0)
            {
                paymentCodesHours[RAD_I] = (TimeSpan)paymentCodesHours[RAD_I] + totalRegularWork;
                switch (shift)
                { 
                    case firstShift:                        
                        break;
                    case secondShift:
                        paymentCodesHours[RAD_II] = (TimeSpan)paymentCodesHours[RAD_II] + totalRegularWork;
                        break;
                    case thirdShift:
                        paymentCodesHours[RAD_III] = (TimeSpan)paymentCodesHours[RAD_III] + totalRegularWork;
                        break;
                }
            }

            return paymentCodesHours;
        }

        private bool isNightShiftDay(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
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
        /// <summary>
        /// removes unneeded pairs and trim first and last pair to the working time interval boundaries
        /// considering latency rules
        /// </summary>
        /// <param name="ioPairs"></param>
        /// <param name="dayIntervals"></param>
        void CleanUpDayPairs(List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            // remove invalid pairs and pairs outside of the interval
            IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if ((iopair.IOPairDate != day) || (iopair.StartTime > iopair.EndTime) ||
                    (iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) ||
                    (iopair.EndTime.TimeOfDay < dayIntervals[0].StartTime.TimeOfDay))
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return;

            // find first and last pair
            IOPairTO firstPair = ioPairs[0];
            IOPairTO lastPair = firstPair;
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
                if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
            }

            // round up first pair start time to full hour, considering start tolerance
            if (firstPair.StartTime.TimeOfDay <= dayIntervals[0].LatestArrivaed.TimeOfDay)
            {
                firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[0].StartTime.TimeOfDay);
            }
            //else
            //{
            //    if (firstPair.StartTime.Minute != 0)
            //    {
            //        firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1, -firstPair.StartTime.Minute, -firstPair.StartTime.Second));
            //    }
            //}
            if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

            // round down last pair end time to full hour, considering end tolerance
            if (lastPair.EndTime.TimeOfDay >= dayIntervals[0].EarliestLeft.TimeOfDay)
            {
                lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[0].EndTime.TimeOfDay);
            }
            //else
            //{
            //    if (lastPair.EndTime.Minute != 0)
            //    {
            //        lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0, -lastPair.EndTime.Minute, -lastPair.EndTime.Second));
            //    }
            //}
            if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        }

        void InitializePaymentCodesHours(Hashtable paymentCodesHours)
        {
            // ulazi za obracun plata
            for (int i = 0; i < passTypeIDs.Length; i++)
            {
                paymentCodesHours.Add(passTypeIDs[i], new TimeSpan(0, 0, 0));
            }           
        }

        TimeSpan CalculateTotalWorkingHours(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            TimeSpan totalWorkingHours = new TimeSpan(0, 0, 0);
            IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator();
            while (dayIntervalsEnum.MoveNext())
            {
                WorkTimeIntervalTO interval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
                totalWorkingHours += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
            }
            if ((totalWorkingHours.Hours == 7) && (totalWorkingHours.Minutes == 59)) totalWorkingHours = new TimeSpan(8, 0, 0);
            return totalWorkingHours;
        }

        int getShift(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            int shift = 0;
            try
            {
                WorkTimeIntervalTO interval = dayIntervals[0];
                switch (interval.StartTime.TimeOfDay.Hours)
                {
                    case firstShiftHrs:
                        shift = firstShift;
                        break;
                    case secondShiftHrs:
                        shift = secondShift;
                        break;
                    case thirdShiftHrs:
                        shift = thirdShift;
                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.btnGenerate_Click(): " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
            return shift;
        }

        bool isWeekend(DateTime day)
        {
            return ((day.DayOfWeek == DayOfWeek.Sunday));
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        void FillReportRowList(int employeeID, Employee emplData, Hashtable payTotal)
        {
            string row = "'" + employeeID.ToString() + "'";
            for (int i = 0; i < passTypeIDs.Length; i++)
            {
                TimeSpan ts = (TimeSpan)payTotal[passTypeIDs[i]];
                float tsFloat = (float)ts.TotalHours;
                row += "," + tsFloat.ToString("F2").Replace(",", ".");
            }
            rowList.Add(row);
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

        bool isRegularTimeSchema(WorkTimeSchemaTO schema)
        {
            return schema.Type.Equals(Constants.regularTimeSchema);
        }

        private void generateCSVReport(DataTable table)
        {
            try
            {
                // Specify the column list to export
                int[] iColumns = { 0, 1, 2, 3, 4 };
                //string[] cHeaders = {"Šifra mesta troška","Br.rada radnika","Ime i prezime", "101", "103", "104", "105", "106", "107", "108", "109", "110", "128", "129", "130", "131", "132", "134", "233","203","204", "205", "206", "209", "210", "213", "217", "237", "256", "505", "507", "614", "615", "734","735","SUMA"};
                string[] cHeaders = { "Platni broj", "Tehnološki rad", "Prekovremeni do 2 h", "Prekovremeni preko 2 h", "Službeni put", "Rad u II smeni", "Rad nedeljom", "Noćni rad", "Državni praznik", "Rad praznikom", "Godišnji odmor", "Bolovanje do 30 dana", "Bolovanje 100%", "Bolovanje fond", "Porođajno do 3 deteta", "Porođajno preko 3 deteta", "Neplaćeno odsustvo", "Plaćeno odsustvo 100%", "Plaćeno odsustvo 60%" };
                // Export the details of specified columns to Excel
                
                if (table.Rows.Count == 0)
                {
                    //MessageBox.Show("There are no data to export to CSV!");
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                }
                else
                {
                    //19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
                    Export objExport = new Export("Win", cHeaders);

                    //RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                    string wu = "";
                    if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
                    {
                        wu = rm.GetString("repAll", culture);
                    }
                    else
                    {
                        wu = cbWorkingUnit.Text.Trim();
                    }
                    string fileName = Constants.csvDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
                    string delimiter = "\t";
                    objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName, delimiter);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateSummaryCSVReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void ATBFODPaymentReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
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
                MessageBox.Show(ex.Message);
            }
            finally {
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
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }       
    }
}