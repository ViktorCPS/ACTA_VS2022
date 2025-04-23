using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;
using System.IO;
using System.Data.OleDb;

using Common;
using TransferObjects;
using Util;

namespace Reports.PIO
{
	/// <summary>
	/// Summary description for WorkingUnitsReports.
	/// </summary>
	public class PIOPaymentReports : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		DebugLog debug;

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
		// Date of currant pair
		DateTime currentDate = new DateTime(0);
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ProgressBar prbEmployee;

		// Counters processed employees 
		int empCount;

		// Working Units that logInUser is granted to
		//ArrayList wUnits;

		// pass type IDs
		public const int RAD = 0;
		public const int GO_ODM = 5;
		public const int DIV_PRAZ = 10;
		public const int PLA_OD = 9;
		public const int BOL = 6;
		public const int BOL_P3 = 11;
		public const int PORODILJ = 12;
		public const int VOJSKA = 13;
		public const int IZOST = -100;
		public const int N_DETETA = 14;
		public const int BOL100 = 15;
		public const int NEPLA_OD = 8;
		public const int PREK_RAD = 16;
		public const int PRERA = -101;
		public const int SLAVA = 17;
		public const int DA_KRV = 18;
		public const int SL_PUTI = 19;
		public const int SL_PUTZ = 7;
		int[] passTypeIDs = { RAD, GO_ODM, DIV_PRAZ, PLA_OD, BOL, BOL_P3, PORODILJ, VOJSKA, IZOST, N_DETETA, BOL100, NEPLA_OD, PREK_RAD, PRERA, SLAVA, DA_KRV, SL_PUTI, SL_PUTZ };

        public const int SL_DAN = 20;   // uvedeno zbog starih slobodnih dana, ne ide u dbf samostalno, vec kao preraspodela (PRERA)
        public const int PRIV_IZL = 1001;
        public const int SLUZB_IZL = 1002;
        public const int OPR_IZOST = 1003;
        public const int BOL_BEZ_ISPL = 1004;
        public const int OPR_IZOST_PLA = 1005;
        public const int OSTALI_IZL = 1006;
        public const int CELODN_RAD = 1007;
        public const int SVE_PAUZE = 1008;

        // Table Definition for Crystal Reports
        DataSet dataSetCR = new DataSet();
        DataTable tableCR = new DataTable("employee_worklist");
        DataTable tableI = new DataTable("images");
        string[] passTypeIDsForCR = { "rad", "go_odm", "div_praz", "pla_od", "bol", "bol_p3", "porodilj", "vojska", "izost", "n_deteta", "bol100", "nepla_od", "prek_rad", "prera", "slava", "da_krv", "sl_puti", "sl_putz" };
        string oprIzost = "opr_izost";
        string bolBezIspl = "bol_bez_ispl";
        string oprIzostPla = "opr_izost_pla";
        string totalPla = "total_pla";
        string totalNepla = "total_nepla";
        string totalPlaMin = "total_pla_min";
        string totalNeplaMin = "total_nepla_min";
        string date = "date";
        string pause = "pause";
        string slIzl = "sl_izl";
        string privIzl = "priv_izl";
        string ostaliIzl = "ostali_izl";

        private bool createCR = false;
        int analyticEmployeeID = -1;
        string analyticRad = "";
        string analyticTotalPla = "";
        string analyticTotalNepla = "";
        bool absencesReport = false;

        int privatnoNum = 0;
        int sluzbenoNum = 0;
        int ostaloNum = 0;
        int neopravdanoNum = 0;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        int radniNum = 0;

        Filter filter;

		public PIOPaymentReports()
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
			
			DateTime date = DateTime.Now.Date;
			this.CenterToScreen();

			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;

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

