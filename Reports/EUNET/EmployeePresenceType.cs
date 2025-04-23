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

using Common;
using TransferObjects;
using Util;

namespace Reports.EUNET
{
	/// <summary>
	/// Summary description for WorkingUnitsReports.
	/// </summary>
	public class EmployeePresenceType : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.CheckBox checkbPDF;
		private System.Windows.Forms.CheckBox checkbCSV;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDocFormat;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;
		ArrayList TypesCells = new ArrayList();
		int[] types;
		string[] values;
		string[] valuesdays;
		int[] total;
		Dictionary<int, string> Types = new Dictionary<int,string>();
		List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

       // private ArrayList TimeSchemaList;

		DebugLog debug;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
		private System.Windows.Forms.Panel panel;

        private const int shiftTime = -200;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;
		string wuString;

		private System.Windows.Forms.Button btnGenerate;
        private GroupBox gbPassTypes;
        private CheckBox chbShowRetired;

		// Key is Type ID, value is its passtype
		Hashtable TypeRow =null;

        int formType;
        const int notAbsenceType = 0;
        private Label lblReportType;
        private RadioButton rbSummary;
        private RadioButton rbAnalytical;
        private GroupBox groupBox1;
        const int absenceType = 1;
        const int pauseAutomated = -102;
        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();
        Dictionary<int, string> timeSchemaTable = new Dictionary<int,string>();

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        List<PassTypeTO> TypeListAll = new List<PassTypeTO>();

        ////if it is JUBMES report
        //bool JUBMESLicence = false;

        Filter filter;

		public EmployeePresenceType()
		{
			InitializeComponent();

            formType = notAbsenceType;
            			
			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			Hashtable TypeRow = new Hashtable();

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePresenceType).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();
			populateWorkigUnitCombo();
			
			DateTime date = DateTime.Now.Date;
			this.CenterToScreen();

			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;
			TypeListAll= new PassType().Search();
			List<PassTypeTO> TypeList = new List<PassTypeTO>();
			foreach(PassTypeTO ptMember in TypeListAll)
			{
				if ((ptMember.PassTypeID >= 0) && (ptMember.IsPass != 0))
				{
                    TypeList.Add(ptMember);
				}
			}

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }

            // get all time schemas
            timeSchemas = new TimeSchema().Search();
			populatePresenceTypeScreen(TypeList);
		}

        public EmployeePresenceType(bool isAbsenceType)
        {
            InitializeComponent();
            formType = absenceType;

            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            Hashtable TypeRow = new Hashtable();

            // get all time schemas
            timeSchemas = new TimeSchema().Search();
            foreach (WorkTimeSchemaTO ts in timeSchemas)
            {
                if (!timeSchemaTable.ContainsKey(ts.TimeSchemaID))
                {
                    timeSchemaTable.Add(ts.TimeSchemaID, ts.Type);
                }
            }

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePresenceType).Assembly);
            setLanguage();
            logInUser = NotificationController.GetLogInUser();
            populateWorkigUnitCombo();

            DateTime date = DateTime.Now.Date;
            this.CenterToScreen();

            dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
            dtpToDate.Value = date;
            List<PassTypeTO> TypeListAll = new PassType().Search();
            List<PassTypeTO> TypeList = new List<PassTypeTO>();
            foreach (PassTypeTO ptMember in TypeListAll)
            {                
                if (((ptMember.PassTypeID >= 0) && (ptMember.IsPass != 0)) || (ptMember.PassTypeID == Constants.absence))
                {
                    TypeList.Add(ptMember);
                }
            }
            
            // get all time schemas
            
            populatePresenceTypeScreen(TypeList);
            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }
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
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.checkbPDF = new System.Windows.Forms.CheckBox();
            this.checkbCSV = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.panel = new System.Windows.Forms.Panel();
            this.gbPassTypes = new System.Windows.Forms.GroupBox();
            this.lblReportType = new System.Windows.Forms.Label();
            this.rbSummary = new System.Windows.Forms.RadioButton();
            this.rbAnalytical = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbPassTypes.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(7, 12);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(343, 91);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(98, 44);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Tag = "FILTERABLE";
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(98, 16);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(230, 21);
            this.cbWorkingUnit.TabIndex = 2;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(44, 16);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 1;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowRetired.Location = new System.Drawing.Point(105, 107);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 18;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(7, 122);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(486, 64);
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
            this.dtpFromDate.Location = new System.Drawing.Point(95, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(49, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(16, 189);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(104, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Tag = "FILTERABLE";
            this.lblDocFormat.Text = "Document Format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDocFormat.Visible = false;
            // 
            // checkbPDF
            // 
            this.checkbPDF.Enabled = false;
            this.checkbPDF.Location = new System.Drawing.Point(144, 189);
            this.checkbPDF.Name = "checkbPDF";
            this.checkbPDF.Size = new System.Drawing.Size(48, 24);
            this.checkbPDF.TabIndex = 10;
            this.checkbPDF.Tag = "FILTERABLE";
            this.checkbPDF.Text = "PDF";
            this.checkbPDF.Visible = false;
            // 
            // checkbCSV
            // 
            this.checkbCSV.Enabled = false;
            this.checkbCSV.Location = new System.Drawing.Point(208, 189);
            this.checkbCSV.Name = "checkbCSV";
            this.checkbCSV.Size = new System.Drawing.Size(48, 24);
            this.checkbCSV.TabIndex = 11;
            this.checkbCSV.Tag = "FILTERABLE";
            this.checkbCSV.Text = "CSV";
            this.checkbCSV.Visible = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(7, 610);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 15;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(389, 610);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Enabled = false;
            this.cbTXT.Location = new System.Drawing.Point(280, 189);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Tag = "FILTERABLE";
            this.cbTXT.Text = "TXT";
            this.cbTXT.Visible = false;
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Enabled = false;
            this.cbCR.Location = new System.Drawing.Point(344, 189);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(56, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            this.cbCR.Visible = false;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Location = new System.Drawing.Point(9, 16);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(471, 320);
            this.panel.TabIndex = 14;
            // 
            // gbPassTypes
            // 
            this.gbPassTypes.Controls.Add(this.panel);
            this.gbPassTypes.Location = new System.Drawing.Point(7, 262);
            this.gbPassTypes.Name = "gbPassTypes";
            this.gbPassTypes.Size = new System.Drawing.Size(486, 342);
            this.gbPassTypes.TabIndex = 17;
            this.gbPassTypes.TabStop = false;
            this.gbPassTypes.Tag = "FILTERABLE";
            this.gbPassTypes.Text = "gbPassTypes";
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(8, 15);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(84, 23);
            this.lblReportType.TabIndex = 19;
            this.lblReportType.Text = "Report Type:";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbSummary
            // 
            this.rbSummary.Checked = true;
            this.rbSummary.Location = new System.Drawing.Point(224, 15);
            this.rbSummary.Name = "rbSummary";
            this.rbSummary.Size = new System.Drawing.Size(104, 24);
            this.rbSummary.TabIndex = 21;
            this.rbSummary.TabStop = true;
            this.rbSummary.Text = "Summary";
            // 
            // rbAnalytical
            // 
            this.rbAnalytical.Location = new System.Drawing.Point(106, 15);
            this.rbAnalytical.Name = "rbAnalytical";
            this.rbAnalytical.Size = new System.Drawing.Size(104, 24);
            this.rbAnalytical.TabIndex = 20;
            this.rbAnalytical.Text = "Analytical";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSummary);
            this.groupBox1.Controls.Add(this.lblReportType);
            this.groupBox1.Controls.Add(this.rbAnalytical);
            this.groupBox1.Location = new System.Drawing.Point(7, 209);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 48);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "FILTERABLE";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(356, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 30;
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
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // EmployeePresenceType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(500, 645);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPassTypes);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkbCSV);
            this.Controls.Add(this.checkbPDF);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbWorkingUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmployeePresenceType";
            this.ShowInTaskbar = false;
            this.Text = "Employee Presence by Type ";
            this.Load += new System.EventHandler(this.EmployeePresenceType_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeePresenceType_KeyUp);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbPassTypes.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
			
		private void populatePresenceTypeScreen(List<PassTypeTO> TypeList)
		{
			try
			{	
				PassTypeTO pt = new PassTypeTO();
				Label TypeLabel = new Label();
				
				int i = 0;
				int j = 0;
				
				foreach(PassTypeTO ptype in TypeList)
				{
					TypeRow = new Hashtable();
					// Draw Types label
					
					TypeLabel = new Label();
					this.panel.Controls.Add(TypeLabel);
					TypeLabel.Text = ptype.Description;
					TypeLabel.Size = new Size(100, 26);
					TypeLabel.TextAlign = ContentAlignment.MiddleCenter;
					TypeLabel.Location = new Point(0+j, 26 * i + 50);

					// Draw Types cell
				
					TypesCell cell = new TypesCell(100+j, 26 * i + 50);
					
					cell.TypeID = ptype.PassTypeID.ToString();
                    cell.AccessibleDescription = ptype.PassTypeID.ToString();
						
					cell.setCheckBoxes();
					TypesCells.Add(cell);
					this.panel.Controls.Add(cell);
					

					TypeRow.Add(ptype.PassTypeID,ptype.Description);	
					i++;
					if (i >= 10) 
					{
						j=j+150;
						i=0;
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.populatePresenceTypeScreen(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbSummary.Checked)
                {
                    Hashtable passTypeSummary = new Hashtable();

                    if (wUnits.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noWUGranted", culture));
                    }
                    else
                    {
                        int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                        if (selectedWorkingUnit == -1)
                        {
                            wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        }
                        if (this.chbHierarhicly.Checked)
                        {
                            WorkingUnit wu = new WorkingUnit();
                            if (selectedWorkingUnit != -1)
                            {
                                wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                                wUnits = wu.Search();
                                wUnits = wu.FindAllChildren(wUnits);
                                selectedWorkingUnit = -1;
                            }
                        }

                        wuString = "";
                        foreach (WorkingUnitTO wu in wUnits)
                        {
                            wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wuString.Length > 0)
                        {
                            wuString = wuString.Substring(0, wuString.Length - 1);
                        }

                        IOPair ioPair = new IOPair();
                        int count = ioPair.SearchForWUCount(selectedWorkingUnit, wuString, dtpFromDate.Value, dtpToDate.Value);
                        if (count > Constants.maxWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                            return;
                        }
                        else if (count > Constants.warningWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                            if (result.Equals(DialogResult.No))
                            {
                                return;
                            }
                        }

                        this.Cursor = Cursors.WaitCursor;

                        List<IOPairTO> ioPairList = ioPair.SearchForWU(selectedWorkingUnit, wuString,
                            dtpFromDate.Value, dtpToDate.Value);
                        
                        // Get Employees for selected Working Unit	
                        Employee empl = new Employee();
                        if (this.chbHierarhicly.Checked || (selectedWorkingUnit == -1))
                        {
                            selectedEmployees = empl.SearchByWU(wuString);
                        }
                        else
                        {
                            selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());
                        }
                        if (!chbShowRetired.Checked)
                        {
                            List<EmployeeTO> emplList = selectedEmployees;
                            selectedEmployees = new List<EmployeeTO>();
                            foreach (EmployeeTO emp in emplList)
                            {
                                if (!emp.Status.Equals(Constants.statusRetired))
                                {
                                    selectedEmployees.Add(emp);
                                }
                            }
                        }
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedule = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

                        foreach (EmployeeTO currentEmployee in selectedEmployees)
                        {
                            List<EmployeeTimeScheduleTO>  emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), dtpFromDate.Value, dtpToDate.Value);

                            if (!emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
                            {
                                emplTimeSchedule.Add(currentEmployee.EmployeeID, new List<EmployeeTimeScheduleTO>());
                            }

                            emplTimeSchedule[currentEmployee.EmployeeID] = emplTS;
                        }

                        Dictionary<int, List<IOPairTO>> classifiedPairs = new Dictionary<int,List<IOPairTO>>();
                        List<IOPairTO> clasifiedPairsList = new List<IOPairTO>();
                        for (int i = 0; i < ioPairList.Count; i++)
                        {
                            //03.02.2009 Natasa if form type is absenceType do not calculate notWorking pairs
                            if ((formType == absenceType
                                && ioPairList[i].IsWrkHrsCount == (int)Constants.IsWrkCount.IsNotCounter) ||
                                (ioPairList[i].StartTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))
                                && ioPairList[i].EndTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))))
                            {
                                continue;
                            }
                            if (ioPairList[i].PassTypeID == pauseAutomated)
                            {
                                ioPairList[i].PassTypeID = Constants.pause;
                            }
                            int currentEmployeeID = ioPairList[i].EmployeeID;
                            if (!classifiedPairs.ContainsKey(currentEmployeeID))
                            {
                                classifiedPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            }

                            classifiedPairs[currentEmployeeID].Add(ioPairList[i]);
                        }

                        /*
                            if (this.checkbPDF.Checked)
                            {
                                this.generateAnalyticalPDFReport(ioPairList);
                            }
                            */

                        // Summary report

                        Hashtable PassTypeSummary = new Hashtable();
                        Hashtable passTypesTotalTime = new Hashtable();
                        Hashtable workOutOfSch = new Hashtable();

                        Hashtable NumDays = new Hashtable();
                        Hashtable emplNumDays = new Hashtable();
                                                
                        List<int> employeesId = new List<int>();
                        foreach (EmployeeTO employ in selectedEmployees)
                        {
                            employeesId.Add(employ.EmployeeID);
                        }

                        // TODO: go trough working unit
                        //int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                        // get Time Schemas for selected Employee and selected Time Interval
                        DateTime date = dtpFromDate.Value.Date;

                        // Key is Pass Type Id, Value is Pass Type Description
                        Types = new Dictionary<int,string>();
                                                
                        foreach (PassTypeTO pt in TypeListAll)
                        {
                            Types.Add(pt.PassTypeID, pt.Description);
                        }
                        //add type regWorkOut for regular work out of time schema intervals
                        Types.Add(shiftTime, rm.GetString("regularWorkOut", culture));

                        // Key is PassTypeID, Value is total time
                        TimeSpan totalTime = new TimeSpan(0);

                        // Totals by PassType
                        foreach (EmployeeTO currentEmployee in selectedEmployees)
                        {
                            List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(currentEmployee.EmployeeID, dtpFromDate.Value.Date, dtpToDate.Value.Date);
                            for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
                            {
                                if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    Hashtable daylyPassTypeSummary = new Hashtable();
                                    Hashtable daylyPassTypesTotalTime = new Hashtable();
                                    foreach (IOPairTO ioTO in classifiedPairs[currentEmployee.EmployeeID])
                                    {
                                        if (isSelectedType(ioTO.PassTypeID))
                                        {
                                            if (day == ioTO.IOPairDate)
                                            {
                                                totalTime = ioTO.EndTime.Subtract(ioTO.StartTime);

                                                if (!daylyPassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
                                                {
                                                    daylyPassTypeSummary.Add(currentEmployee.EmployeeID, new Hashtable());
                                                }

                                                daylyPassTypesTotalTime = (Hashtable)daylyPassTypeSummary[currentEmployee.EmployeeID];

                                                if (daylyPassTypesTotalTime.ContainsKey(ioTO.PassTypeID))
                                                {
                                                    daylyPassTypesTotalTime[ioTO.PassTypeID] = ((TimeSpan)daylyPassTypesTotalTime[ioTO.PassTypeID]).Add(totalTime);
                                                }
                                                else
                                                {
                                                    daylyPassTypesTotalTime.Add(ioTO.PassTypeID, totalTime);
                                                }

                                                daylyPassTypeSummary[currentEmployee.EmployeeID] = daylyPassTypesTotalTime;


                                                if (!PassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
                                                {
                                                    PassTypeSummary.Add(currentEmployee.EmployeeID, new Hashtable());
                                                }

                                                passTypesTotalTime = (Hashtable)PassTypeSummary[currentEmployee.EmployeeID];

                                                if (passTypesTotalTime.ContainsKey(ioTO.PassTypeID))
                                                {
                                                    passTypesTotalTime[ioTO.PassTypeID] = ((TimeSpan)passTypesTotalTime[ioTO.PassTypeID]).Add(totalTime);
                                                }
                                                else
                                                {
                                                    passTypesTotalTime.Add(ioTO.PassTypeID, totalTime);
                                                }

                                                PassTypeSummary[currentEmployee.EmployeeID] = passTypesTotalTime;
                                            }
                                        }
                                    }
                                    
                                    if (emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
                                    {
                                        foreach (PassTypeTO pt in TypeListAll)
                                        {
                                            if (daylyPassTypesTotalTime.ContainsKey(pt.PassTypeID))
                                            {

                                                if (!NumDays.ContainsKey(currentEmployee.EmployeeID))
                                                {
                                                    NumDays.Add(currentEmployee.EmployeeID, new Hashtable());
                                                }

                                                emplNumDays = (Hashtable)NumDays[currentEmployee.EmployeeID];

                                                if (emplNumDays.ContainsKey(pt.PassTypeID))
                                                {
                                                    emplNumDays[pt.PassTypeID] = (int)(emplNumDays[pt.PassTypeID]) + 1;
                                                }
                                                else
                                                {
                                                    emplNumDays.Add(pt.PassTypeID, 1);
                                                }

                                                NumDays[currentEmployee.EmployeeID] = emplNumDays;
                                            }
                                        }
                                    }
                                }
                            }
                            if (PassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
                            {
                                passTypesTotalTime = (Hashtable)PassTypeSummary[currentEmployee.EmployeeID];
                                if (!NumDays.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    NumDays.Add(currentEmployee.EmployeeID, new Hashtable());
                                }

                                emplNumDays = (Hashtable)NumDays[currentEmployee.EmployeeID];

                                int daysNumShift = 0;
                                TimeSpan shiftTotal = new TimeSpan();
                                for (DateTime day1 = dtpFromDate.Value; day1 <= dtpToDate.Value; day1 = day1.AddDays(1))
                                {
                                    if (isHoliday(day1))
                                        continue;
                                    TimeSpan shift = new TimeSpan();
                                    bool is2DayShift = false;
                                    bool is2DaysShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                    //get intervls for employee and day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day1, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                                    Employee employee = new Employee();
                                    employee.EmplTO = currentEmployee;
                                    ArrayList schemas = employee.findTimeSchema(day1);
                                    WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                                    if (schemas.Count > 0)
                                        timeSchema = (WorkTimeSchemaTO)schemas[0];
                                    //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                    if (edi != null)
                                    {
                                        intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                    }//if (edi != null)
                                    else
                                    {
                                        WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                                        intervals.Add(interval);

                                    }
                                    Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                    for (int i = 0; i < intervals.Count; i++)
                                    {
                                        WorkTimeIntervalTO interval = intervals[i];
                                        shift += interval.EndTime.Subtract(interval.StartTime);
                                    }

                                    //foreach (IOPairTO trimPair in trimedPairs)
                                    //{
                                    //    if (trimPair.PassTypeID == Constants.regularWork)
                                    //    {
                                    //        regWorkInWT += trimPair.EndTime.Subtract(trimPair.StartTime);
                                    //    }
                                    //}
                                    if (shift.TotalMinutes > 0)
                                        daysNumShift++;

                                    shiftTotal += shift;

                                }//for

                                passTypesTotalTime.Add(shiftTime, shiftTotal);
                                emplNumDays.Add(shiftTime, daysNumShift);
                            }
                        }
                    
                        if (PassTypeSummary.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }

                        if (passTypeSummary.Count > Constants.maxWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                            return;
                        }
                        else if (passTypeSummary.Count > Constants.warningWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                            if (result.Equals(DialogResult.No))
                            {
                                return;
                            }
                        }

                        if (this.cbCR.Checked)
                        {
                            DataSet dataSet = new DataSet();
                            DataTable table = new DataTable("loc_rep");

                            table.Columns.Add("date", typeof(System.String));
                            table.Columns.Add("last_name", typeof(System.String));
                            table.Columns.Add("first_name", typeof(System.String));
                            table.Columns.Add("location", typeof(System.String));
                            table.Columns.Add("start", typeof(System.String));
                            table.Columns.Add("end", typeof(System.String));
                            table.Columns.Add("type", typeof(System.String));
                            table.Columns.Add("total_time", typeof(System.String));
                            table.Columns.Add("working_unit", typeof(System.String));
                            table.Columns.Add("employee_id", typeof(int));
                            table.Columns.Add("imageID", typeof(byte));

                            DataTable tableI = new DataTable("images");
                            tableI.Columns.Add("image", typeof(System.Byte[]));
                            tableI.Columns.Add("imageID", typeof(byte));

                            int counter = 0;

                            DataRow row = table.NewRow();

                            // Key is Pass Type Id, Value is Pass Type Description
                            List<PassTypeTO> passTypes = new List<PassTypeTO>();

                            int countpt = TypeListAll.Count;
                            countpt++;
                            
                            types = new int[countpt];
                            values = new string[countpt];
                            valuesdays = new string[countpt];
                            total = new int[countpt];
                            
                            foreach (TypesCell cell in TypesCells)
                            {
                                cell.getCheckBoxes();

                                int i = 0;
                                foreach (PassTypeTO ptmember in TypeListAll)
                                {

                                    if ((cell.HasType == 1) && (cell.TypeID == ptmember.PassTypeID.ToString()))
                                    {
                                        passTypes.Add(ptmember);                                      
                                    }

                                    types[i] = ptmember.PassTypeID;
                                    i++;                                   
                                    
                                }
                            }
                            PassTypeTO extraHrs = new PassTypeTO();
                            extraHrs.PassTypeID = shiftTime;
                            types[countpt-1] = shiftTime;                            

                            passTypes.Add(extraHrs);
                            if (passTypes.Count > 8)
                            {
                                if (formType == absenceType && isSelectedType(Constants.regularWork))
                                {
                                    MessageBox.Show(rm.GetString("numberPassTypesRegWork", culture));
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("numberPassTypes", culture));
                                    return;
                                }
                            }
                            if (passTypes.Count == 0)
                            {
                                MessageBox.Show(rm.GetString("selectAtLeastOnePassType", culture));
                                return;
                            }

                            Hashtable currentHT = new Hashtable();
                            Hashtable currentHTDays = new Hashtable();

                            string minutes = "";
                            string h = "";
                            string[] typesheader;
                            typesheader = new string[countpt];

                            for (int j = 0; j < countpt; j++)
                            {
                                foreach (PassTypeTO ptmember in passTypes)
                                {
                                    if (types[j] == ptmember.PassTypeID)
                                    {
                                        if (Types.ContainsKey(types[j]))
                                        {
                                            string buf = (string)Types[types[j]];
                                            typesheader[j] = buf;
                                        }
                                        if (h == "")
                                        {
                                            h = typesheader[j];
                                        }
                                        else
                                        {
                                            h = h + "," + typesheader[j];
                                        }
                                    }
                                }
                            }

                            string emplString = "";
                            foreach (EmployeeTO employee in selectedEmployees)
                            {
                                emplString += employee.EmployeeID.ToString().Trim() + ",";
                            }

                            if (emplString.Length > 0)
                            {
                                emplString = emplString.Substring(0, emplString.Length - 1);
                            }

                            EmployeesTimeSchedule emplTSchedule = new EmployeesTimeSchedule();

                            List<EmployeeTimeScheduleTO> emplTS = emplTSchedule.SearchEmployeesSchedules(emplString, dtpFromDate.Value, dtpToDate.Value);

                            foreach (EmployeeTO currentEmployee in selectedEmployees)
                            {
                                string s = "";
                                string d = "";

                                for (int j = 0; j < countpt; j++)
                                {
                                    values[j] = null;
                                }

                                for (int j = 0; j < countpt; j++)
                                {
                                    valuesdays[j] = null;
                                }

                                IOPair iopair = new IOPair();

                                //foreach (Employee employ in selectedEmployees)
                                //{
                                //    employeesId.Add(employ.EmployeeID);
                                //}

                                List<IOPairTO> ioPairListAll = iopair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                                if (PassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    currentHT = (Hashtable)PassTypeSummary[currentEmployee.EmployeeID];
                                    currentHTDays = (Hashtable)NumDays[currentEmployee.EmployeeID];

                                    row = table.NewRow();
                                    //row["employee_id"] = currentEmployee.EmployeeID;
                                    row["last_name"] = currentEmployee.FirstName + " " + currentEmployee.LastName;
                                    //row["first_name"] = currentEmployee.FirstName;
                                    TimeSpan min = new TimeSpan();
                                    for (int j = 0; j < countpt; j++)
                                    {
                                        if ((currentHT.ContainsKey(types[j])) && (currentHTDays.ContainsKey(types[j])))
                                        {
                                            TimeSpan totalSpan = (TimeSpan)currentHT[types[j]];
                                            int numDays = (int)currentHTDays[types[j]];
                                            if (totalSpan.Minutes < 10)
                                            {
                                                minutes = "0" + totalSpan.Minutes.ToString();
                                            }
                                            else
                                            {
                                                minutes = totalSpan.Minutes.ToString();

                                            }
                                            TimeSpan noSec = new TimeSpan(totalSpan.Days * 24 + totalSpan.Hours, totalSpan.Minutes, 0);                                      
                                            if(j==countpt-1)
                                                min -= noSec;
                                            else
                                                min += noSec;
                                            valuesdays[j] = numDays.ToString();
                                            values[j] = (totalSpan.Days * 24 + totalSpan.Hours).ToString() + ":" + minutes;
                                        }

                                    }
                                    string str = "";
                                    row["employee_id"] = min.TotalMinutes;
                                    if (min.TotalMinutes < 0)
                                        str += "-";
                                    if(Math.Abs(min.Minutes)>=10)
                                        str += Math.Abs(min.Days)*24+Math.Abs(min.Hours) + ":" + Math.Abs(min.Minutes);
                                    else
                                        str += Math.Abs(min.Days) * 24 + Math.Abs(min.Hours) + ":0" + Math.Abs(min.Minutes);
                                    row["first_name"] = str;
                                    for (int j = 0; j < countpt; j++)
                                    {
                                        foreach (PassTypeTO ptmember in passTypes)
                                        {
                                            if (types[j] == ptmember.PassTypeID)
                                            {
                                                if (s == "")
                                                {
                                                    if (values[j] == null)
                                                    {
                                                        string buf = "00:00";
                                                        s = s + buf;
                                                    }
                                                    else
                                                    {
                                                        string buf1 = values[j];
                                                        s = s + buf1;
                                                    }
                                                    //break;
                                                }
                                                else
                                                {
                                                    if (values[j] == null)
                                                    {
                                                        string buf = "00:00";
                                                        s = s + "," + buf;
                                                    }
                                                    else
                                                    {
                                                        string buf1 = values[j];
                                                        s = s + "," + buf1;
                                                    }
                                                    //break;
                                                }

                                                if (d == "")
                                                {
                                                    if (valuesdays[j] == null)
                                                    {
                                                        string buf = "0";
                                                        d = d + buf;
                                                    }
                                                    else
                                                    {
                                                        string buf1 = valuesdays[j];
                                                        d = d + buf1;
                                                    }
                                                    //break;
                                                }
                                                else
                                                {
                                                    if (valuesdays[j] == null)
                                                    {
                                                        string buf = "0";
                                                        d = d + "," + buf;
                                                    }
                                                    else
                                                    {
                                                        string buf1 = valuesdays[j];
                                                        d = d + "," + buf1;
                                                    }
                                                    //break;
                                                }
                                            }
                                        }
                                    }

                                    row["location"] = s;
                                    row["total_time"] = d;
                                    row["type"] = h;

                                    row["working_unit"] = currentEmployee.WorkingUnitName;

                                    row["imageID"] = 1;
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
                                }
                            }
                            table.AcceptChanges();
                            dataSet.Tables.Add(table);
                            dataSet.Tables.Add(tableI);

                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                EUNET_sr.EmployeePresenceTypeCRView_sr view = new EUNET_sr.EmployeePresenceTypeCRView_sr(
                                    dataSet, dtpFromDate.Value, dtpToDate.Value);

                                view.ShowDialog(this);

                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                EUNET_en.EmployeePresenceTypeCRView_en view = new EUNET_en.EmployeePresenceTypeCRView_en(
                                    dataSet, dtpFromDate.Value, dtpToDate.Value);

                                view.ShowDialog(this);
                            }
                        }
                    }

                    //this.Close();
               }
                    //10.02.2009 Natasa add analitical report
                else if (rbAnalytical.Checked)
                {
                    Hashtable passTypeSummary = new Hashtable();

                    if (wUnits.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noWUGranted", culture));
                    }
                    else
                    {
                        int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                        if (selectedWorkingUnit == -1)
                        {
                            wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        }
                        if (this.chbHierarhicly.Checked)
                        {
                            WorkingUnit wu = new WorkingUnit();
                            if (selectedWorkingUnit != -1)
                            {
                                wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                                wUnits = wu.Search();
                                wUnits = wu.FindAllChildren(wUnits);
                                selectedWorkingUnit = -1;
                            }
                        }

                        wuString = "";
                        foreach (WorkingUnitTO wu in wUnits)
                        {
                            wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wuString.Length > 0)
                        {
                            wuString = wuString.Substring(0, wuString.Length - 1);
                        }

                        IOPair ioPair = new IOPair();
                        int count = ioPair.SearchForWUCount(selectedWorkingUnit, wuString, dtpFromDate.Value, dtpToDate.Value);
                        if (count > Constants.maxWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                            return;
                        }
                        else if (count > Constants.warningWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                            if (result.Equals(DialogResult.No))
                            {
                                return;
                            }
                        }

                        this.Cursor = Cursors.WaitCursor;

                        List<IOPairTO> ioPairList = ioPair.SearchForWU(selectedWorkingUnit, wuString,
                            dtpFromDate.Value, dtpToDate.Value.AddDays(1));

                        // Get Employees for selected Working Unit	
                        Employee empl = new Employee();
                        if (this.chbHierarhicly.Checked || (selectedWorkingUnit == -1))
                        {
                            selectedEmployees = empl.SearchByWU(wuString);
                        }
                        else
                        {
                            selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());
                        }
                        if (!chbShowRetired.Checked)
                        {
                            List<EmployeeTO> emplList = selectedEmployees;
                            selectedEmployees = new List<EmployeeTO>();
                            foreach (EmployeeTO emp in emplList)
                            {
                                if (!emp.Status.Equals(Constants.statusRetired))
                                {
                                    selectedEmployees.Add(emp);
                                }
                            }
                        }
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedule = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

                        foreach (EmployeeTO currentEmployee in selectedEmployees)
                        {
                            List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), dtpFromDate.Value, dtpToDate.Value);

                            if (!emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
                            {
                                emplTimeSchedule.Add(currentEmployee.EmployeeID, new List<EmployeeTimeScheduleTO>());
                            }

                            emplTimeSchedule[currentEmployee.EmployeeID] = emplTS;
                        }

                        Dictionary<int, List<IOPairTO>> classifiedPairs = new Dictionary<int,List<IOPairTO>>();
                        List<IOPairTO> clasifiedPairsList = new List<IOPairTO>();
                        for (int i = 0; i < ioPairList.Count; i++)
                        {
                            //03.02.2009 Natasa if form type is absenceType do not calculate notWorking pairs
                            if ((formType == absenceType
                                && ioPairList[i].IsWrkHrsCount == (int)Constants.IsWrkCount.IsNotCounter) ||
                                (ioPairList[i].StartTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))
                                && ioPairList[i].EndTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))))
                            
                            {
                                continue;
                            }
                            if (ioPairList[i].PassTypeID == pauseAutomated)
                            {
                                ioPairList[i].PassTypeID = Constants.pause;
                            }
                            int currentEmployeeID = ioPairList[i].EmployeeID;
                            if (!classifiedPairs.ContainsKey(currentEmployeeID))
                            {
                                classifiedPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            }

                            classifiedPairs[currentEmployeeID].Add(ioPairList[i]);
                        }

                        /*
                            if (this.checkbPDF.Checked)
                            {
                                this.generateAnalyticalPDFReport(ioPairList);
                            }
                            */

                        // Summary report

                        Hashtable EmployeesTotalTime = new Hashtable();
                        
                        List<int> employeesId = new List<int>();
                        foreach (EmployeeTO employ in selectedEmployees)
                        {
                            employeesId.Add(employ.EmployeeID);
                        }

                        // TODO: go trough working unit
                        //int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                        // get Time Schemas for selected Employee and selected Time Interval
                        DateTime date = dtpFromDate.Value.Date;

                        // Key is Pass Type Id, Value is Pass Type Description
                        Types = new Dictionary<int,string>();

                        foreach (PassTypeTO pt in TypeListAll)
                        {
                            Types.Add(pt.PassTypeID, pt.Description);
                        }

                        //add type regWorkOut for regular work out of time schema intervals
                        Types.Add(shiftTime, rm.GetString("regularWorkOut", culture));
                        
                        TimeSpan totalTime = new TimeSpan(0);
                        int numOfRecords = 0;
                        // Totals by PassType
                        foreach (EmployeeTO currentEmployee in selectedEmployees)
                        {
                            List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(currentEmployee.EmployeeID, dtpFromDate.Value.Date, dtpToDate.Value.Date);
                                                     
                            Hashtable daylyPassTypeSummary = new Hashtable();
                            for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
                            {
                                if (!daylyPassTypeSummary.ContainsKey(day))
                                {
                                    daylyPassTypeSummary.Add(day, new Hashtable());
                                }
                                Hashtable passTypeTotalTime = new Hashtable();
                                passTypeTotalTime = (Hashtable)daylyPassTypeSummary[day];
                                if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    foreach (IOPairTO ioTO in classifiedPairs[currentEmployee.EmployeeID])
                                    {
                                        if (isSelectedType(ioTO.PassTypeID))
                                        {
                                            if (day == ioTO.IOPairDate)
                                            {
                                                totalTime = ioTO.EndTime.Subtract(ioTO.StartTime);                                                

                                                if (passTypeTotalTime.ContainsKey(ioTO.PassTypeID))
                                                {
                                                    passTypeTotalTime[ioTO.PassTypeID] = ((TimeSpan)passTypeTotalTime[ioTO.PassTypeID]).Add(totalTime);
                                                }
                                                else
                                                {
                                                    passTypeTotalTime.Add(ioTO.PassTypeID, totalTime);
                                                    numOfRecords++;
                                                }
                                            }
                                        }
                                    }                                    
                                }
                                if (isHoliday(day))
                                    continue;
                                TimeSpan shiftTotal = new TimeSpan();

                                TimeSpan shift = new TimeSpan();
                                bool is2DayShift = false;
                                bool is2DaysShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                //get intervls for employee and day
                                Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                                Employee employee = new Employee();
                                employee.EmplTO = currentEmployee;
                                ArrayList schemas = employee.findTimeSchema(day);
                                WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                                if (schemas.Count > 0)
                                    timeSchema = (WorkTimeSchemaTO)schemas[0];
                                //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                if (edi != null)
                                {
                                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                }//if (edi != null)
                                else
                                {
                                    WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                                    intervals.Add(interval);

                                }
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                for (int i = 0; i < intervals.Count; i++)
                                {
                                    WorkTimeIntervalTO interval = intervals[i];
                                    shift += interval.EndTime.Subtract(interval.StartTime);
                                }

                                shiftTotal += shift;

                                passTypeTotalTime.Add(shiftTime, shiftTotal);
                            }                                
                            
                            if (daylyPassTypeSummary.Count > 0)
                            {
                                EmployeesTotalTime.Add(currentEmployee.EmployeeID, daylyPassTypeSummary);
                            }
                        }

                        if (EmployeesTotalTime.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }

                        if (numOfRecords > Constants.maxWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                            return;
                        }
                        else if (numOfRecords > Constants.warningWUReportRecords)
                        {
                            this.Cursor = Cursors.Arrow;
                            DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                            if (result.Equals(DialogResult.No))
                            {
                                return;
                            }
                        }

                        if (this.cbCR.Checked)
                        {
                            DataSet dataSet = new DataSet();
                            DataTable table = new DataTable("loc_rep");

                            table.Columns.Add("date", typeof(System.String));
                            table.Columns.Add("last_name", typeof(System.String));
                            table.Columns.Add("first_name", typeof(System.String));
                            table.Columns.Add("location", typeof(System.String));
                            table.Columns.Add("start", typeof(System.String));
                            table.Columns.Add("end", typeof(System.String));
                            table.Columns.Add("type", typeof(System.String));
                            table.Columns.Add("total_time", typeof(System.String));
                            table.Columns.Add("working_unit", typeof(System.String));
                            table.Columns.Add("employee_id", typeof(int));
                            table.Columns.Add("imageID", typeof(byte));

                            DataTable tableI = new DataTable("images");
                            tableI.Columns.Add("image", typeof(System.Byte[]));
                            tableI.Columns.Add("imageID", typeof(byte));

                            int counter = 0;

                            DataRow row = table.NewRow();

                            // Key is Pass Type Id, Value is Pass Type Description
                            List<PassTypeTO> passTypes = new List<PassTypeTO>();

                            int countpt = TypeListAll.Count;
                            if (formType == absenceType)
                            {
                                countpt++;
                            }
                            types = new int[countpt];
                            values = new string[countpt];
                            valuesdays = new string[countpt];
                            total = new int[countpt];

                            foreach (TypesCell cell in TypesCells)
                            {
                                cell.getCheckBoxes();

                                int i = 0;
                                foreach (PassTypeTO ptmember in TypeListAll)
                                {

                                    if ((cell.HasType == 1) && (cell.TypeID == ptmember.PassTypeID.ToString()))
                                    {
                                        passTypes.Add(ptmember);
                                    }

                                    types[i] = ptmember.PassTypeID;
                                    i++;
                                }                             
                            }
                            
                            PassTypeTO extraHrs = new PassTypeTO();
                            extraHrs.PassTypeID = shiftTime;
                            types[countpt-1] = shiftTime;                            

                            passTypes.Add(extraHrs);
                            if (passTypes.Count > 8)
                            {
                                  MessageBox.Show(rm.GetString("numberPassTypes", culture));
                                  return;
                            }
                            if (passTypes.Count == 0)
                            {
                                MessageBox.Show(rm.GetString("selectAtLeastOnePassType", culture));
                                return;
                            }

                            Hashtable currentHT = new Hashtable();

                            string minutes = "";
                            string h = "";
                            string[] typesheader;
                            typesheader = new string[countpt];

                            for (int i = 0; i < countpt; i++)
                            {
                                foreach (PassTypeTO ptmember in passTypes)
                                {
                                    if (types[i] == ptmember.PassTypeID)
                                    {
                                        if (Types.ContainsKey(types[i]))
                                        {
                                            string buf = (string)Types[types[i]];
                                            typesheader[i] = buf;

                                        }
                                        if (h == "")
                                        {
                                            h = typesheader[i];
                                        }
                                        else
                                        {
                                            h = h + "," + typesheader[i];
                                        }
                                    }
                                }
                            }

                            string emplString = "";
                            foreach (EmployeeTO employee in selectedEmployees)
                            {
                                emplString += employee.EmployeeID.ToString().Trim() + ",";
                            }

                            if (emplString.Length > 0)
                            {
                                emplString = emplString.Substring(0, emplString.Length - 1);
                            }
                                                        
                            List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(emplString, dtpFromDate.Value, dtpToDate.Value);

                            foreach (EmployeeTO currentEmployee in selectedEmployees)
                            {                                
                                IOPair iopair = new IOPair();

                                List<IOPairTO> ioPairListAll = iopair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                                if (EmployeesTotalTime.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    Hashtable emplTable = (Hashtable)EmployeesTotalTime[currentEmployee.EmployeeID];
                                    for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
                                    {
                                        string s = "";
                                        string d = "";

                                        for (int i = 0; i < countpt; i++)
                                        {
                                            values[i] = null;
                                        }

                                        if (emplTable.ContainsKey(day))
                                        {
                                            currentHT = (Hashtable)emplTable[day];

                                            row = table.NewRow();
                                            row["employee_id"] = currentEmployee.EmployeeID;
                                            row["last_name"] = currentEmployee.FirstName + " " + currentEmployee.LastName;
                                            //row["first_name"] = currentEmployee.FirstName;
                                            row["date"] = day.ToString("dd.MM.yyyy");

                                            TimeSpan min = new TimeSpan();
                                            for (int i = 0; i < countpt; i++)
                                            {
                                                if (currentHT.ContainsKey(types[i]))
                                                {
                                                    TimeSpan totalSpan = (TimeSpan)currentHT[types[i]];
                                                    if (totalSpan.Minutes < 10)
                                                    {
                                                        minutes = "0" + totalSpan.Minutes.ToString();
                                                    }
                                                    else
                                                    {
                                                        minutes = totalSpan.Minutes.ToString();

                                                    }
                                                    TimeSpan noSec = new TimeSpan(totalSpan.Days * 24 + totalSpan.Hours, totalSpan.Minutes, 0);
                                                    if (i == countpt - 1)
                                                        min -= noSec;
                                                    else
                                                        min += noSec;
                                                    values[i] = (totalSpan.Days * 24 + totalSpan.Hours).ToString() + ":" + minutes;
                                                }

                                            }
                                            string str = "";
                                            row["end"] = min.TotalMinutes;
                                            if (min.TotalMinutes < 0)
                                                str += "-";
                                            if (Math.Abs(min.Minutes) >= 10)
                                                str += Math.Abs(min.Hours) + ":" + Math.Abs(min.Minutes);
                                            else
                                                str += Math.Abs(min.Hours) + ":0" + Math.Abs(min.Minutes);
                                            row["start"] = str;
                                            for (int i = 0; i < countpt; i++)
                                            {
                                                foreach (PassTypeTO ptmember in passTypes)
                                                {
                                                    if (types[i] == ptmember.PassTypeID)
                                                    {
                                                        if (s == "")
                                                        {
                                                            if (values[i] == null)
                                                            {
                                                                string buf = "00:00";
                                                                s = s + buf;
                                                            }
                                                            else
                                                            {
                                                                string buf1 = values[i];
                                                                s = s + buf1;
                                                            }
                                                            //break;
                                                        }
                                                        else
                                                        {
                                                            if (values[i] == null)
                                                            {
                                                                string buf = "00:00";
                                                                s = s + "," + buf;
                                                            }
                                                            else
                                                            {
                                                                string buf1 = values[i];
                                                                s = s + "," + buf1;
                                                            }
                                                            //break;
                                                        }
                                                    }
                                                }
                                            }

                                            row["location"] = s;
                                            row["total_time"] = d;
                                            row["type"] = h;

                                            row["working_unit"] = currentEmployee.WorkingUnitName;

                                            row["imageID"] = 1;
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
                                        }
                                    }
                                }
                            }
                            table.AcceptChanges();
                            dataSet.Tables.Add(table);
                            dataSet.Tables.Add(tableI);

                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                EUNET_sr.EmployeePresenceTypeAnaliticCRView_sr view = new EUNET_sr.EmployeePresenceTypeAnaliticCRView_sr(
                                    dataSet, dtpFromDate.Value, dtpToDate.Value);

                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                EUNET_en.EmployeePresenceTypeAnaliticCRView_en view = new EUNET_en.EmployeePresenceTypeAnaliticCRView_en(
                                    dataSet, dtpFromDate.Value, dtpToDate.Value);

                                view.ShowDialog(this);
                            }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        //trim pairs with intervals
        private List<IOPairTO> trimPairsWithIntervals(List<IOPairTO> dayPairs, List<WorkTimeIntervalTO> intervalList, bool is2DayShift, DateTime day, WorkTimeSchemaTO schema)
        {
            List<IOPairTO> trimedPairs = new List<IOPairTO>();
            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
            foreach (WorkTimeIntervalTO interval in intervalList)
            {
                intervals.Add(interval.Clone());
            }
            try
            {
                if (schema.Type.Equals(Constants.schemaTypeFlexi) && dayPairs.Count > 0 && intervalList.Count > 0)
                {
                    WorkTimeIntervalTO interval = intervals[0];
                    IOPairTO firstPair = dayPairs[0];
                    foreach (IOPairTO iopair in dayPairs)
                    {
                        if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
                    }

                    if (firstPair.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.EarliestArrived;
                        interval.StartTime = interval.EarliestArrived;
                        interval.LatestArrivaed = interval.EarliestArrived;
                        interval.EndTime = interval.EarliestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                    if (firstPair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && firstPair.StartTime.TimeOfDay <= interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EndTime = new DateTime(firstPair.StartTime.Ticks + (interval.EndTime.Ticks - interval.StartTime.Ticks));
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                        interval.EarliestArrived = firstPair.StartTime;
                        interval.StartTime = firstPair.StartTime;
                        interval.LatestArrivaed = firstPair.StartTime;

                    }
                    if (firstPair.StartTime.TimeOfDay > interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.LatestArrivaed;
                        interval.StartTime = interval.LatestArrivaed;
                        interval.LatestArrivaed = interval.LatestArrivaed;
                        interval.EndTime = interval.LatestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                }
                foreach (IOPairTO pair in dayPairs)
                {
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        if (interval.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                            interval.EarliestArrived = interval.StartTime;
                        if (interval.LatestLeft.TimeOfDay < interval.EndTime.TimeOfDay)
                            interval.LatestLeft = interval.EndTime;
                        //if whole pair is inside a interval add pair to trimed pairs list
                        if ((pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay || pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay) &&
                           (pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay || pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay))
                        {
                            trimedPairs.Add(pair);
                        }
                        else if (pair.EndTime.TimeOfDay > interval.EarliestArrived.TimeOfDay && pair.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                        {
                            pair.StartTime = new DateTime(pair.StartTime.Year, pair.StartTime.Month, pair.StartTime.Day, interval.EarliestArrived.Hour, interval.EarliestArrived.Minute, 0);
                            if (pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                                  pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay)
                            {
                                pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            }
                            trimedPairs.Add(pair);
                        }
                        else if (pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                       pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay)
                        {
                            pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            trimedPairs.Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.trimPairsWithIntervals(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return trimedPairs;
        }

        private bool isSelectedType(int p)
        {
            bool isSelected = false;
            try
            {
                foreach (TypesCell cell in TypesCells)
                {
                    cell.getCheckBoxes();

                    if ((cell.HasType == 1) && (cell.TypeID == p.ToString()))
                    {
                        isSelected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return isSelected;
        }     		

        private void SelecteType(int p)
        {
            try
            {
                foreach (TypesCell cell in TypesCells)
                {
                    if (cell.TypeID == p.ToString())
                    {
                        cell.HasType = 1;
                        cell.setCheckBoxes();
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.SelecteType(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.populateWorkigUnitCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
				this.Text = rm.GetString("employeePresenceTypeReport", culture);
				gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhicly.Text = rm.GetString("hierarchically", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                gbPassTypes.Text = rm.GetString("gbPassTypes", culture);
                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
                lblReportType.Text = rm.GetString("reportType", culture);
                rbAnalytical.Text = rm.GetString("analitycal", culture);
                rbSummary.Text = rm.GetString("summary", culture);
				
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void chbHierarhicly_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

        private void EmployeePresenceType_KeyUp(object sender, KeyEventArgs e)
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
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWorkingUnit.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            check = true;
                        }
                    }
                }
                chbHierarhicly.Checked = check;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        //calculate holes in working time for employees and days with IOPairs
        //get values in minutes
        public TimeSpan calculateHolesDuringWorkingTime(Dictionary<int, List<IOPairTO>> emplIOPairs, Dictionary<int, List<EmployeeTimeScheduleTO>> employeeTimeShedule, List<EmployeeTO> employees, DateTime fromDay, DateTime toDay)
        {
           TimeSpan holesDuringWorkingTime = new TimeSpan(0);
           try
           {
               IOPair ioPair = new IOPair();
               // all employee time schedules for selected Time Interval, key is employee ID
               Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

               List<int> employeesID = new List<int>();
               foreach (EmployeeTO employee in employees)
               {
                   employeesID.Add(employee.EmployeeID);
               }
               // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
               Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();
               emplPairs = emplIOPairs;
               emplTimeSchedules = employeeTimeShedule;

               //Start calculation
               //for each employee, day, interval in that day
               foreach (int employeeID in employeesID)
               {
                   if (!emplTimeSchedules.ContainsKey(employeeID))
                       continue;
                   
                   TimeSpan intervalDuration = new TimeSpan();
                   for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                   {
                       Employee empl = new Employee();
                       empl.EmplTO.EmployeeID = employeeID;
                       ArrayList schemas = empl.findTimeSchema(day);

                       if (schemas != null && schemas.Count > 0)
                       {
                           WorkTimeSchemaTO schema = (WorkTimeSchemaTO)schemas[0];

                           //for flexi and normal working time don't count holidays as absence
                           if (isHoliday(day) && (schema.Type.Equals(Constants.schemaTypeFlexi) || schema.Type.Equals(Constants.schemaTypeNormal)))
                               continue;
                           //calculate holes only for employee's dates
                           bool is2DayShift = false;
                           bool is2DayShiftPrevious = false;
                           WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                           //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                           //are night shift days. If day is night shift day, also take first interval of next day
                           Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DayShift,
                               ref is2DayShiftPrevious, ref firstIntervalNextDay, this.timeSchemas);

                           TimeSpan ioPairsDuration = new TimeSpan(0);
                           List<IOPairTO> insertedPairs = new List<IOPairTO>();
                           WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                           if (schemas.Count > 0)
                               timeSchema = (WorkTimeSchemaTO)schemas[0];
                           //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                           if (edi != null)
                           {
                               List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);
                               Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                               for (int i = 0; i < intervals.Count; i++)
                               {
                                   WorkTimeIntervalTO interval = intervals[i];
                                   dayIntervals.Add(i, interval);
                               }
                               List<IOPairTO> newList = new List<IOPairTO>();
                               if (emplIOPairs.ContainsKey(empl.EmplTO.EmployeeID))
                                   foreach (IOPairTO iop in emplIOPairs[empl.EmplTO.EmployeeID])
                                   {                                       
                                       newList.Add(iop);
                                   }
                               List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(newList, day, is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, dayIntervals);

                               List<IOPairTO> trimedPairs = trimPairsWithIntervals(dayIOPairList, intervals, is2DayShift, day, timeSchema);

                               for (int i = 0; i < trimedPairs.Count;i++)
                               {
                                   IOPairTO pair = trimedPairs[i];

                                   for (int j = 0; j < insertedPairs.Count;j++)                                      
                                   {
                                       IOPairTO insPair = insertedPairs[j];
                                       if (((pair.EndTime.TimeOfDay >= insPair.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= insPair.EndTime.TimeOfDay)
                                           || (pair.StartTime.TimeOfDay <= insPair.EndTime.TimeOfDay && pair.StartTime.TimeOfDay >= insPair.StartTime.TimeOfDay)
                                           || (pair.StartTime.TimeOfDay <= insPair.StartTime.TimeOfDay && pair.EndTime.TimeOfDay >= insPair.EndTime.TimeOfDay))
                                           && pair.IOPairDate == insPair.IOPairDate)
                                       {
                                           if (pair.EndTime.TimeOfDay <= insPair.EndTime.TimeOfDay && pair.EndTime.TimeOfDay >= insPair.StartTime.TimeOfDay)
                                           {
                                               pair.EndTime = insPair.StartTime;
                                           }
                                           else if (pair.StartTime.TimeOfDay >= insPair.StartTime.TimeOfDay && pair.StartTime.TimeOfDay <= insPair.EndTime.TimeOfDay)
                                           {
                                               pair.StartTime = insPair.EndTime;
                                           }
                                           else
                                           {
                                               IOPairTO newPair = new IOPairTO();
                                               newPair = pair;
                                               newPair.EndTime = insPair.StartTime;
                                               pair.StartTime = insPair.EndTime;
                                               insertedPairs.Add(newPair);
                                               ioPairsDuration += (new DateTime(newPair.EndTime.TimeOfDay.Ticks - newPair.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                           }
                                       }
                                   }
                                   insertedPairs.Add(pair);
                                   ioPairsDuration += pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
                               }

                               foreach (WorkTimeIntervalTO interval in intervals)
                               {
                                   intervalDuration += (new DateTime(interval.EndTime.TimeOfDay.Ticks - interval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                               }
                           }
                           holesDuringWorkingTime += intervalDuration - ioPairsDuration;
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
            return holesDuringWorkingTime;
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        private void EmployeePresenceType_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //List<int> modList = new List<int>();
                //modList = Common.Misc.getLicenceModuls();
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                //if (cost==(int)Constants.Customers.JUBMES)
                //{
                //    JUBMESLicence = true;
                //}
                //else
                //{
                //    JUBMESLicence = false;
                //}
                this.SelecteType(Constants.regularWork);
                this.SelecteType(Constants.pause);
                this.SelecteType(Constants.vacation);
                this.SelecteType(Constants.sickLeave);
                this.SelecteType(Constants.officialOut);
                this.SelecteType(Constants.officialTravel);
                this.SelecteType(Constants.dayOff);
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
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       

       
	}
}