            createDataSetForCR();
		}

        //used only when we want to create Crystal Report
        public PIOPaymentReports(int wuIndex, bool hierarhicly, DateTime fromDate, DateTime toDate,
            int employeeID, bool absencesReport)
        {
            InitializeComponent();

            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(WorkingUnitsReports).Assembly);
            logInUser = NotificationController.GetLogInUser();

            populateWorkigUnitCombo();
            if ((cbWorkingUnit.Items.Count > 0) && (wuIndex >= 0))
                cbWorkingUnit.SelectedIndex = wuIndex;

            chbHierarhicly.Checked = hierarhicly;

            dtpFromDate.Value = fromDate;
            dtpToDate.Value = toDate;

            this.analyticEmployeeID = employeeID;
            this.absencesReport = absencesReport;

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

            createCR = true;
            createDataSetForCR();
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.prbEmployee = new System.Windows.Forms.ProgressBar();
            this.lblCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(344, 91);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 2;
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 1;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(16, 143);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(485, 64);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Tag = "FILTERABLE";
            this.gbTimeInterval.Text = "Date Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(320, 24);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(280, 24);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(80, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(16, 295);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(397, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // prbEmployee
            // 
            this.prbEmployee.Location = new System.Drawing.Point(128, 239);
            this.prbEmployee.Name = "prbEmployee";
            this.prbEmployee.Size = new System.Drawing.Size(373, 16);
            this.prbEmployee.Step = 1;
            this.prbEmployee.TabIndex = 14;
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(24, 237);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(32, 16);
            this.lblCount.TabIndex = 15;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(64, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 16);
            this.label2.TabIndex = 16;
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(80, 239);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(32, 16);
            this.lblEmployee.TabIndex = 17;
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(365, 24);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 36;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click_1);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged_1);
            // 
            // PIOPaymentReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(513, 333);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.prbEmployee);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbWorkingUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PIOPaymentReports";
            this.ShowInTaskbar = false;
            this.Text = "PaymentReports";
            this.Load += new System.EventHandler(this.PIOPaymentReports_Load);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public void btnGenerateReport_Click(object sender, System.EventArgs e)
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

                if (cbWorkingUnit.Items.Count <= 1)
                {
                    MessageBox.Show(rm.GetString("noEmplReportPrivilege", culture));
                    return;
                }
			
				if (this.dtpFromDate.Value <= this.dtpToDate.Value)
				{
					if (cbWorkingUnit.SelectedIndex > 0)
					{
						wuID = (int) cbWorkingUnit.SelectedValue;

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
                        //get all WU for wheach user has right to see reports
                        wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        foreach (WorkingUnitTO wu in wuArray)
                        {
                            wUnits += wu.WorkingUnitID.ToString().Trim() + ",";
                        }
                        if (wUnits.Length > 0)
                        {
                            wUnits = wUnits.Substring(0, wUnits.Length - 1);
                        }

                        // find Employees for selected Working Units
                        employees = new Employee().SearchByWU(wUnits.Trim());
					}

					foreach (EmployeeTO empl in employees)
					{
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

					int employeeIDindex = -1;
					foreach(int employeeID in employeesID)
					{
						employeeIDindex++;

                        if ((createCR) && (analyticEmployeeID != -1) && (analyticEmployeeID != employeeID))
                            continue;

                        if ((createCR) && (absencesReport))
                        {
                            privatnoNum = 0;
                            sluzbenoNum = 0;
                            ostaloNum = 0;
                            neopravdanoNum = 0;
                            radniNum = 0;
                        }

						// add rows with employee ID, and hours for every payment code
						// generateData(employeeID, (ArrayList) emplPairs[employeeID], dtpFromDate.Value, dtpToDate.Value.AddDays(1));

						Hashtable paymentCodesHours = new Hashtable();
						for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
						{
							bool isRegularSchema = true;
                            Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
							if (edi == null) continue;
                            Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
							IDictionaryEnumerator ediEnum = edi.GetEnumerator();
							while(ediEnum.MoveNext())
							{
                                employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
							}

							List<IOPairTO> edp = GetEmployeeDayPairs(emplPairs[employeeID], isRegularSchema, day);
							List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
							foreach(IOPairTO ioPairTO in edp)
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
							while(dayPaymentCodesHoursEnum.MoveNext())
							{
								if(!paymentCodesHours.ContainsKey(dayPaymentCodesHoursEnum.Key))
								{
									paymentCodesHours.Add(dayPaymentCodesHoursEnum.Key,dayPaymentCodesHoursEnum.Value);
								}
								else
								{
									paymentCodesHours[dayPaymentCodesHoursEnum.Key] = 
										((TimeSpan)paymentCodesHours[dayPaymentCodesHoursEnum.Key]).Add((TimeSpan)dayPaymentCodesHoursEnum.Value);
								}
							}

                            if ((createCR) && (analyticEmployeeID != -1) && (analyticEmployeeID == employeeID))
                            {
                                FillAnalyticCRRowList(analyticEmployeeID, employees[employeeIDindex], day, dayPaymentCodesHours);
                            }

                            if ((createCR) && (absencesReport))
                            {
                                bool workDay = false;
                                if ((TimeSpan)dayPaymentCodesHours[PRIV_IZL] >= (new TimeSpan(0, 1, 0)))
                                    workDay = true;
                                if ((TimeSpan)dayPaymentCodesHours[SLUZB_IZL] >= (new TimeSpan(0, 1, 0)))
                                    workDay = true;
                                if ((TimeSpan)dayPaymentCodesHours[OSTALI_IZL] >= (new TimeSpan(0, 1, 0)))
                                    workDay = true;
                                if ((TimeSpan)dayPaymentCodesHours[IZOST] >= (new TimeSpan(0, 1, 0)))
                                {
                                    neopravdanoNum++;
                                    workDay = true;
                                }
                                if (((TimeSpan)dayPaymentCodesHours[RAD] >= (new TimeSpan(0, 1, 0)))
                                    || workDay)
                                    radniNum++;
                            }
						}
                        // ne generise se izvestaj ako nista nije racunato ni za jedan dan (nema zadatog radnog vremena)
                        if (paymentCodesHours.Count > 0)
                        {
                            // izracunaj opravdane placene izostanke kao zbir sluzbenih i privatnih izlazaka do 5h i ostalih izlazaka
                            //paymentCodesHours[OPR_IZOST_PLA] = (TimeSpan)paymentCodesHours[SLUZB_IZL] + (TimeSpan)paymentCodesHours[OSTALI_IZL];

                            // svi privatni izlasci idu u redovan rad i opravdane izostanke placene - 4.6.2008. -> Sanja 
                            paymentCodesHours[OPR_IZOST_PLA] = (TimeSpan)paymentCodesHours[SLUZB_IZL] + (TimeSpan)paymentCodesHours[PRIV_IZL] + (TimeSpan)paymentCodesHours[OSTALI_IZL];

                            paymentCodesHours[RAD] = (TimeSpan)paymentCodesHours[RAD] + (TimeSpan)paymentCodesHours[PRIV_IZL];

                            // privatni izlasci do 5h idu u redovan rad, ostatak u izostanke
                            /*if ((TimeSpan)paymentCodesHours[PRIV_IZL] <= new TimeSpan(5, 0, 0))
                            {
                                paymentCodesHours[RAD] = (TimeSpan)paymentCodesHours[RAD] + (TimeSpan)paymentCodesHours[PRIV_IZL];
                                paymentCodesHours[OPR_IZOST_PLA] = (TimeSpan)paymentCodesHours[OPR_IZOST_PLA] + (TimeSpan)paymentCodesHours[PRIV_IZL];
                            }
                            else
                            {
                                paymentCodesHours[RAD] = (TimeSpan)paymentCodesHours[RAD] + new TimeSpan(5, 0, 0);
                                paymentCodesHours[OPR_IZOST_PLA] = (TimeSpan)paymentCodesHours[OPR_IZOST_PLA] + new TimeSpan(5, 0, 0);
                            }*/

                            //privatni izlasci ne spadaju u neopravdane izostanke
                            //u novoj verziji nisu ni usli u zbir izostanaka
                            //paymentCodesHours[IZOST] = (TimeSpan)paymentCodesHours[IZOST] - (TimeSpan)paymentCodesHours[PRIV_IZL];

                            // izracunaj opravdane izostanke kao zbir sluzbenih i privatnih izlazaka i ostalih izlazaka
                            paymentCodesHours[OPR_IZOST] = (TimeSpan)paymentCodesHours[SLUZB_IZL] + (TimeSpan)paymentCodesHours[PRIV_IZL] + (TimeSpan)paymentCodesHours[OSTALI_IZL];

                            // generisu se izvestaj Radne liste (Crystal Report) ili .dbf file za racunanje plata
                            // stavi u izvestaj samo aktivne i blokirane zaposlene <--> Darko 4.12.2007.
                            if ((employees[employeeIDindex].Status == Constants.statusActive) || (employees[employeeIDindex].Status == Constants.statusBlocked))
                            {
                                if (paymentCodesHours.Count > 0)
                                {
                                    if (((createCR) && (analyticEmployeeID == -1) && (!absencesReport))
                                        || (!createCR))
                                        FillCRRowList(employeeID, employees[employeeIDindex], paymentCodesHours);
                                    if ((createCR) && (absencesReport))
                                        FillAbsencesCRRowList(employeeID, employees[employeeIDindex], paymentCodesHours);
                                    if (!createCR) FillReportRowList(employeeID, employees[employeeIDindex], paymentCodesHours);
                                }
                            }

                            if ((createCR) && (analyticEmployeeID != -1) && (analyticEmployeeID == employeeID))
                            {
                                calculateSummaryTotalsForAnalityc(employeeID, employees[employeeIDindex], paymentCodesHours);
                                break;
                            }
                        }

						empCount = empCount + 1;
						prbEmployee.Value++;
						this.lblCount.Text = empCount.ToString();
						this.lblCount.Refresh();
					}
				}
				else
				{
					MessageBox.Show (rm.GetString("wrongDatePickUp", culture));
					return;
				}
				
                if (createCR)
                {
                    generateCRReport("");
                }
                else
                {
                    this.label2.Text = "";
                    // report
                    //generatePDFReport();
                    //generateCSVReport();
                    generateTXTandDBFReport();
                    generateCRReport("*");
                    this.Close();
                }
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.btnGenerate_Click(): " + ex.Message + " empCount=" + empCount.ToString() + "\n" + ex.StackTrace);
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
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
			List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
			foreach(IOPairTO iopair in emplPairs)
			{
				if (iopair.IOPairDate == day)
				{
					employeeDayPairs.Add(iopair);
				}

				// pairs that belong to the tomorrow's part of the night shift (00:00-07:00)
				if (!isRegularSchema && (iopair.IOPairDate == day.AddDays(1) &&
					iopair.StartTime.TimeOfDay < new TimeSpan(7,0,0)))
				{
					employeeDayPairs.Add(iopair);
				}
			}
			return employeeDayPairs;
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

		/// <summary>
		/// gets employee's working intervals for the given day
		/// </summary>
		/// <param name="employeeTimeScheduleList"></param>
		/// <param name="day"></param>
		/// <returns></returns>
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

		bool isHoliday(DateTime day)
		{
			return (holidays.ContainsKey(day));
		}

		bool isWeekend(DateTime day)
		{
			return ((day.DayOfWeek == DayOfWeek.Saturday) || (day.DayOfWeek == DayOfWeek.Sunday));
		}

		bool isRegularTimeSchema(WorkTimeSchemaTO schema)
		{
			return (schema.Type != Constants.schemaTypeIndustrial);
		}

        bool is8hoursShift(Dictionary<int, WorkTimeIntervalTO> intervals)
		{
			TimeSpan ts = new TimeSpan(0,0,0);
			for (int i = 0; i < intervals.Count; i++)
			{
				ts = ts.Add(intervals[i].EndTime - intervals[i].StartTime);
			}
			return (!(ts > new TimeSpan(8,0,0)));
		}

        bool isWorkingDay(Dictionary<int, WorkTimeIntervalTO> intervals)
		{
			TimeSpan ts = new TimeSpan(0,0,0);
			for (int i = 0; i < intervals.Count; i++)
			{
				ts = ts.Add(intervals[i].EndTime - intervals[i].StartTime);
			}
			return (ts > new TimeSpan(0,0,0));
		}

        bool isNightShiftDay(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
		{
			// see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
			IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator(); 
			while(dayIntervalsEnum.MoveNext())
			{
                WorkTimeIntervalTO dayInterval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
				if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0,0,0)) ||
					(dayInterval.EndTime.TimeOfDay == new TimeSpan(23,59,0))) &&
					(dayInterval.EndTime > dayInterval.StartTime))
				{
					return true;
				}
			}
			return false;
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

				if (logInUser != null)
				{
					wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    //wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
				}

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.populateWorkigUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("PaymentReports", culture);

				gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhicly.Text = rm.GetString("hierarchically", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void generateTXTandDBFReport()
		{
			try
			{
                /*
				//write data to TXT file
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				string fileName = Constants.txtDocPath
					+ "PIO izvestaj za plate-" + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";

				FileStream stream = new FileStream(fileName, FileMode.Create);
				stream.Close();

				StreamWriter writer = File.AppendText(fileName);
				foreach (string row in rowList)
				{
					writer.WriteLine(row);
				}
				writer.Close();
				System.Diagnostics.Process.Start("notepad.exe",fileName);

				debug.writeBenchmarkLog("Posle notepad.exe");
                 */

				// insert records into .dbf file
				OleDbConnection myDBFConn = null;
				OleDbTransaction myTrans = null;
				try
				{
					debug.writeBenchmarkLog("Pre kreiranja konekcije");
					string strDBFConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Constants.txtDocPath + ";Extended Properties=dBASE IV;User ID=Admin;Password=";
					debug.writeBenchmarkLog("Connection string: " + strDBFConn);
					myDBFConn = new OleDbConnection(strDBFConn);
					debug.writeBenchmarkLog("Posle kreiranja konekcije");
					debug.writeBenchmarkLog("Pre otvaranja konekcije");
					myDBFConn.Open();
					debug.writeBenchmarkLog("Posle otvaranja konekcije");
					debug.writeBenchmarkLog("Pre pocetka transakcije");
					myTrans = myDBFConn.BeginTransaction();
					debug.writeBenchmarkLog("Posle pocetka transakcije");
					OleDbCommand myDBFCommand = new OleDbCommand("DELETE from finansi", myDBFConn, myTrans);
					debug.writeBenchmarkLog("Pre brisanja record-a");
					myDBFCommand.ExecuteNonQuery();
					debug.writeBenchmarkLog("Posle brisanja record-a");
					debug.writeBenchmarkLog("Pre foreach");
					foreach (string row in rowList)
					{
						myDBFCommand.CommandText = "INSERT INTO finansi " +
							"(SIF, RAD, GO_ODM, DIV_PRAZ, PLA_OD, BOL, BOL_P3, PORODILJ, VOJSKA, IZOST, N_DETETA, BOL100, NEPLA_OD, PREK_RAD, PRERA, SLAVA, DA_KRV, SL_PUTI, SL_PUTZ) " +
							"VALUES (" + row +")";
						debug.writeBenchmarkLog("Insert komanda: " + myDBFCommand.CommandText);
						myDBFCommand.ExecuteNonQuery();
					}
					debug.writeBenchmarkLog("Posle foreach");
					debug.writeBenchmarkLog("Pre Commit");
					myTrans.Commit();
					debug.writeBenchmarkLog("Posle Commit");
				}
				catch (Exception ex)
				{
					if (myTrans != null) myTrans.Rollback();
					debug.writeBenchmarkLog("Prvi catch: " + ex.Message);
				}
				finally
				{
					if (myDBFConn != null) myDBFConn.Close();
				}

                try
                {
                    string fileNameSource = Constants.txtDocPath + "finansi.dbf";
                    string fileNameDest = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\finansi.dbf";

                    File.Copy(fileNameSource, fileNameDest, true);
                }
                catch (Exception ex)
                {
                    debug.writeLog(DateTime.Now + " PIOPaymentReports.generateTXTandDBFReport(): Error copying finansi.dbf from source to desktop. " + ex.Message + "\n");
                }
			}
			catch (System.Threading.ThreadAbortException taex)
			{
				debug.writeBenchmarkLog("Drugi catch: " + taex.Message);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeeAnaliticReport.generateTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		void InitializePaymentCodesHours(Hashtable paymentCodesHours)
		{
            // ulazi za obracun plata
			for (int i = 0; i < passTypeIDs.Length; i++)
			{
				paymentCodesHours.Add(passTypeIDs[i],new TimeSpan(0,0,0));
			}
            // dodatni ulazi za izvestaj Radne liste
            paymentCodesHours.Add(PRIV_IZL,new TimeSpan(0,0,0));
            paymentCodesHours.Add(SLUZB_IZL, new TimeSpan(0, 0, 0));
            paymentCodesHours.Add(OPR_IZOST,new TimeSpan(0,0,0));
            paymentCodesHours.Add(BOL_BEZ_ISPL,new TimeSpan(0,0,0));
            paymentCodesHours.Add(OPR_IZOST_PLA, new TimeSpan(0, 0, 0));
            paymentCodesHours.Add(OSTALI_IZL, new TimeSpan(0, 0, 0));
            paymentCodesHours.Add(SVE_PAUZE, new TimeSpan(0, 0, 0));
		}

		/// <summary>
		/// calculate hours per each payment code for one regular time schema employee day
		/// </summary>
		/// <param name="employeeID"></param>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
		/// <param name="day"></param>
        private Hashtable CalculatePaymentPerRegularEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			Hashtable paymentCodesHours = new Hashtable();

			InitializePaymentCodesHours(paymentCodesHours);

            // sumiraj predvidjeni broj radnih sati za dan
            TimeSpan totalWorkingHours = CalculateTotalWorkingHours(dayIntervals);

			// izbaci parove koji se ne odnose na radne sate
			IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if (iopair.IsWrkHrsCount == 0)
				{
					ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
            // ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
            if (ioPairs.Count <= 0 && !isHoliday(day))
            {
                paymentCodesHours[IZOST] = (TimeSpan)paymentCodesHours[IZOST] + totalWorkingHours;
                return paymentCodesHours;
            }

			// sumiraj sve parove prekovremenog rada
            TimeSpan totalPrekRad = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.PassTypeID == PREK_RAD)
                {
                    totalPrekRad += (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                }
            }
            // pozovi metodu koja vraca prekovremeni rad u minutima za tog zaposlenog za taj dan iz tabele extra_hours_used i izracunati totalPrekRad
            int totalPrekRadMinutes = (new ExtraHourUsed()).SearchEmployeeUsedSumByType(employeeID, day, day, Constants.extraHoursUsedOvertime);
            totalPrekRad += new TimeSpan(0, totalPrekRadMinutes, 0);

			// dodaj broj prekovremenih sati na PREK_RAD
			paymentCodesHours[PREK_RAD] = (TimeSpan)paymentCodesHours[PREK_RAD] + totalPrekRad;

			// izbaci parove prekovremenog rada
			ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if (iopair.PassTypeID == PREK_RAD)
				{
					ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
            if (ioPairs.Count <= 0 && !isHoliday(day)) 
                return paymentCodesHours;

            // ako je broj predvidjenih radnih sati za dan 0, zavrsi obradu
            if (totalWorkingHours == new TimeSpan(0, 0, 0)) return paymentCodesHours;

			// ako je praznik dodaj predvidjen broj radnih sati na DIV_PRAZ (drzavni i verski praznik) i zavrsi obradu
			if (isHoliday(day))
			{
				paymentCodesHours[DIV_PRAZ] = (TimeSpan)paymentCodesHours[DIV_PRAZ] + totalWorkingHours;
				return paymentCodesHours;
			}

			// ako je celodnevno odsustvo dodaj predvidjen broj radnih sati na odgovarajuci PassTypeID i zavrsi obradu
			foreach(IOPairTO iopair in ioPairs)
			{
				PassTypeTO passType = passTypes[iopair.PassTypeID];
                if (passType.IsPass == Constants.wholeDayAbsence)
                {
                    if (paymentCodesHours.ContainsKey(iopair.PassTypeID))
                    {
                        paymentCodesHours[iopair.PassTypeID] = (TimeSpan)paymentCodesHours[iopair.PassTypeID] + totalWorkingHours;
                        return paymentCodesHours;
                    }
                    else
                    {
                        if (iopair.PassTypeID == SL_DAN)    // ako je stari slobodan dan, ulazi kao preraspodela
                        {
                            paymentCodesHours[PRERA] = (TimeSpan)paymentCodesHours[PRERA] + totalWorkingHours;
                            return paymentCodesHours;
                        }
                        else if (iopair.PassTypeID == CELODN_RAD)    // ako je celodnevno odsustvo redovan rad, ulazi kao redovan rad
                        {
                            paymentCodesHours[RAD] = (TimeSpan)paymentCodesHours[RAD] + totalWorkingHours;
                            return paymentCodesHours;
                        }
                        else
                        {
                            MessageBox.Show("Tip prolaska " + iopair.PassTypeID.ToString() + " ne moze biti obradjen! Proverite podatke za zaposlenog " + employeeID.ToString() +
                                            " za dan " + day.ToShortDateString(), "ACTA Info");
                            return paymentCodesHours;
                        }
                    }
                }
			}

			// izbaci nepotrebne parove i podesi dolazak i odlazak uzimajuci u obzir pravila dolaska i odlaska
			CleanUpDayPairs(ioPairs, dayIntervals, day);
			
			// ako nema validnih parova, postavi kasnjenje na predvidjeni broj radnih sati i zavrsi obradu
			if (ioPairs.Count <= 0) 
			{
				paymentCodesHours[IZOST] = (TimeSpan)paymentCodesHours[IZOST] + totalWorkingHours;
				return paymentCodesHours;
			}

            // sumiraj trajanje svih preostalih parova (oni su svi sa pass type 1 - prolasci na citacu, i nema privatnih izlazaka jer su ranije sumirani i izbaceni)
            TimeSpan totalDuration = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
            }

            // izracunaj kasnjenje kao razliku broja predvidjenih sati i stvarnog trajanja svih parova
            TimeSpan totalLatency = totalWorkingHours - totalDuration;
            paymentCodesHours[IZOST] = (TimeSpan)paymentCodesHours[IZOST] + totalLatency;

            // sumiraj sve parove privatnih izlazaka
            TimeSpan totalPrivIzl = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.PassTypeID == Constants.privateOut)
                {
                    totalPrivIzl += (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    if ((createCR) && (absencesReport)
                            && ((iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay) >= (new TimeSpan(0, 1, 0))))
                        privatnoNum++;
                }
            }
            // dodaj broj sati privatnih izlazaka na PRIV_IZL
            paymentCodesHours[PRIV_IZL] = (TimeSpan)paymentCodesHours[PRIV_IZL] + totalPrivIzl;

            // izbaci parove privatnih izlazaka
            ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if (iopair.PassTypeID == Constants.privateOut)
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return paymentCodesHours;

			// sumiraj trajanja parova svaki za svoj tip, osim za redovan rad u koji ulaze i sluzbeni i privatni izlasci i pauze
			TimeSpan totalRegularWork = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				if (!paymentCodesHours.ContainsKey(iopair.PassTypeID) &&
					!((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                      (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut)))
					continue;

				if ((iopair.PassTypeID == Constants.regularWork) || (iopair.PassTypeID == Constants.officialOut) ||
                    (iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause) || (iopair.PassTypeID == Constants.otherOut))
				{
					paymentCodesHours[RAD] = (TimeSpan)paymentCodesHours[RAD] + (iopair.EndTime.TimeOfDay-iopair.StartTime.TimeOfDay);
                    // sumiraj sluzbene izlaske za potrebe izvestaja Radne liste
                    if (iopair.PassTypeID == Constants.officialOut)
                    {
                        paymentCodesHours[SLUZB_IZL] = (TimeSpan)paymentCodesHours[SLUZB_IZL] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                        if ((createCR) && (absencesReport)
                            && ((iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay) >= (new TimeSpan(0, 1, 0))))
                            sluzbenoNum++;
                    }
                    // sumiraj ostale izlaske za potrebe izvestaja Radne liste
                    else if (iopair.PassTypeID == Constants.otherOut)
                    {
                        paymentCodesHours[OSTALI_IZL] = (TimeSpan)paymentCodesHours[OSTALI_IZL] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                        if ((createCR) && (absencesReport)
                            && ((iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay) >= (new TimeSpan(0, 1, 0))))
                            ostaloNum++;
                    }
                    // sumiraj sve pauze za potrebe izvestaja Radne liste
                    else if ((iopair.PassTypeID == Constants.automaticPause) || (iopair.PassTypeID == Constants.pause))
                    {
                        paymentCodesHours[SVE_PAUZE] = (TimeSpan)paymentCodesHours[SVE_PAUZE] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    }
				}
				else
				{
                    if (paymentCodesHours.ContainsKey(iopair.PassTypeID))
                    {
                        paymentCodesHours[iopair.PassTypeID] = (TimeSpan)paymentCodesHours[iopair.PassTypeID] + (iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                    }
                    else
                    {
                        MessageBox.Show("Tip prolaska " + iopair.PassTypeID.ToString() + " ne moze biti obradjen! Proverite podatke za zaposlenog " + employeeID.ToString() +
                                        " za dan " + day.ToShortDateString(), "ACTA Info");
                        return paymentCodesHours;
                    }
				}
			}		

			return paymentCodesHours;
		}

        private Hashtable CalculatePaymentPerBrigadeEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			Hashtable paymentCodesHours = new Hashtable();

			// remove unneeded pairs and trim first and last pair to the working time interval boundaries
			// considering latency rules, differently for regular and night shift days
			if(!isNightShiftDay(dayIntervals))
			{
				CleanUpDayPairs(ioPairs, dayIntervals, day);
			}
			else
			{
				CleanUpNightPairs(employeeID, ioPairs, dayIntervals, day);
			}

			// if not working day do nothing
			if (!isWorkingDay(dayIntervals)) return paymentCodesHours;

			TimeSpan expectedHours = is8hoursShift(dayIntervals) ? new TimeSpan(8,0,0) : new TimeSpan(12,0,0);
			TimeSpan expectedBreak = (expectedHours == new TimeSpan(8,0,0)) ? new TimeSpan(0,30,0) : new TimeSpan(0,45,0);
			TimeSpan expectedMealTickets = (expectedHours == new TimeSpan(8,0,0)) ? new TimeSpan(1,0,0) : new TimeSpan(1,30,0);

			// if no valid pairs set latency for the whole day (payment code 4010) and close the day
			if (ioPairs.Count <= 0)
			{
				paymentCodesHours.Add(passTypes[Constants.late].PaymentCode,expectedHours.Subtract(expectedBreak));
				return paymentCodesHours;
			}

			// if whole day absences found add 7.5 hours or 11.25 h to its payment code (various) and close the day
			foreach(IOPairTO iopair in ioPairs)
			{
				PassTypeTO passType = passTypes[iopair.PassTypeID];
				if (passType.IsPass == Constants.wholeDayAbsence)
				{
					paymentCodesHours.Add(passType.PaymentCode,expectedHours.Subtract(expectedBreak));
					return paymentCodesHours;
				}
			}

			// sum duration of all pairs (they're all with pass type 1 - prolasci na citacu)
			TimeSpan totalDuration = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay-iopair.StartTime.TimeOfDay);
				if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalDuration = totalDuration.Add(new TimeSpan(0,1,0));			}

			// calculate latency as difference between expected hours and actual duration (already
			// calculated considering start and end latencies, round it up to full hour and set
			// latency (payment code 4010)
			TimeSpan totalLatency = expectedHours - totalDuration;
			if (totalLatency.Minutes != 0)
			{
				totalLatency = totalLatency.Add(new TimeSpan(1,-totalLatency.Minutes,-totalLatency.Seconds));
			}

			// calculate sum of ordinary private exits (4000 - bez preraspodele) and round it up to full hour
			TimeSpan totalPrivateExits = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				if (iopair.PassTypeID == Constants.privateOut)
				{
					totalPrivateExits = totalPrivateExits.Add(iopair.EndTime.TimeOfDay-iopair.StartTime.TimeOfDay);
					if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalPrivateExits = totalPrivateExits.Add(new TimeSpan(0,1,0));
				}
			}		
			if (totalPrivateExits.Minutes != 0)
			{
				totalPrivateExits = totalPrivateExits.Add(new TimeSpan(1,-totalPrivateExits.Minutes,-totalPrivateExits.Seconds));
			}

			// calculate sum of night hours (0560 - the ones after 22:00), round it down it to full hour
			TimeSpan totalNightHours = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				if ((iopair.PassTypeID != Constants.privateOut) && (iopair.EndTime.TimeOfDay > new TimeSpan(22,0,0)))
				{
					TimeSpan startNightWork = new TimeSpan(22,0,0);
					if (iopair.StartTime.TimeOfDay > startNightWork) startNightWork = iopair.StartTime.TimeOfDay;
					totalNightHours = totalNightHours.Add(iopair.EndTime.TimeOfDay-startNightWork);
				}
				if ((iopair.PassTypeID != Constants.privateOut) && (iopair.StartTime.TimeOfDay < new TimeSpan(6,0,0)))
				{
					TimeSpan endNightWork = new TimeSpan(6,0,0);
					if (iopair.EndTime.TimeOfDay < endNightWork) endNightWork = iopair.EndTime.TimeOfDay;
					totalNightHours = totalNightHours.Add(endNightWork - iopair.StartTime.TimeOfDay);
				}
				if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalNightHours = totalNightHours.Add(new TimeSpan(0,1,0));
			}		
			if (totalNightHours.Minutes != 0)
			{
				totalNightHours = totalNightHours.Add(new TimeSpan(0,-totalNightHours.Minutes,-totalNightHours.Seconds));
			}

			// calculate payed hours (payment code 0031) considering break time, and meal tickets (payment code 3000)
			TimeSpan totalPayedHours = expectedHours - totalLatency - totalPrivateExits;

			if (totalPayedHours > new TimeSpan(expectedHours.Hours/2,0,0))	// add a meal ticket and subtract break time from working hours
			{
				paymentCodesHours.Add(passTypes[Constants.tickets].PaymentCode,expectedMealTickets);
				totalPayedHours = totalPayedHours.Subtract(expectedBreak);
				if (totalNightHours > totalPayedHours-totalNightHours) totalNightHours = totalNightHours.Subtract(expectedBreak);
			}
			else	// subtract break time from greater of total latency time and total privat exits time
			{
				if ((totalLatency > totalPrivateExits) && (totalLatency >= expectedBreak))
				{
					totalLatency = totalLatency.Subtract(expectedBreak);
				}
				else if (totalPrivateExits >= expectedBreak)
				{
					totalPrivateExits = totalPrivateExits.Subtract(expectedBreak);
				}
			}

			// set value for the payment code 0031 and 0560 (regular work and work on holiday)
			if (totalPayedHours.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.regularWork].PaymentCode,totalPayedHours);
				if (isHoliday(day))
				{
					paymentCodesHours.Add(passTypes[Constants.workOnHoliday].PaymentCode,totalPayedHours);
				}
			}

			// set value for the payment code 4010 (latency)
			if (totalLatency.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.late].PaymentCode,totalLatency);
			}

			// set value for the payment code 0400 (night work)
			if (totalNightHours.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.nightWork].PaymentCode,totalNightHours);
			}

			// set value for the payment code 4000 (private exits)
			if (totalPrivateExits.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.privateOut].PaymentCode,totalPrivateExits);
			}

			return paymentCodesHours;
		}

		/// <summary>
		/// removes unneeded pairs and trim first and last pair to the working time interval boundaries
		/// considering latency rules
		/// </summary>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
        //void CleanUpDayPairs(ArrayList ioPairs, Hashtable dayIntervals, DateTime day)
        //{
        //    // remove invalid pairs and pairs outside of the interval
        //    IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
        //    while(ioPairsEnum.MoveNext())
        //    {
        //        IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
        //        if((iopair.IOPairDate != day) || (iopair.StartTime > iopair.EndTime) || 
        //            (iopair.StartTime.TimeOfDay > ((TimeSchemaInterval) dayIntervals[0]).EndTime.TimeOfDay) ||
        //            (iopair.EndTime.TimeOfDay   < ((TimeSchemaInterval) dayIntervals[0]).StartTime.TimeOfDay))
        //        {
        //            ioPairs.Remove(ioPairsEnum.Current);
        //            ioPairsEnum = ioPairs.GetEnumerator();
        //        }
        //    }
        //    if (ioPairs.Count <= 0) return;

        //    // find first and last pair
        //    IOPairTO firstPair = (IOPairTO)ioPairs[0];
        //    IOPairTO lastPair = firstPair;
        //    foreach(IOPairTO iopair in ioPairs)
        //    {
        //        if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
        //        if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
        //    }
			
        //    TimeSpan totalWorkingHours = CalculateTotalWorkingHours(dayIntervals);

        //    // set start time
        //    if (firstPair.StartTime.TimeOfDay < ((TimeSchemaInterval) dayIntervals[0]).EarliestArrived.TimeOfDay)
        //    {
        //        firstPair.StartTime = firstPair.StartTime.AddHours(((TimeSchemaInterval) dayIntervals[0]).EarliestArrived.Hour - firstPair.StartTime.Hour);
        //        firstPair.StartTime = firstPair.StartTime.AddMinutes(((TimeSchemaInterval) dayIntervals[0]).EarliestArrived.Minute - firstPair.StartTime.Minute);
        //        firstPair.StartTime = firstPair.StartTime.AddSeconds(((TimeSchemaInterval) dayIntervals[0]).EarliestArrived.Second - firstPair.StartTime.Second);
        //    }
        //    if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

        //    // find expected end time
        //    DateTime expectedEndTime = new DateTime(firstPair.StartTime.Ticks);
        //    if (((TimeSchemaInterval) dayIntervals[dayIntervals.Count-1]).LatestLeft.TimeOfDay <= (firstPair.StartTime.TimeOfDay + totalWorkingHours))
        //    {
        //        expectedEndTime = expectedEndTime.AddHours(((TimeSchemaInterval) dayIntervals[dayIntervals.Count-1]).LatestLeft.Hour - expectedEndTime.Hour);
        //        expectedEndTime = expectedEndTime.AddMinutes(((TimeSchemaInterval) dayIntervals[dayIntervals.Count-1]).LatestLeft.Minute - expectedEndTime.Minute);
        //        expectedEndTime = expectedEndTime.AddSeconds(((TimeSchemaInterval) dayIntervals[dayIntervals.Count-1]).LatestLeft.Second - expectedEndTime.Second);
        //    }
        //    else
        //    {
        //        expectedEndTime = (firstPair.StartTime.Add(totalWorkingHours));
        //    }
																																  
        //    // set end time																											  
        //    if (lastPair.EndTime.TimeOfDay > expectedEndTime.TimeOfDay)
        //    {
        //        lastPair.EndTime = expectedEndTime;
        //    }
        //    if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        //}

        void CleanUpDayPairs(List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            // remove invalid pairs and pairs outside of the interval
            IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if ((iopair.IOPairDate != day) || (iopair.StartTime > iopair.EndTime) ||
                    //(iopair.StartTime.TimeOfDay > ((TimeSchemaInterval) dayIntervals[0]).EndTime.TimeOfDay) ||
                    //(iopair.EndTime.TimeOfDay   < ((TimeSchemaInterval) dayIntervals[0]).StartTime.TimeOfDay))
                    (iopair.StartTime.TimeOfDay > dayIntervals[0].LatestLeft.TimeOfDay) ||
                    (iopair.EndTime.TimeOfDay < dayIntervals[0].EarliestArrived.TimeOfDay))
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return;

            // find first and last pair
            IOPairTO firstPair = (IOPairTO)ioPairs[0];
            IOPairTO lastPair = firstPair;
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
                if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
            }

            TimeSpan totalWorkingHours = CalculateTotalWorkingHours(dayIntervals);

            // set start time
            if (firstPair.StartTime.TimeOfDay < dayIntervals[0].EarliestArrived.TimeOfDay)
            {
                firstPair.StartTime = firstPair.StartTime.AddHours(dayIntervals[0].EarliestArrived.Hour - firstPair.StartTime.Hour);
                firstPair.StartTime = firstPair.StartTime.AddMinutes(dayIntervals[0].EarliestArrived.Minute - firstPair.StartTime.Minute);
                firstPair.StartTime = firstPair.StartTime.AddSeconds(dayIntervals[0].EarliestArrived.Second - firstPair.StartTime.Second);
            }
            if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

            // find expected end time
            DateTime expectedEndTime = new DateTime(firstPair.StartTime.Ticks);
            if (dayIntervals[dayIntervals.Count - 1].LatestLeft.TimeOfDay <= (firstPair.StartTime.TimeOfDay + totalWorkingHours))
            {
                expectedEndTime = expectedEndTime.AddHours(dayIntervals[dayIntervals.Count - 1].LatestLeft.Hour - expectedEndTime.Hour);
                expectedEndTime = expectedEndTime.AddMinutes(dayIntervals[dayIntervals.Count - 1].LatestLeft.Minute - expectedEndTime.Minute);
                expectedEndTime = expectedEndTime.AddSeconds(dayIntervals[dayIntervals.Count - 1].LatestLeft.Second - expectedEndTime.Second);
            }
            else
            {
                expectedEndTime = (firstPair.StartTime.Add(totalWorkingHours));
            }

            // remove pairs completely behind the expected end time
            ioPairsEnum = ioPairs.GetEnumerator();
            while (ioPairsEnum.MoveNext())
            {
                IOPairTO iopair = (IOPairTO)ioPairsEnum.Current;
                if (iopair.StartTime.TimeOfDay > expectedEndTime.TimeOfDay)
                {
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
                    ioPairsEnum = ioPairs.GetEnumerator();
                }
            }
            if (ioPairs.Count <= 0) return;

            // find last pair again
            lastPair = ioPairs[0];
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
            }

            // set end time																											  
            if (lastPair.EndTime.TimeOfDay > expectedEndTime.TimeOfDay)
            {
                lastPair.EndTime = expectedEndTime;
            }

            if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
        }

		/// <summary>
		/// removes unneeded pairs belonging to the night shift and trim first and last pair
		/// to the working time interval boundaries considering latency rules
		/// </summary>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
        void CleanUpNightPairs(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			// night shift day can have 1 or 2 intervals
			if (dayIntervals.Count == 1)
			{
				// if night shift day has only 00:00- interval it's already calculated as yesterday
				// and should be dropped
				if (dayIntervals[0].StartTime.TimeOfDay == new TimeSpan(0,0,0))
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
					if (tomorrowIntervals != null)	dayIntervals[0] = tomorrowIntervals[0];
					else
					{
						debug.writeLog(DateTime.Now + " PaymentReports.CleanUpNightPairs(): Tomorrow intervals cannot be found! Employee: " + employeeID.ToString() + " Day: " + day.ToShortDateString() + "\n");
						return;
					}
				}
			}

			// remove invalid pairs and pairs outside of the interval
			IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if (
					(iopair.StartTime > iopair.EndTime) ||
					(
					(iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay)   &&
					(iopair.StartTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay) &&
					(iopair.EndTime.TimeOfDay   > dayIntervals[0].EndTime.TimeOfDay)   &&
					(iopair.EndTime.TimeOfDay   < dayIntervals[1].StartTime.TimeOfDay)
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
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if((iopair.IOPairDate == day) && (iopair.StartTime.TimeOfDay < dayIntervals[0].EndTime.TimeOfDay))
				{
					ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
			if (ioPairs.Count <= 0) return;

			// find first and last pair
			IOPairTO firstPair = (IOPairTO)ioPairs[0];
			IOPairTO lastPair = firstPair;
			foreach(IOPairTO iopair in ioPairs)
			{
				if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
				if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
			}

			// find the proper interval for the first and last pair considering the day
			int firstPairInterval = (firstPair.IOPairDate == day) ? 1 : 0;
			int lastPairInterval  = (lastPair.IOPairDate  == day) ? 1 : 0;

			// round up first pair start time to full hour, considering start tolerance
			if (firstPair.StartTime.TimeOfDay <= dayIntervals[firstPairInterval].LatestArrivaed.TimeOfDay)
			{
				firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[firstPairInterval].StartTime.TimeOfDay);
			}
			else
			{
				if (firstPair.StartTime.Minute != 0)
				{
					firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1,-firstPair.StartTime.Minute,-firstPair.StartTime.Second));
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
					lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0,-lastPair.EndTime.Minute,-lastPair.EndTime.Second));
				}
			}
			if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
		}

        TimeSpan CalculateTotalWorkingHours(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
		{
			TimeSpan totalWorkingHours = new TimeSpan(0,0,0);
			IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator();
			while(dayIntervalsEnum.MoveNext())
			{
                WorkTimeIntervalTO interval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
				totalWorkingHours += (interval.EndTime.TimeOfDay-interval.StartTime.TimeOfDay);
			}
			return totalWorkingHours;
		}

		void FillReportRowList(int employeeID, EmployeeTO emplData, Hashtable payTotal)
		{
			string row = "'" + employeeID.ToString() + "'";
			for (int i = 0; i < passTypeIDs.Length; i++)
			{
				TimeSpan ts = (TimeSpan) payTotal[passTypeIDs[i]];
				float tsFloat = (float)ts.TotalHours;
				row += "," + tsFloat.ToString("F2").Replace(",",".");
			}
			rowList.Add(row);
		}

        void createDataSetForCR()
        {
            tableCR.Columns.Add("employee_id", typeof(int));
            tableCR.Columns.Add("first_name", typeof(System.String));
            tableCR.Columns.Add("last_name", typeof(System.String));
            tableCR.Columns.Add("working_unit", typeof(System.String));
            tableCR.Columns.Add("rad", typeof(System.String));
            tableCR.Columns.Add("go_odm", typeof(System.String));
            tableCR.Columns.Add("div_praz", typeof(System.String));
            tableCR.Columns.Add("pla_od", typeof(System.String));
            tableCR.Columns.Add("bol", typeof(System.String));
            tableCR.Columns.Add("bol_p3", typeof(System.String));
            tableCR.Columns.Add("porodilj", typeof(System.String));
            tableCR.Columns.Add("vojska", typeof(System.String));
            tableCR.Columns.Add("izost", typeof(System.String));
            tableCR.Columns.Add("n_deteta", typeof(System.String));
            tableCR.Columns.Add("bol100", typeof(System.String));
            tableCR.Columns.Add("nepla_od", typeof(System.String));
            tableCR.Columns.Add("prek_rad", typeof(System.String));
            tableCR.Columns.Add("prera", typeof(System.String));
            tableCR.Columns.Add("slava", typeof(System.String));
            tableCR.Columns.Add("da_krv", typeof(System.String));
            tableCR.Columns.Add("sl_puti", typeof(System.String));
            tableCR.Columns.Add("sl_putz", typeof(System.String));
            tableCR.Columns.Add("opr_izost", typeof(System.String));
            tableCR.Columns.Add("bol_bez_ispl", typeof(System.String));
            tableCR.Columns.Add("opr_izost_pla", typeof(System.String));
            tableCR.Columns.Add("total_pla", typeof(System.String));
            tableCR.Columns.Add("total_nepla", typeof(System.String));
            tableCR.Columns.Add("total_pla_min", typeof(int));
            tableCR.Columns.Add("total_nepla_min", typeof(int));

            if ((analyticEmployeeID != -1) || (absencesReport))
            {
                tableCR.Columns.Add("date", typeof(System.DateTime));
                tableCR.Columns.Add("pause", typeof(System.String));
                tableCR.Columns.Add("sl_izl", typeof(System.String));
                tableCR.Columns.Add("priv_izl", typeof(System.String));
                tableCR.Columns.Add("ostali_izl", typeof(System.String));

                tableCR.Columns.Add("rad_min", typeof(int));
                tableCR.Columns.Add("go_odm_min", typeof(int));
                tableCR.Columns.Add("div_praz_min", typeof(int));
                tableCR.Columns.Add("pla_od_min", typeof(int));
                tableCR.Columns.Add("bol_min", typeof(int));
                tableCR.Columns.Add("bol_p3_min", typeof(int));
                tableCR.Columns.Add("porodilj_min", typeof(int));
                tableCR.Columns.Add("vojska_min", typeof(int));
                tableCR.Columns.Add("izost_min", typeof(int));
                tableCR.Columns.Add("n_deteta_min", typeof(int));
                tableCR.Columns.Add("bol100_min", typeof(int));
                tableCR.Columns.Add("nepla_od_min", typeof(int));
                tableCR.Columns.Add("prek_rad_min", typeof(int));
                tableCR.Columns.Add("prera_min", typeof(int));
                tableCR.Columns.Add("slava_min", typeof(int));
                tableCR.Columns.Add("da_krv_min", typeof(int));
                tableCR.Columns.Add("sl_puti_min", typeof(int));
                tableCR.Columns.Add("sl_putz_min", typeof(int));
                tableCR.Columns.Add("opr_izost_min", typeof(int));
                tableCR.Columns.Add("bol_bez_ispl_min", typeof(int));
                tableCR.Columns.Add("opr_izost_pla_min", typeof(int));
                tableCR.Columns.Add("pause_min", typeof(int));
                tableCR.Columns.Add("sl_izl_min", typeof(int));
                tableCR.Columns.Add("priv_izl_min", typeof(int));
                tableCR.Columns.Add("ostali_izl_min", typeof(int));
                tableCR.Columns.Add("priv_izl_NUM", typeof(int));
                tableCR.Columns.Add("sl_izl_NUM", typeof(int));
                tableCR.Columns.Add("ostali_izl_NUM", typeof(int));
                tableCR.Columns.Add("izost_NUM", typeof(int));
                tableCR.Columns.Add("rad_NUM", typeof(int));
            }
            else
            {
            }

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
        }

        void FillCRRowList(int employeeID, EmployeeTO emplData, Hashtable payTotal)
		{
            //in payTotal:
            //RAD = rad (ovde su i pauze i celodnevni rad) + (PRIV_IZL < 5h + SLUZB_IZL + OSTALI_IZL) = rad + OPR_IZOST_PLA
            //PRIV_IZL = all priv_izl
            //SLUZB_IZL = all sluzb_izl
            //IZOST = all neopravdani_izl
            //OPR_IZOST = all opravdani_izl (PRIV_IZL + SLUZB_IZL + OSTALI_IZL)
            //BOL_BEZ_ISPL = bol_bez_ispl
            //OPR_IZOST_PLA = all payed opravdani_izl (PRIV_IZL < 5h + SLUZB_IZL + OSTALI_IZL)

            //for display
            //total placeno - RAD + sum(everything else that is not in total neplaceno, and without OPR_IZOST and OPR_IZOST_PLA)
            //total neplaceno - IZOST + BOL_BEZ_ISPL + NEPLA_OD + OPR_IZOST - OPR_IZOST_PLA
            //RAD - return value to rad, but after calculating total placeno

            TimeSpan ts = new TimeSpan();

            DataRow row = tableCR.NewRow();

            row["employee_id"] = emplData.EmployeeID;
            row["last_name"] = emplData.LastName;
            row["first_name"] = emplData.FirstName;
            row["working_unit"] = emplData.WorkingUnitName;
           
            TimeSpan emplTotalPayed = new TimeSpan();
            TimeSpan emplTotalNPayed = new TimeSpan();
			for (int i = 0; i < passTypeIDs.Length; i++)
			{
                ts = (TimeSpan)payTotal[passTypeIDs[i]];
                if ((passTypeIDs[i] != IZOST) && (passTypeIDs[i] != NEPLA_OD))
                {
                    emplTotalPayed += ts;
                }
                if ((passTypeIDs[i] == IZOST) || (passTypeIDs[i] == NEPLA_OD))
                {
                    emplTotalNPayed += ts;
                }
                if (passTypeIDs[i] == RAD)
                {
                    TimeSpan tsTemp = new TimeSpan();
                    tsTemp = (TimeSpan)payTotal[OPR_IZOST_PLA];
                    ts -= tsTemp;
                }

                // Sanja 10.7.2008. izmena: umesto neopravdanih treba da se vide ostali izlasci
                // logika racunanja nije menjana, samo su izvrsene vizuelne izmene
                if (passTypeIDs[i] == IZOST)
                {
                    row[passTypeIDsForCR[i]] = createTimeString((TimeSpan)payTotal[OSTALI_IZL]);
                }
                else
                {
                    //row[passTypeIDsForCR[i]] = (ts.Hours + (ts.Days * 24)).ToString() + ":" + getMinutesString(ts.Minutes);
                    row[passTypeIDsForCR[i]] = createTimeString(ts);
                }
			}

            ts = (TimeSpan)payTotal[OPR_IZOST];
            emplTotalNPayed += ts;
            //row[oprIzost] = (ts.Hours + (ts.Days * 24)).ToString() + ":" + getMinutesString(ts.Minutes);
            
            // Sanja 10.7.2008. izmena: umesto opravdanih treba da se vide privatni izlasci
            // logika racunanja nije menjana, samo su izvrsene vizuelne izmene
            //row[oprIzost] = createTimeString(ts);
            row[oprIzost] = createTimeString((TimeSpan)payTotal[PRIV_IZL]);

            ts = (TimeSpan)payTotal[BOL_BEZ_ISPL];
            emplTotalNPayed += ts;
            //row[bolBezIspl] = (ts.Hours + (ts.Days * 24)).ToString() + ":" + getMinutesString(ts.Minutes);
            row[bolBezIspl] = createTimeString(ts);

            ts = (TimeSpan)payTotal[OPR_IZOST_PLA];
            emplTotalNPayed -= ts;
            //row[oprIzostPla] = (ts.Hours + (ts.Days * 24)).ToString() + ":" + getMinutesString(ts.Minutes);

            // Sanja 10.7.2008. izmena: umesto opravdanih placenih treba da se vide sluzbeni izlasci
            // logika racunanja nije menjana, samo su izvrsene vizuelne izmene
            //row[oprIzostPla] = createTimeString(ts);
            row[oprIzostPla] = createTimeString((TimeSpan)payTotal[SLUZB_IZL]);

            //zaokruzivanje
            TimeSpan oneMin = new TimeSpan(0, 1, 0);
            if (emplTotalPayed.Seconds > 29)
                emplTotalPayed = emplTotalPayed.Add(oneMin);
            if (emplTotalNPayed.Seconds > 29)
                emplTotalNPayed = emplTotalNPayed.Add(oneMin);

            //row[totalPla] = (emplTotalPayed.Hours + (emplTotalPayed.Days * 24)).ToString() + ":" + getMinutesString(emplTotalPayed.Minutes);
            row[totalPla] = createTimeString(emplTotalPayed);
            //row[totalNepla] = (emplTotalNPayed.Hours + (emplTotalNPayed.Days * 24)).ToString() + ":" + getMinutesString(emplTotalNPayed.Minutes);
            row[totalNepla] = createTimeString(emplTotalNPayed);

            row[totalPlaMin] = (emplTotalPayed.Hours + (emplTotalPayed.Days * 24)) * 60 + emplTotalPayed.Minutes;
            row[totalNeplaMin] = (emplTotalNPayed.Hours + (emplTotalNPayed.Days * 24)) * 60 + emplTotalNPayed.Minutes;

            row["imageID"] = 1;

            tableCR.Rows.Add(row);
            tableCR.AcceptChanges();
		}

        void FillAnalyticCRRowList(int employeeID, EmployeeTO emplData, DateTime day, Hashtable payTotal)
        {
            // izracunaj opravdane placene izostanke kao zbir sluzbenih i privatnih izlazaka do 5h i ostalih izlazaka
            payTotal[OPR_IZOST_PLA] = (TimeSpan)payTotal[SLUZB_IZL] + (TimeSpan)payTotal[OSTALI_IZL];

            //privatni izlasci ne spadaju u neopravdane izostanke
            //u novoj verziji nisu ni usli u zbir izostanaka
            //payTotal[IZOST] = (TimeSpan)payTotal[IZOST] - (TimeSpan)payTotal[PRIV_IZL];

            // izracunaj opravdane izostanke kao zbir sluzbenih i privatnih izlazaka i ostalih izlazaka
            payTotal[OPR_IZOST] = (TimeSpan)payTotal[SLUZB_IZL] + (TimeSpan)payTotal[PRIV_IZL] + (TimeSpan)payTotal[OSTALI_IZL];

            //in payTotal:
            //RAD = rad (ovde su i pauze i celodnevni rad) + (SLUZB_IZL + OSTALI_IZL) = rad + OPR_IZOST_PLA
            //PRIV_IZL = all priv_izl
            //SLUZB_IZL = all sluzb_izl
            //IZOST = all neopravdani_izl
            //OPR_IZOST = all opravdani_izl (PRIV_IZL + SLUZB_IZL + OSTALI_IZL)
            //BOL_BEZ_ISPL = bol_bez_ispl
            //OPR_IZOST_PLA = all payed opravdani_izl (SLUZB_IZL + OSTALI_IZL)

            //for display
            //total placeno - RAD + sum(everything else that is not in total neplaceno, and without OPR_IZOST and OPR_IZOST_PLA)
            //total neplaceno - IZOST + BOL_BEZ_ISPL + NEPLA_OD
            //RAD - return value to rad, but before calculating total placeno (OPR_IZOST_PLA is not displayed,
            //      so, do not calculate it in total
            //      also, remove pause from rad

            TimeSpan ts = new TimeSpan();

            string min = "_min";

            DataRow row = tableCR.NewRow();

            row["employee_id"] = emplData.EmployeeID;
            row["last_name"] = emplData.LastName;
            row["first_name"] = emplData.FirstName;
            row["working_unit"] = emplData.WorkingUnitName;
            row[date] = day;

            TimeSpan emplTotalPayed = new TimeSpan();
            TimeSpan emplTotalNPayed = new TimeSpan();
            for (int i = 0; i < passTypeIDs.Length; i++)
            {
                ts = (TimeSpan)payTotal[passTypeIDs[i]];
                if (passTypeIDs[i] == RAD)
                {
                    TimeSpan tsTemp = new TimeSpan();
                    tsTemp = (TimeSpan)payTotal[OPR_IZOST_PLA];
                    ts -= tsTemp;

                    tsTemp = (TimeSpan)payTotal[SVE_PAUZE];
                    ts -= tsTemp;
                }
                
                if ((passTypeIDs[i] != IZOST) && (passTypeIDs[i] != NEPLA_OD))
                {
                    emplTotalPayed += ts;
                }
                if ((passTypeIDs[i] == IZOST) || (passTypeIDs[i] == NEPLA_OD))
                {
                    emplTotalNPayed += ts;
                }

                row[passTypeIDsForCR[i]] = createTimeString(ts);
                row[passTypeIDsForCR[i] + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;
            }

            ts = (TimeSpan)payTotal[BOL_BEZ_ISPL];
            emplTotalNPayed += ts;
            row[bolBezIspl] = createTimeString(ts);
            row[bolBezIspl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[PRIV_IZL];
            emplTotalPayed += ts;
            row[privIzl] = createTimeString(ts);
            row[privIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[SLUZB_IZL];
            emplTotalPayed += ts;
            row[slIzl] = createTimeString(ts);
            row[slIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[OSTALI_IZL];
            emplTotalPayed += ts;
            row[ostaliIzl] = createTimeString(ts);
            row[ostaliIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[SVE_PAUZE];
            emplTotalPayed += ts;
            row[pause] = createTimeString(ts);
            row[pause + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            //zaokruzivanje
            TimeSpan oneMin = new TimeSpan(0, 1, 0);
            if (emplTotalPayed.Seconds > 29)
                emplTotalPayed = emplTotalPayed.Add(oneMin);
            if (emplTotalNPayed.Seconds > 29)
                emplTotalNPayed = emplTotalNPayed.Add(oneMin);

            row[totalPla] = createTimeString(emplTotalPayed);
            row[totalNepla] = createTimeString(emplTotalNPayed);

            row[totalPlaMin] = (emplTotalPayed.Hours + (emplTotalPayed.Days * 24)) * 60 + emplTotalPayed.Minutes;
            row[totalNeplaMin] = (emplTotalNPayed.Hours + (emplTotalNPayed.Days * 24)) * 60 + emplTotalNPayed.Minutes;

            row["imageID"] = 1;

            tableCR.Rows.Add(row);
            tableCR.AcceptChanges();
        }

        void FillAbsencesCRRowList(int employeeID, EmployeeTO emplData, Hashtable payTotal)
        {
            TimeSpan ts = new TimeSpan();

            string min = "_min";

            DataRow row = tableCR.NewRow();

            row["employee_id"] = emplData.EmployeeID;
            row["last_name"] = emplData.LastName;
            row["first_name"] = emplData.FirstName;
            row["working_unit"] = emplData.WorkingUnitName;
           
            TimeSpan emplTotalPayed = new TimeSpan();
			for (int i = 0; i < passTypeIDs.Length; i++)
			{
                ts = (TimeSpan)payTotal[passTypeIDs[i]];
                if (passTypeIDs[i] == IZOST)
                {
                    emplTotalPayed += ts;
                    row[passTypeIDsForCR[i]] = createTimeString(ts);
                    row[passTypeIDsForCR[i] + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;
                    break;
                }  
			}

            ts = (TimeSpan)payTotal[PRIV_IZL];
            emplTotalPayed += ts;
            row[privIzl] = createTimeString(ts);
            row[privIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[SLUZB_IZL];
            emplTotalPayed += ts;
            row[slIzl] = createTimeString(ts);
            row[slIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            ts = (TimeSpan)payTotal[OSTALI_IZL];
            emplTotalPayed += ts;
            row[ostaliIzl] = createTimeString(ts);
            row[ostaliIzl + min] = (ts.Hours + (ts.Days * 24)) * 60 + ts.Minutes;

            //zaokruzivanje
            TimeSpan oneMin = new TimeSpan(0, 1, 0);
            if (emplTotalPayed.Seconds > 29)
                emplTotalPayed = emplTotalPayed.Add(oneMin);

            row[totalPla] = createTimeString(emplTotalPayed);
            row[totalPlaMin] = (emplTotalPayed.Hours + (emplTotalPayed.Days * 24)) * 60 + emplTotalPayed.Minutes;

            row["priv_izl_NUM"] = privatnoNum;
            row["sl_izl_NUM"] = sluzbenoNum;
            row["ostali_izl_NUM"] = ostaloNum;
            row["izost_NUM"] = neopravdanoNum;
            row["rad_NUM"] = radniNum;

            row["imageID"] = 1;

            tableCR.Rows.Add(row);
            tableCR.AcceptChanges();
        }

        void calculateSummaryTotalsForAnalityc(int employeeID, EmployeeTO emplData, Hashtable payTotal)
        {
            TimeSpan ts = new TimeSpan();
            TimeSpan emplTotalPayed = new TimeSpan();
            TimeSpan emplTotalNPayed = new TimeSpan();

            for (int i = 0; i < passTypeIDs.Length; i++)
            {
                ts = (TimeSpan)payTotal[passTypeIDs[i]];
                if ((passTypeIDs[i] != IZOST) && (passTypeIDs[i] != NEPLA_OD))
                {
                    emplTotalPayed += ts;
                }
                if ((passTypeIDs[i] == IZOST) || (passTypeIDs[i] == NEPLA_OD))
                {
                    emplTotalNPayed += ts;
                }
                if (passTypeIDs[i] == RAD)
                {
                    TimeSpan tsTemp = new TimeSpan();
                    tsTemp = (TimeSpan)payTotal[OPR_IZOST_PLA];
                    ts -= tsTemp;
                    analyticRad = createTimeString(ts);
                }
            }

            ts = (TimeSpan)payTotal[OPR_IZOST];
            emplTotalNPayed += ts;

            ts = (TimeSpan)payTotal[BOL_BEZ_ISPL];
            emplTotalNPayed += ts;

            ts = (TimeSpan)payTotal[OPR_IZOST_PLA];
            emplTotalNPayed -= ts;

            //zaokruzivanje
            TimeSpan oneMin = new TimeSpan(0, 1, 0);
            if (emplTotalPayed.Seconds > 29)
                emplTotalPayed = emplTotalPayed.Add(oneMin);
            if (emplTotalNPayed.Seconds > 29)
                emplTotalNPayed = emplTotalNPayed.Add(oneMin);

            analyticTotalPla = createTimeString(emplTotalPayed);
            analyticTotalNepla = createTimeString(emplTotalNPayed);
        }

        string getMinutesString(int min)
        {
            string minutes = "";
            if (min < 10)
                minutes = "0" + min.ToString();
            else
                minutes = min.ToString();

            return minutes;
        }

        string createTimeString(TimeSpan ts)
        {
            string timeString = "";
            int hours = ts.Hours + (ts.Days * 24);
            int minutes = ts.Minutes;
            string sign = "";
            if ((hours < 0) || (minutes < 0))
            {
                sign = "-";
                if (hours < 0)
                    hours = -hours;
                if (minutes < 0)
                    minutes = -minutes;
            }
            timeString = sign + hours.ToString() + ":" + getMinutesString(minutes);

            return timeString;
        }

        void generateCRReport(string indicator)
        {
            if (tableCR.Rows.Count == 0)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show(rm.GetString("dataNotFound", culture));
                return;
            }
            if ((analyticEmployeeID == -1) && (!absencesReport))
            {
                //if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                //{
                Reports.PIO.PIO_sr.PIOWorkListsCRView_sr view = new Reports.PIO.PIO_sr.PIOWorkListsCRView_sr(dataSetCR, dtpFromDate.Value, dtpToDate.Value, indicator);
                view.ShowDialog(this);
                /*}
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.PIO.PIO_en.PIOWorkListsCRView_en view = new Reports.PIO.PIO_en.PIOWorkListsCRView_en(dataSetCR, dtpFromDate.Value, dtpToDate.Value, indicator);
                    view.ShowDialog(this);
                }*/
            }
            else if (analyticEmployeeID != -1)
            {
                //if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                //{
                Reports.PIO.PIO_sr.PIOWorkListsAnalyticCRView_sr view = new Reports.PIO.PIO_sr.PIOWorkListsAnalyticCRView_sr(dataSetCR, dtpFromDate.Value, dtpToDate.Value,
                    analyticRad, analyticTotalPla, analyticTotalNepla);
                view.ShowDialog(this);
                /*}
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.PIO.PIO_en.PIOWorkListsAnalyticCRView_en view = new Reports.PIO.PIO_en.PIOWorkListsAnalyticCRView_en(dataSetCR, dtpFromDate.Value, dtpToDate.Value,
                       analyticRad, analyticTotalPla, analyticTotalNepla);
                    view.ShowDialog(this);
                }*/
            }
            else if (absencesReport)
            {
                //if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                //{
                Reports.PIO.PIO_sr.PIOWorkListsAbsencesCRView_sr view = new Reports.PIO.PIO_sr.PIOWorkListsAbsencesCRView_sr(dataSetCR, dtpFromDate.Value, dtpToDate.Value);
                view.ShowDialog(this);
                /*}
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.PIO.PIO_en.PIOWorkListsAbsencesCRView_en view = new Reports.PIO.PIO_en.PIOWorkListsAbsencesCRView_en(dataSetCR, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }*/
            }
        }

		private bool isRegularSchema (WorkTimeSchemaTO schema)
		{
			bool isRegular = true;

			try
			{
				if (schema == null)
				{
					return false;
				}

				// Time Schema is regular if cycle duration is 7 and interval duration of last two days are 0
				// is this condition enough to determine regular Time Schema?!!!
				if (schema.CycleDuration != 7)
				{
					isRegular = false;
				}
				else
				{
                    Dictionary<int, WorkTimeIntervalTO> intervals = schema.Days[5];
					TimeSpan hours = new TimeSpan();
					foreach (int intNum in intervals.Keys)
					{
						hours = hours.Add(intervals[intNum].EndTime.Subtract(intervals[intNum].StartTime));
					}

					if (hours.TotalSeconds > 0)
					{
						isRegular = false;
					}
					else
					{
						intervals = schema.Days[6];
						hours = new TimeSpan();
						foreach (int intNum in intervals.Keys)
						{
							hours = hours.Add(intervals[intNum].EndTime.Subtract(intervals[intNum].StartTime));
						}

						if (hours.TotalSeconds > 0)
						{
							isRegular = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.isRegularSchema(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isRegular;
		}
        
        private void PIOPaymentReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.MittalWorkingUnitsReports_Load(): " + ex.Message + "\n");
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
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCriteria_Click_1(object sender, EventArgs e)
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
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged_1(object sender, EventArgs e)
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
                MessageBox.Show(ex.Message);
            }
        }       
	}
}

