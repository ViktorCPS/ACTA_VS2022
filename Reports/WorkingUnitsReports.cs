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

namespace Reports
{
	/// <summary>
	/// Summary description for WorkingUnitsReports.
	/// </summary>
	public class WorkingUnitsReports : System.Windows.Forms.Form
    {
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.CheckBox checkbPDF;
		private System.Windows.Forms.CheckBox checkbCSV;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDocFormat;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnGenerate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		DebugLog debug;
		private System.Windows.Forms.RadioButton rbAnalytical;
		private System.Windows.Forms.RadioButton rbSummary;
		private System.Windows.Forms.Label lblReportType;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
        private CheckBox chbShowRetired;


        List<OrganizationalUnitTO>  oUnits = new List<OrganizationalUnitTO>();
		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        List<int> emloyeesID ;
        private TabControl tabWOUnit;
        private TabPage tabWorkingUnit;
        private TabPage tabOrganizationalUnit;
        private CheckBox chbHierarhiclyWU;
        private ComboBox cbWorkingUnit;
        private Label lblWorkingUnitName;
        private CheckBox cbHierarchyOU;
        private ComboBox cbOrganizationalUnit;
        private Label lblOrganizationalUnitName;

        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

        Filter filter;

		public WorkingUnitsReports()
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
            populateOrganizationalUnitCombo();
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
            this.rbAnalytical = new System.Windows.Forms.RadioButton();
            this.rbSummary = new System.Windows.Forms.RadioButton();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.tabWOUnit = new System.Windows.Forms.TabControl();
            this.tabWorkingUnit = new System.Windows.Forms.TabPage();
            this.tabOrganizationalUnit = new System.Windows.Forms.TabPage();
            this.chbHierarhiclyWU = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.cbHierarchyOU = new System.Windows.Forms.CheckBox();
            this.cbOrganizationalUnit = new System.Windows.Forms.ComboBox();
            this.lblOrganizationalUnitName = new System.Windows.Forms.Label();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.tabWOUnit.SuspendLayout();
            this.tabWorkingUnit.SuspendLayout();
            this.tabOrganizationalUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(16, 151);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(464, 64);
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
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(16, 239);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(104, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Tag = "FILTERABLE";
            this.lblDocFormat.Text = "Document Format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkbPDF
            // 
            this.checkbPDF.Enabled = false;
            this.checkbPDF.Location = new System.Drawing.Point(144, 239);
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
            this.checkbCSV.Location = new System.Drawing.Point(208, 239);
            this.checkbCSV.Name = "checkbCSV";
            this.checkbCSV.Size = new System.Drawing.Size(48, 24);
            this.checkbCSV.TabIndex = 11;
            this.checkbCSV.Tag = "FILTERABLE";
            this.checkbCSV.Text = "CSV";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(16, 382);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 17;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(368, 382);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rbAnalytical
            // 
            this.rbAnalytical.Checked = true;
            this.rbAnalytical.Location = new System.Drawing.Point(176, 287);
            this.rbAnalytical.Name = "rbAnalytical";
            this.rbAnalytical.Size = new System.Drawing.Size(104, 24);
            this.rbAnalytical.TabIndex = 15;
            this.rbAnalytical.TabStop = true;
            this.rbAnalytical.Tag = "FILTERABLE";
            this.rbAnalytical.Text = "Analytical";
            this.rbAnalytical.CheckedChanged += new System.EventHandler(this.rbAnalytical_CheckedChanged);
            // 
            // rbSummary
            // 
            this.rbSummary.Location = new System.Drawing.Point(288, 287);
            this.rbSummary.Name = "rbSummary";
            this.rbSummary.Size = new System.Drawing.Size(104, 24);
            this.rbSummary.TabIndex = 16;
            this.rbSummary.Tag = "FILTERABLE";
            this.rbSummary.Text = "Summary";
            this.rbSummary.CheckedChanged += new System.EventHandler(this.rbSummary_CheckedChanged);
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(48, 287);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(100, 23);
            this.lblReportType.TabIndex = 14;
            this.lblReportType.Tag = "FILTERABLE";
            this.lblReportType.Text = "Report Type:";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(280, 239);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Tag = "FILTERABLE";
            this.cbTXT.Text = "TXT";
            this.cbTXT.CheckedChanged += new System.EventHandler(this.cbTXT_CheckedChanged);
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(344, 239);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(56, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            this.cbCR.CheckedChanged += new System.EventHandler(this.cbCR_CheckedChanged);
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowRetired.Location = new System.Drawing.Point(48, 327);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 19;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(344, 40);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 35;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(27, 65);
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
            this.cbFilter.Location = new System.Drawing.Point(5, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // tabWOUnit
            // 
            this.tabWOUnit.Controls.Add(this.tabWorkingUnit);
            this.tabWOUnit.Controls.Add(this.tabOrganizationalUnit);
            this.tabWOUnit.Location = new System.Drawing.Point(19, 12);
            this.tabWOUnit.Name = "tabWOUnit";
            this.tabWOUnit.SelectedIndex = 0;
            this.tabWOUnit.Size = new System.Drawing.Size(322, 133);
            this.tabWOUnit.TabIndex = 36;
            // 
            // tabWorkingUnit
            // 
            this.tabWorkingUnit.BackColor = System.Drawing.Color.Transparent;
            this.tabWorkingUnit.Controls.Add(this.chbHierarhiclyWU);
            this.tabWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.tabWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.tabWorkingUnit.Location = new System.Drawing.Point(4, 22);
            this.tabWorkingUnit.Name = "tabWorkingUnit";
            this.tabWorkingUnit.Padding = new System.Windows.Forms.Padding(3);
            this.tabWorkingUnit.Size = new System.Drawing.Size(314, 107);
            this.tabWorkingUnit.TabIndex = 0;
            this.tabWorkingUnit.Text = "Working Unit";
            // 
            // tabOrganizationalUnit
            // 
            this.tabOrganizationalUnit.BackColor = System.Drawing.Color.Transparent;
            this.tabOrganizationalUnit.Controls.Add(this.cbHierarchyOU);
            this.tabOrganizationalUnit.Controls.Add(this.cbOrganizationalUnit);
            this.tabOrganizationalUnit.Controls.Add(this.lblOrganizationalUnitName);
            this.tabOrganizationalUnit.Location = new System.Drawing.Point(4, 22);
            this.tabOrganizationalUnit.Name = "tabOrganizationalUnit";
            this.tabOrganizationalUnit.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrganizationalUnit.Size = new System.Drawing.Size(314, 107);
            this.tabOrganizationalUnit.TabIndex = 1;
            this.tabOrganizationalUnit.Text = "Organizational Unit";
            // 
            // chbHierarhiclyWU
            // 
            this.chbHierarhiclyWU.Location = new System.Drawing.Point(77, 63);
            this.chbHierarhiclyWU.Name = "chbHierarhiclyWU";
            this.chbHierarhiclyWU.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhiclyWU.TabIndex = 6;
            this.chbHierarhiclyWU.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(77, 22);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(224, 21);
            this.cbWorkingUnit.TabIndex = 5;
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(13, 22);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 4;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbHierarchyOU
            // 
            this.cbHierarchyOU.Location = new System.Drawing.Point(77, 63);
            this.cbHierarchyOU.Name = "cbHierarchyOU";
            this.cbHierarchyOU.Size = new System.Drawing.Size(104, 24);
            this.cbHierarchyOU.TabIndex = 6;
            this.cbHierarchyOU.Text = "Hierarchy ";
            // 
            // cbOrganizationalUnit
            // 
            this.cbOrganizationalUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrganizationalUnit.Location = new System.Drawing.Point(77, 22);
            this.cbOrganizationalUnit.Name = "cbOrganizationalUnit";
            this.cbOrganizationalUnit.Size = new System.Drawing.Size(224, 21);
            this.cbOrganizationalUnit.TabIndex = 5;
            // 
            // lblOrganizationalUnitName
            // 
            this.lblOrganizationalUnitName.Location = new System.Drawing.Point(13, 22);
            this.lblOrganizationalUnitName.Name = "lblOrganizationalUnitName";
            this.lblOrganizationalUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblOrganizationalUnitName.TabIndex = 4;
            this.lblOrganizationalUnitName.Text = "Name:";
            this.lblOrganizationalUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WorkingUnitsReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 431);
            this.ControlBox = false;
            this.Controls.Add(this.tabWOUnit);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.rbSummary);
            this.Controls.Add(this.rbAnalytical);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkbCSV);
            this.Controls.Add(this.checkbPDF);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.gbTimeInterval);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkingUnitsReports";
            this.ShowInTaskbar = false;
            this.Text = "WorkingUnitsReports";
            this.Load += new System.EventHandler(this.WorkingUnitsReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WorkingUnitsReports_KeyUp);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.tabWOUnit.ResumeLayout(false);
            this.tabWorkingUnit.ResumeLayout(false);
            this.tabOrganizationalUnit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnGenerateReport_Click(object sender, System.EventArgs e)
		{
			try
			{

				debug.writeLog(DateTime.Now + " WorkigUnitsReports.btnGenerateReport_Click() \n");
				this.Cursor = Cursors.WaitCursor;
				Hashtable passTypeSummary = new Hashtable();
                if (tabWOUnit.SelectedTab == tabWorkingUnit)
                {
                    if (rbAnalytical.Checked)
                    {
                        if (wUnits.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("noWUGranted", culture));
                        }
                        else
                        {
                            int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;

                            WorkingUnit wu = new WorkingUnit();
                            if (selectedWorkingUnit != -1)
                            {
                                wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                                wUnits = wu.Search();
                            }
                            else
                            {
                                if (selectedWorkingUnit == -1)
                                {
                                    for (int i = wUnits.Count - 1; i >= 0; i--)
                                    {
                                        if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                        {
                                            wUnits.RemoveAt(i);
                                        }
                                    }
                                }
                            }
                            if (this.chbHierarhiclyWU.Checked)
                            {
                                wUnits = wu.FindAllChildren(wUnits);
                                selectedWorkingUnit = -1;
                            }

                            string wuString = "";
                            foreach (WorkingUnitTO wunit in wUnits)
                            {
                                wuString += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (wuString.Length > 0)
                            {
                                wuString = wuString.Substring(0, wuString.Length - 1);
                            }
                            Employee empl = new Employee();//Searching for employees in order to find retired ones
                            List<EmployeeTO> emlList = empl.SearchByWU(wuString);
                            emloyeesID = new List<int>();
                            foreach (EmployeeTO employee in emlList)
                            {
                                if (this.chbShowRetired.Checked)
                                {
                                    emloyeesID.Add(employee.EmployeeID);
                                }
                                else
                                {
                                    if (!employee.Status.Equals(Constants.statusRetired))
                                    {
                                        emloyeesID.Add(employee.EmployeeID);
                                    }
                                }
                            }
                            IOPair ioPair = new IOPair();
                            int count = ioPair.SearchEmplDateCount(this.dtpFromDate.Value, this.dtpToDate.Value, emloyeesID);
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

                            List<IOPairTO> ioPairList = ioPair.SearchEmplDate(this.dtpFromDate.Value, this.dtpToDate.Value, emloyeesID);
                            if (ioPairList.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("dataNotFound", culture));
                                return;
                            }
                            /*
                            if (this.checkbPDF.Checked)
                            {
                                this.generateAnalyticalPDFReport(ioPairList);
                            }
                            */
                            /* iskljucen je.
                            if (this.checkbCSV.Checked)
                            {
                                debug.writeLog(DateTime.Now + " WorkingUnitsReports csv: STARTED!\n");
                                this.generateAnalyticalCSVReport(ioPairList);
                                debug.writeLog(DateTime.Now + " WorkingUnitsReports csv: FNISHED!\n");
                            }
                             * */
                            else
                            {
                                if (this.cbTXT.Checked)
                                {
                                    this.generateAnalyticalTXTReport(ioPairList);
                                }
                                if (this.cbCR.Checked)
                                {
                                    this.generateAnalyticalCRReport(ioPairList);
                                }
                            }

                            //this.Close();
                        }
                    }
                    else
                    {
                        if (cbWorkingUnit.SelectedIndex == 0)
                        {
                            MessageBox.Show(rm.GetString("noWUSelected", culture));
                            return;
                        }
                        // Summary report
                        passTypeSummary = this.SummaryReportData();

                        //If employee did not pass, display 0h00min, so, even if (passTypeSummary.Count == 0)
                        //now there are some results to display
                        /*if (passTypeSummary.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }*/

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
                        
                            /*
                            if (this.checkbPDF.Checked)
                            {
                                this.generateSummaryPDFReport(passTypeSummary);
                            }
                            */
                            if (this.checkbCSV.Checked)
                            {
                                debug.writeLog(DateTime.Now + " WorkingUnitsSummaryReports csv: STARTED!\n");
                                this.generateSummaryCSVReport(passTypeSummary);
                                debug.writeLog(DateTime.Now + " WorkingUnitsSummaryReports csv: FNISHED!\n");
                            }
                            if (passTypeSummary.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("dataNotFound", culture));
                                return;
                            }
                            else
                            {
                                if (this.cbTXT.Checked)
                                {
                                    this.generateSummaryTXTReport(passTypeSummary);
                                }
                                if (this.cbCR.Checked)
                                {
                                    this.generateSummaryCRReport(passTypeSummary);
                                }
                            }
                        //this.Close();
                    }
                }
                else //deo za Organizacione jedinice
                {
                    if (rbAnalytical.Checked)
                    {
                        if (oUnits.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("noWUGranted", culture));
                        }
                        else
                        {
                            int selectedOrgUnit = (int)cbOrganizationalUnit.SelectedValue;

                            OrganizationalUnit ou = new OrganizationalUnit();
                            if (selectedOrgUnit != -1)
                            {
                                ou.OrgUnitTO.OrgUnitID = selectedOrgUnit;
                                oUnits = ou.Search();
                            }
                            else
                            {
                                if (selectedOrgUnit == -1)
                                {
                                    for (int i = oUnits.Count - 1; i >= 0; i--)
                                    {
                                        if (oUnits[i].OrgUnitID == oUnits[i].ParentOrgUnitID)
                                        {
                                            oUnits.RemoveAt(i);
                                        }
                                    }
                                }
                            }
                            if (this.cbHierarchyOU.Checked)
                            {
                                oUnits = ou.FindAllChildren(oUnits);
                                selectedOrgUnit = -1;
                            }

                            string ouString = "";
                            foreach (OrganizationalUnitTO ounit in oUnits)
                            {
                                ouString += ounit.OrgUnitID.ToString().Trim() + ",";
                            }

                            if (ouString.Length > 0)
                            {
                                ouString = ouString.Substring(0, ouString.Length - 1);
                            }
                            Employee empl = new Employee();//Searching for employees in order to find retired ones
                            List<EmployeeTO> emlList = empl.SearchByOU(ouString);
                            emloyeesID = new List<int>();
                            foreach (EmployeeTO employee in emlList)
                            {
                                if (this.chbShowRetired.Checked)
                                {
                                    emloyeesID.Add(employee.EmployeeID);
                                }
                                else
                                {
                                    if (!employee.Status.Equals(Constants.statusRetired))
                                    {
                                        emloyeesID.Add(employee.EmployeeID);
                                    }
                                }
                            }
                            IOPair ioPair = new IOPair();
                            int count = ioPair.SearchEmplDateCount(this.dtpFromDate.Value, this.dtpToDate.Value, emloyeesID);
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

                            List<IOPairTO> ioPairList = ioPair.SearchEmplDate(this.dtpFromDate.Value, this.dtpToDate.Value, emloyeesID);
                            if (ioPairList.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("dataNotFound", culture));
                                return;
                            }
                            else
                            {
                                /*
                                if (this.checkbPDF.Checked)
                                {
                                    this.generateAnalyticalPDFReport(ioPairList);
                                }
                                */
                                /* iskljucen je.
                                if (this.checkbCSV.Checked)
                                {
                                    debug.writeLog(DateTime.Now + " WorkingUnitsReports csv: STARTED!\n");
                                    this.generateAnalyticalCSVReport(ioPairList);
                                    debug.writeLog(DateTime.Now + " WorkingUnitsReports csv: FNISHED!\n");
                                }
                                 * */
                                if (this.cbTXT.Checked)
                                {
                                    this.generateAnalyticalTXTReport(ioPairList);
                                }
                                if (this.cbCR.Checked)
                                {
                                    this.generateAnalyticalCRReport(ioPairList);

                                }
                            }
                            //this.Close();
                        }
                    }
                    else
                    {
                        if (cbOrganizationalUnit.SelectedIndex == 0)
                        {
                            MessageBox.Show(rm.GetString("noWUSelected", culture));
                            return;
                        }
                        // Summary report
                        passTypeSummary = this.OUSummaryReportData();

                        //If employee did not pass, display 0h00min, so, even if (passTypeSummary.Count == 0)
                        //now there are some results to display
                        /*if (passTypeSummary.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }*/

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
                        if (passTypeSummary.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }
                        else
                        {
                            /*
                            if (this.checkbPDF.Checked)
                            {
                                this.generateSummaryPDFReport(passTypeSummary);
                            }
                            */
                            if (this.checkbCSV.Checked)
                            {
                                debug.writeLog(DateTime.Now + " OrganizationUnitsSummaryReports csv: STARTED!\n");
                                this.generateSummaryCSVReport(passTypeSummary);
                                debug.writeLog(DateTime.Now + " WorkingUnitsSummaryReports csv: FNISHED!\n");
                            }
                            if (this.cbTXT.Checked)
                            {
                                this.generateSummaryTXTReport(passTypeSummary);
                            }
                            if (this.cbCR.Checked)
                            {
                                this.generateSummaryCRReport(passTypeSummary);
                            }
                        }
                        //this.Close();
                    }
                }
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.btnGenerateReport_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void populateOrganizationalUnitCombo()
        {
            
            try
            {
                oUnits = new OrganizationalUnit().Search();

                foreach (OrganizationalUnitTO ouTO in oUnits)
                {
                    ouArray.Add(ouTO);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOrganizationalUnit.DataSource = ouArray;
                cbOrganizationalUnit.DisplayMember = "Name";
                cbOrganizationalUnit.ValueMember = "OrgUnitID";

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Employees.populateOrganizationUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.populateWorkigUnitCombo(): " + ex.Message + "\n");
				throw ex;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/*
		private void pdfAnalyticalSetup(PDFDocument doc)
		{
			try
			{
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				// Set header
				doc.Title = "Izveštaj o ukupnom borvaku radnika po tipovima boravka";
				doc.LeftBoxText = "Radna jedinica: " + wu + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.pdfSetup(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		*/

		/*
		private void pdfSummarySetup(PDFDocument doc)
		{
			try
			{
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				// Set header
				
				doc.Title = "Izveštaj o tipu prolazaka \n za radnu jedinicu  za  vremenski period";
				doc.LeftBoxText = "Radna jedinica: " + wu + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
				
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.pdfSetup(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		*/

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("UnitReports", culture);
                lblOrganizationalUnitName.Text = rm.GetString("lblName", culture);
                cbHierarchyOU.Text = rm.GetString("hierarchically", culture);
				//gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhiclyWU.Text = rm.GetString("hierarchically", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				lblReportType.Text = rm.GetString("reportType", culture);
				rbAnalytical.Text = rm.GetString("analitycal", culture);
				rbSummary.Text = rm.GetString("summary", culture);
                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
                tabWorkingUnit.Text = rm.GetString("WorkingUnit",culture);
                tabOrganizationalUnit.Text = rm.GetString("OrganizationalUnit", culture);
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}


		// Header
		/*
		public void InsertTitle(int titleFontSize, PDFDocument doc)
		{
			try
			{
				doc.TextStyle.Bold = true;
				doc.HeaderHeight = Constants.HeaderHeight;
				doc.Rect.String = "main";
				double totalWidth = doc.StandardWidth - (doc.LeftMargine + doc.RightMargine);
				doc.Rect.Height = doc.HeaderHeight;
				
				doc.Color.String = "0 0 0";

				doc.PageNumber = 1;
				doc.Rect.Position(doc.LeftMargine, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.Rect.Width = totalWidth / 4;
				doc.FontSize = Constants.pdfFontSize;
				doc.HPos = 0.0;
				doc.VPos = 1.0;
				doc.AddText(doc.LeftBoxText);
								
				doc.Rect.Position(doc.LeftMargine + totalWidth / 4, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.Rect.Width = 2 * (totalWidth / 4);
				doc.FontSize = titleFontSize;
				doc.HPos = 0.5;
				doc.VPos = 0.0;
				doc.AddText(doc.Title);
				
				doc.Rect.Position(doc.LeftMargine + 3 * (totalWidth / 4), doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.FontSize = Constants.pdfFontSize;
				doc.Rect.Width = totalWidth / 4;
				doc.HPos = 1.0;
				doc.VPos = 1.0;
				doc.AddText(doc.RightBoxText);
				doc.TextStyle.Bold = false;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.InsertTitle(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		/*
		private void generateAnalyticalPDFReport(ArrayList ioPairList)
		{
			try
			{
				PDFDocument doc = new PDFDocument();
				this.pdfAnalyticalSetup(doc);
				this.InsertTitle(14, doc);

				string[] colNames = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};
				int[] colWidths = {3, 3, 4, 3, 3, 3, 3, 4, 3};
				double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
				ArrayList tableData = new ArrayList();

				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				string wuName = "";

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					if ( wuName != pairTO.WUName )
					{
						string[] rowEmpty =  {"", "", "", "", "", "", "", "", ""};
						tableData.Add(rowEmpty);
						
						string[] rowTitle =  {"", pairTO.WUName, "", "", "", "", "", "", ""};
						tableData.Add(rowTitle);
						wuName = pairTO.WUName;
					}

					tableData.Add(rowData);
				}

				doc.InsertTable(colNames, colWidths, colPositions, tableData);
				
				doc.InsertFooter(doc.FontSize);
				doc.Save();

				debug.writeLog(DateTime.Now + " WorkingUnitsReports OPEN Document: Started! \n");
				doc.Open();
				debug.writeLog(DateTime.Now + " WorkingUnitsReports OPEN Document: Finished! \n");
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateAnalyticalPDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		private void generateAnalyticalCSVReport(List<IOPairTO> ioPairList)
		{
			try
			{
				ArrayList tableData = new ArrayList();

				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("lokacija", typeof(System.String));
				table.Columns.Add("ulazak", typeof(System.String));
				table.Columns.Add("izlazak", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6,7,8};
				string[] cHeaders = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
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
					string fileName = Constants.csvDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateAnalyticalCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateAnalyticalTXTReport(List<IOPairTO> ioPairList)
		{
			try
			{
				ArrayList tableData = new ArrayList();

				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("lokacija", typeof(System.String));
				table.Columns.Add("ulazak", typeof(System.String));
				table.Columns.Add("izlazak", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();
				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6,7,8};
				string[] cHeaders = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noTXTExport", culture));
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
					string fileName = Constants.txtDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
					objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);
					System.Diagnostics.Process.Start(fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateAnalyticalTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void rbSummary_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbSummary.Checked)
			{
				rbAnalytical.Checked = false;
			}
			else
			{
				rbAnalytical.Checked = true;
			}
		}

		private void rbAnalytical_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbAnalytical.Checked)
			{
				rbSummary.Checked = false;
			}
			else
			{
				rbSummary.Checked = true;
			}
		}

		private Hashtable SummaryReportData()
		{
			Hashtable passTypesTotalTime = new Hashtable();
			// Key Employee ID, value passTypesTotalTime
			Hashtable emplTypeTotal = new Hashtable();
			
			try
			{
				List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();
				Employee empl = new Employee();
				IOPair ioPair = new IOPair();
                string wuString = "";
                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhiclyWU.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                        }
                        else
                        {
                            if (selectedWorkingUnit == -1)
                            {
                                for (int i = wUnits.Count - 1; i >= 0; i--)
                                {
                                    if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                    {
                                        wUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        wUnits = wu.FindAllChildren(wUnits);
                        selectedWorkingUnit = -1;
                        foreach (WorkingUnitTO wunit in wUnits)
                        {
                            wuString += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wuString.Length > 0)
                        {
                            wuString = wuString.Substring(0, wuString.Length - 1);
                        }
                    }
                    else
                    {
                        wuString = cbWorkingUnit.SelectedValue.ToString();
                    }

                }
				selectedEmployees = empl.SearchByWU(wuString);

				// list of pairs for report
				List<IOPairTO> ioPairList = new List<IOPairTO>();

				// list of Time Schema for selected Employee and selected Time Interval
				ArrayList timeScheduleList = new ArrayList();

				// list of Time Schedule for one month
				ArrayList timeSchedule = new ArrayList();

				
				List<int> employeesId = new List<int>();
				foreach(EmployeeTO employ in selectedEmployees)
				{
					employeesId.Add(employ.EmployeeID);
				}
				
				// TODO: go trough working unit
				//int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);
					
				// get all valid IO Pairs for selected employee and time interval
				ioPairList = ioPair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);
					
				// get Time Schemas for selected Employee and selected Time Interval
				DateTime date = dtpFromDate.Value.Date;

				// Key is Pass Type Id, Value is Pass Type Description
				Dictionary<int, string> passTypes = new Dictionary<int,string>();

				List<PassTypeTO> passTypesAll = new PassType().Search();
				foreach (PassTypeTO pt in passTypesAll)
				{
					passTypes.Add(pt.PassTypeID, pt.Description);
				}

				// Key is PassTypeID, Value is total time
				TimeSpan totalTime = new TimeSpan(0);
				

				// Totals by PassType
				 
					foreach (IOPairTO iopairTO in ioPairList)
					{
                        foreach (EmployeeTO currentEmployee in selectedEmployees)
                        {
						if (iopairTO.EmployeeID.Equals(currentEmployee.EmployeeID))
						{
							totalTime = iopairTO.EndTime.Subtract(iopairTO.StartTime); 
							
							if (!emplTypeTotal.ContainsKey(currentEmployee.EmployeeID))
							{
								emplTypeTotal.Add(currentEmployee.EmployeeID, new Hashtable());
							}
							
							passTypesTotalTime = (Hashtable) emplTypeTotal[currentEmployee.EmployeeID];

							if (passTypesTotalTime.ContainsKey(iopairTO.PassTypeID))
							{
								passTypesTotalTime[iopairTO.PassTypeID] = ((TimeSpan) passTypesTotalTime[iopairTO.PassTypeID]).Add(totalTime);	
							}
							else
							{
								passTypesTotalTime.Add(iopairTO.PassTypeID, totalTime);
							}

							emplTypeTotal[currentEmployee.EmployeeID] = passTypesTotalTime;
						}
					}
					
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.SummaryReportData(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}

			return emplTypeTotal;
		}
        //20.06.2017 Miodrag
        private Hashtable OUSummaryReportData()
        {
            Hashtable passTypesTotalTime = new Hashtable();
            // Key Employee ID, value passTypesTotalTime
            Hashtable emplTypeTotal = new Hashtable();

            try
            {
                List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();
                Employee empl = new Employee();
                IOPair ioPair = new IOPair();
                string ouString = "";
                if (oUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    int selectedOrganizationalUnit = (int)cbOrganizationalUnit.SelectedValue;

                    if (this.cbHierarchyOU.Checked)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        if (selectedOrganizationalUnit != -1)
                        {
                            ou.OrgUnitTO.OrgUnitID = selectedOrganizationalUnit;
                            oUnits = ou.Search();
                        }
                        else
                        {
                            if (selectedOrganizationalUnit == -1)
                            {
                                for (int i = oUnits.Count - 1; i >= 0; i--)
                                {
                                    if (oUnits[i].OrgUnitID == oUnits[i].ParentOrgUnitID)
                                    {
                                        oUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        oUnits = ou.FindAllChildren(oUnits);
                        selectedOrganizationalUnit = -1;
                        foreach (OrganizationalUnitTO ounit in oUnits)
                        {
                            ouString += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (ouString.Length > 0)
                        {
                            ouString = ouString.Substring(0, ouString.Length - 1);
                        }
                    }
                    else
                    {
                        ouString = cbOrganizationalUnit.SelectedValue.ToString();
                    }

                }
                selectedEmployees = empl.SearchByOU(ouString);

                // list of pairs for report
                List<IOPairTO> ioPairList = new List<IOPairTO>();

                // list of Time Schema for selected Employee and selected Time Interval
                ArrayList timeScheduleList = new ArrayList();

                // list of Time Schedule for one month
                ArrayList timeSchedule = new ArrayList();


                List<int> employeesId = new List<int>();
                foreach (EmployeeTO employ in selectedEmployees)
                {
                    employeesId.Add(employ.EmployeeID);
                }

                // TODO: go trough working unit
                //int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                // get all valid IO Pairs for selected employee and time interval
                ioPairList = ioPair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);

                // get Time Schemas for selected Employee and selected Time Interval
                DateTime date = dtpFromDate.Value.Date;

                // Key is Pass Type Id, Value is Pass Type Description
                Dictionary<int, string> passTypes = new Dictionary<int, string>();

                List<PassTypeTO> passTypesAll = new PassType().Search();
                foreach (PassTypeTO pt in passTypesAll)
                {
                    passTypes.Add(pt.PassTypeID, pt.Description);
                }

                // Key is PassTypeID, Value is total time
                TimeSpan totalTime = new TimeSpan(0);


                // Totals by PassType

                foreach (IOPairTO iopairTO in ioPairList)
                {
                    foreach (EmployeeTO currentEmployee in selectedEmployees)
                    {
                        if (iopairTO.EmployeeID.Equals(currentEmployee.EmployeeID))
                        {
                            totalTime = iopairTO.EndTime.Subtract(iopairTO.StartTime);

                            if (!emplTypeTotal.ContainsKey(currentEmployee.EmployeeID))
                            {
                                emplTypeTotal.Add(currentEmployee.EmployeeID, new Hashtable());
                            }

                            passTypesTotalTime = (Hashtable)emplTypeTotal[currentEmployee.EmployeeID];

                            if (passTypesTotalTime.ContainsKey(iopairTO.PassTypeID))
                            {
                                passTypesTotalTime[iopairTO.PassTypeID] = ((TimeSpan)passTypesTotalTime[iopairTO.PassTypeID]).Add(totalTime);
                            }
                            else
                            {
                                passTypesTotalTime.Add(iopairTO.PassTypeID, totalTime);
                            }

                            emplTypeTotal[currentEmployee.EmployeeID] = passTypesTotalTime;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WorkingUnitsReports.OUSummaryReportData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

            return emplTypeTotal;
        }
        //MM
		/*
		private void generateSummaryPDFReport(Hashtable summaryTypeList)
		{
			ArrayList selectedEmployees = new ArrayList();
			try
			{
				PDFDocument doc = new PDFDocument();
				this.pdfSummarySetup(doc);
				this.InsertTitle(14, doc);

				string[] colNames = {"rbr","Prezime", "Ime", "Tip", "Trajanje"};
				int[] colWidths = {1, 3, 3, 3, 3};
				double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
				ArrayList tableData = new ArrayList();

				// Get Employees for selected Working Unit			
				Employee empl = new Employee();
				selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());

				// Key is Pass Type Id, Value is Pass Type Description
				ArrayList passTypesAll = new PassType().Search("", "", "", "","");
				int index = 0;
				Hashtable currentHT = new Hashtable();

				foreach(Employee currentEmployee in selectedEmployees)
				{
					if (summaryTypeList.ContainsKey(currentEmployee.EmployeeID))
					{
						currentHT = (Hashtable) summaryTypeList[currentEmployee.EmployeeID];
						foreach(PassType currentPassType in passTypesAll)
						{
							if (currentHT.ContainsKey(currentPassType.PassTypeID))
							{
								index++;
								TimeSpan totalSpan = (TimeSpan) currentHT[currentPassType.PassTypeID];
								string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, currentPassType.Description,
													   (totalSpan.Days * 24 + totalSpan.Hours).ToString() + "h " + totalSpan.Minutes + "min"};
								tableData.Add(rowData);
							}
						}
					}
				}
				
				doc.InsertTable(colNames, colWidths, colPositions, tableData);
				
				doc.InsertFooter(doc.FontSize);
				doc.Save();

				debug.writeLog(DateTime.Now + " SummaryWorkingUnitsReports OPEN Document: Started! \n");
				doc.Open();
				debug.writeLog(DateTime.Now + " SummaryWorkingUnitsReports OPEN Document: Finished! \n");
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateSummaryPDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/


		private void generateSummaryCSVReport(Hashtable employeeTotals)
		{
			List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();
			try
			{
				// Get Employees for selected Working Unit			
				Employee empl = new Employee();
				selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());

				// Key is Pass Type Id, Value is Pass Type Description
				List<PassTypeTO> passTypesAll = new PassType().Search();
				int index = 0;
				Hashtable currentHT = new Hashtable();

				ArrayList tableData = new ArrayList();

				foreach(EmployeeTO currentEmployee in selectedEmployees)
				{
					if (employeeTotals.ContainsKey(currentEmployee.EmployeeID))
					{
						currentHT = (Hashtable) employeeTotals[currentEmployee.EmployeeID];
						foreach(PassTypeTO currentPassType in passTypesAll)
						{
							if (currentHT.ContainsKey(currentPassType.PassTypeID))
							{
								index++;
								TimeSpan totalSpan = (TimeSpan) currentHT[currentPassType.PassTypeID];
								string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, currentPassType.Description,
													   (totalSpan.Days * 24 + totalSpan.Hours).ToString() + "h " + totalSpan.Minutes + "min"};
								tableData.Add(rowData);
							}
						}
					}
                    else
                    {
                        index++;
                        string description = "";
                        foreach (PassTypeTO currentPassType in passTypesAll)
                        {
                            if (currentPassType.PassTypeID == Constants.regularWork)
                            {
                                description = currentPassType.Description;
                                break;
                            }
                        }

                        string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, description,
													   "0h 00min"};
                        tableData.Add(rowData);
                    }
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4};
				string[] cHeaders = {"rbr","Prezime", "Ime", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
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
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateSummaryCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateSummaryTXTReport(Hashtable employeeTotals)
		{
			List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();
			try
			{
				// Get Employees for selected Working Unit			
				Employee empl = new Employee();
                if (tabWOUnit.SelectedTab == tabWorkingUnit)
                {
                    string wuString = "";
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhiclyWU.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                        }
                        else
                        {
                            if (selectedWorkingUnit == -1)
                            {
                                for (int i = wUnits.Count - 1; i >= 0; i--)
                                {
                                    if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                    {
                                        wUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        wUnits = wu.FindAllChildren(wUnits);
                        selectedWorkingUnit = -1;
                        foreach (WorkingUnitTO wunit in wUnits)
                        {
                            wuString += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wuString.Length > 0)
                        {
                            wuString = wuString.Substring(0, wuString.Length - 1);
                        }
                    }
                    else
                    {
                        wuString = cbWorkingUnit.SelectedValue.ToString();
                    }

                    selectedEmployees = empl.SearchByWU(wuString);
                }
                else //odavde krecu organizacione
                {
                    string ouString = "";
                    int selectedOrgUnit = (int)cbOrganizationalUnit.SelectedValue;

                    if (this.cbHierarchyOU.Checked)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        if (selectedOrgUnit != -1)
                        {
                            ou.OrgUnitTO.OrgUnitID = selectedOrgUnit;
                            oUnits = ou.Search();
                        }
                        else
                        {
                            if (selectedOrgUnit == -1)
                            {
                                for (int i = oUnits.Count - 1; i >= 0; i--)
                                {
                                    if (oUnits[i].OrgUnitID == oUnits[i].ParentOrgUnitID)
                                    {
                                        oUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        oUnits = ou.FindAllChildren(oUnits);
                        selectedOrgUnit = -1;
                        foreach (OrganizationalUnitTO ounit in oUnits)
                        {
                            ouString += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (ouString.Length > 0)
                        {
                            ouString = ouString.Substring(0, ouString.Length - 1);
                        }
                    }
                    else
                    {
                        ouString = cbOrganizationalUnit.SelectedValue.ToString();
                    }

                    selectedEmployees = empl.SearchByOU(ouString);
                }
                List<EmployeeTO> emplList = new List<EmployeeTO>();

                foreach (EmployeeTO employee in selectedEmployees)
                {
                    if (this.chbShowRetired.Checked)
                    {
                        emplList.Add(employee);
                    }
                    else
                    {
                        if (!employee.Status.Equals(Constants.statusRetired))
                        {
                            emplList.Add(employee);
                        }
                    }
                }
				List<PassTypeTO> passTypesAll = new PassType().Search();
				int index = 0;
				Hashtable currentHT = new Hashtable();

				ArrayList tableData = new ArrayList();

				foreach(EmployeeTO currentEmployee in emplList)
				{
					if (employeeTotals.ContainsKey(currentEmployee.EmployeeID))
					{
						currentHT = (Hashtable) employeeTotals[currentEmployee.EmployeeID];
						foreach(PassTypeTO currentPassType in passTypesAll)
						{
							if (currentHT.ContainsKey(currentPassType.PassTypeID))
							{
								index++;
								TimeSpan totalSpan = (TimeSpan) currentHT[currentPassType.PassTypeID];
								string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, currentPassType.Description,
													   (totalSpan.Days * 24 + totalSpan.Hours).ToString() + "h " + totalSpan.Minutes + "min"};
								tableData.Add(rowData);
							}
						}
					}
                    else
                    {
                        index++;
                        string description = "";
                        foreach (PassTypeTO currentPassType in passTypesAll)
                        {
                            if (currentPassType.PassTypeID == Constants.regularWork)
                            {
                                description = currentPassType.Description;
                                break;
                            }
                        }

                        string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, description,
													   "0h 00min"};
                        tableData.Add(rowData);
                    }
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4};
				string[] cHeaders = {"rbr","Prezime", "Ime", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					//19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
					Export objExport = new Export("Win", cHeaders);
					
					
                    
                    string fileName;
                    if (tabWOUnit.SelectedTab == tabWorkingUnit)
                    {
                        string wu = "";
                        if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
                        {
                            wu = rm.GetString("repAll", culture);
                        }
                        else
                        {
                            wu = cbWorkingUnit.Text.Trim();
                        }
                        fileName = Constants.txtDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";

                    }
                    else
                    {
                        string ou = "";
                        if (cbOrganizationalUnit.Text.Trim().Equals(rm.GetString("all", culture)))
                        {
                            ou = rm.GetString("repAll", culture);
                        }
                        else
                        {
                            ou = cbWorkingUnit.Text.Trim();
                        }
                         fileName = Constants.txtDocPath + "\\Sumarni izvestaj po organizacionoj jedinici " + ou + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
                    }
                    objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);
					System.Diagnostics.Process.Start(fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateSummaryTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateAnalyticalCRReport(List<IOPairTO> dataList)
		{
			try
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
                table.Columns.Add("organizational_unit", typeof(System.String));
				table.Columns.Add("employee_id", typeof(int));
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

				TimeSpan timeSpan = new TimeSpan();
                
				string minutes;
                int i = 0;
				foreach(IOPairTO pair in dataList)
				{
					DataRow row = table.NewRow();
					
					row["date"] = pair.IOPairDate.ToString("dd.MM.yyy");
					row["first_name"] = pair.EmployeeName;
					row["last_name"] = pair.EmployeeLastName;
					row["location"] = pair.LocationName;
					row["start"] = pair.StartTime.ToString("HH:mm");
					row["end"] = pair.EndTime.ToString("HH:mm");
					row["type"] = pair.PassType;
					timeSpan = pair.EndTime.Subtract(pair.StartTime);
					
					if (timeSpan.Minutes < 10) 
					{
						minutes = "0" + timeSpan.Minutes.ToString();
					}
					else
					{
						minutes = timeSpan.Minutes.ToString();
						
					}
					row["total_time"] = timeSpan.Hours.ToString() + "h " + minutes + "min ";
					
                    if(tabWOUnit.SelectedTab==tabOrganizationalUnit)
                    {
                        int j = 1;
                        Employee em = new Employee();
                        em.EmplTO = em.Find(pair.EmployeeID.ToString().Trim());
                        for (j = 1; j < ouArray.Count; j++)
                        {
                            //log.writeLog(ouArray.Count+" - "+ ouArray[j].OrgUnitID + " == " + employee.OrgUnitID);
                            if (ouArray[j].OrgUnitID == em.EmplTO.OrgUnitID)
                            {
                                row["working_unit"] =  ouArray[j].Name;
                                break;
                            }
                        }
                    
                    }
                    else
                    {
                        row["working_unit"] = pair.WUName;
                    }
					row["employee_id"] = pair.EmployeeID;

                    row["imageID"] = 1;
                    if (i == 0)
                    {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

					table.Rows.Add(row);
                    i++;
				}
				table.AcceptChanges();
				dataSet.Tables.Add(table);
                dataSet.Tables.Add(tableI);

				if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
				{
					Reports_sr.WorkingUnitCRView view = new Reports_sr.WorkingUnitCRView(
						dataSet, dtpFromDate.Value, dtpToDate.Value);
					view.ShowDialog(this);
				}
				else if(NotificationController.GetLanguage().Equals(Constants.Lang_en))
				{
					Reports_en.WorkingUnitCRView_en view = new Reports_en.WorkingUnitCRView_en(
						dataSet, dtpFromDate.Value, dtpToDate.Value);
					view.ShowDialog(this);
				}
		}
		catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateAnalyticalCRReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
					
}

        private void generateSummaryCRReport(Hashtable summaryTypeList)
        {
            List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

            try
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
                table.Columns.Add("organizational_unit", typeof(System.String));
                table.Columns.Add("employee_id", typeof(int));
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                DataRow row = table.NewRow();

                // Get Employees for selected Working Unit			
                Employee empl = new Employee();
                if (tabWOUnit.SelectedTab == tabWorkingUnit)
                {
                    string wuString = "";
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                    
                    if (this.chbHierarhiclyWU.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                        }
                        else
                        {
                            if (selectedWorkingUnit == -1)
                            {
                                for (int i = wUnits.Count - 1; i >= 0; i--)
                                {
                                    if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                    {
                                        wUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        wUnits = wu.FindAllChildren(wUnits);
                        selectedWorkingUnit = -1;
                        foreach (WorkingUnitTO wunit in wUnits)
                        {
                            wuString += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (wuString.Length > 0)
                        {
                            wuString = wuString.Substring(0, wuString.Length - 1);
                        }
                    }
                    else
                    {
                        wuString = cbWorkingUnit.SelectedValue.ToString();
                    }

                    selectedEmployees = empl.SearchByWU(wuString);
                }
                else //odavde krecu organizacione
                {
                    string ouString = "";
                    int selectedOrgUnit = (int)cbOrganizationalUnit.SelectedValue;

                    if (this.cbHierarchyOU.Checked)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        if (selectedOrgUnit != -1)
                        {
                            ou.OrgUnitTO.OrgUnitID = selectedOrgUnit;
                            oUnits = ou.Search();
                        }
                        else
                        {
                            if (selectedOrgUnit == -1)
                            {
                                for (int i = oUnits.Count - 1; i >= 0; i--)
                                {
                                    if (oUnits[i].OrgUnitID == oUnits[i].ParentOrgUnitID)
                                    {
                                        oUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        oUnits = ou.FindAllChildren(oUnits);
                        selectedOrgUnit = -1;
                        foreach (OrganizationalUnitTO ounit in oUnits)
                        {
                            ouString += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (ouString.Length > 0)
                        {
                            ouString = ouString.Substring(0, ouString.Length - 1);
                        }
                    }
                    else
                    {
                        ouString = cbOrganizationalUnit.SelectedValue.ToString();
                    }

                    selectedEmployees = empl.SearchByOU(ouString);
                }
                List<EmployeeTO> emplList = new List<EmployeeTO>();

                foreach (EmployeeTO employee in selectedEmployees)
                {
                    if (this.chbShowRetired.Checked)
                    {
                        emplList.Add(employee);
                    }
                    else
                    {
                        if (!employee.Status.Equals(Constants.statusRetired))
                        {
                            emplList.Add(employee);
                        }
                    }
                }
                // Key is Pass Type Id, Value is Pass Type Description
                List<PassTypeTO> passTypesAll = new PassType().Search();
                Hashtable currentHT = new Hashtable();

                string minutes;
                int j = 0;
                foreach (EmployeeTO currentEmployee in emplList)
                {
                    if (summaryTypeList.ContainsKey(currentEmployee.EmployeeID))
                    {
                        currentHT = (Hashtable)summaryTypeList[currentEmployee.EmployeeID];
                        foreach (PassTypeTO currentPassType in passTypesAll)
                        {
                            if (currentHT.ContainsKey(currentPassType.PassTypeID))
                            {
                                TimeSpan totalSpan = (TimeSpan)currentHT[(object)currentPassType.PassTypeID];
                                //TimeSpan totalSpan = (TimeSpan) currentHT[currentPassType.PassTypeID;

                                row = table.NewRow();
                                row["last_name"] = currentEmployee.LastName;
                                row["first_name"] = currentEmployee.FirstName;
                                row["type"] = currentPassType.Description;

                                if (totalSpan.Minutes < 10)
                                {
                                    minutes = "0" + totalSpan.Minutes.ToString();
                                }
                                else
                                {
                                    minutes = totalSpan.Minutes.ToString();

                                }
                                row["total_time"] = (totalSpan.Days * 24 + totalSpan.Hours).ToString() + "h " + minutes + "min";
                                //mm
                                if (tabWOUnit.SelectedTab == tabOrganizationalUnit)
                                {
                                    int k = 1;
                                    
                                    for (k = 1; k < ouArray.Count; k++)
                                    {

                                        if (ouArray[k].OrgUnitID == currentEmployee.OrgUnitID)
                                        {
                                            row["working_unit"] = ouArray[k].Name;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    row["working_unit"] = currentEmployee.WorkingUnitName;
                                }
                                //mm
                                //
                                row["employee_id"] = currentEmployee.EmployeeID;

                                row["imageID"] = 1;
                                if (j == 0)
                                {
                                    //add logo image just once
                                    DataRow rowI = tableI.NewRow();
                                    rowI["image"] = Constants.LogoForReport;
                                    rowI["imageID"] = 1;
                                    tableI.Rows.Add(rowI);
                                    tableI.AcceptChanges();
                                }

                                table.Rows.Add(row);
                                j++;
                            }
                        }
                    }
                    else
                    {
                        row = table.NewRow();
                        row["last_name"] = currentEmployee.LastName;
                        row["first_name"] = currentEmployee.FirstName;
                        foreach (PassTypeTO currentPassType in passTypesAll)
                        {
                            if (currentPassType.PassTypeID == Constants.regularWork)
                            {
                                row["type"] = currentPassType.Description;
                                break;
                            }
                        }
                        row["total_time"] = "0h 00min";
                        //mm
                        if (tabWOUnit.SelectedTab == tabOrganizationalUnit)
                        {
                            int k = 1;

                            for (k = 1; k < ouArray.Count; k++)
                            {

                                if (ouArray[k].OrgUnitID == currentEmployee.OrgUnitID)
                                {
                                    row["working_unit"] = ouArray[k].Name;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            row["working_unit"] = currentEmployee.WorkingUnitName;
                        }
                        //mm
                        row["employee_id"] = currentEmployee.EmployeeID;

                        row["imageID"] = 1;
                        if (j == 0)
                        {
                            //add logo image just once
                            DataRow rowI = tableI.NewRow();
                            rowI["image"] = Constants.LogoForReport;
                            rowI["imageID"] = 1;
                            tableI.Rows.Add(rowI);
                            tableI.AcceptChanges();
                        }

                        table.Rows.Add(row);
                        j++;
                    }
                }

                table.AcceptChanges();
                dataSet.Tables.Add(table);
                dataSet.Tables.Add(tableI);

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports_sr.WorkingUnitSummaryCRView view = new Reports_sr.WorkingUnitSummaryCRView(
                        dataSet, dtpFromDate.Value, dtpToDate.Value);

                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports_en.WorkingUnitSummaryCRView_en view = new Reports_en.WorkingUnitSummaryCRView_en(
                        dataSet, dtpFromDate.Value, dtpToDate.Value);

                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WorkingUnitsReports.generateSummaryPDFReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbCR_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCR.Checked)
            {
                cbTXT.Checked = false;
            }
            else
            {
                cbTXT.Checked = true;
            }
        }

        private void cbTXT_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTXT.Checked)
            {
                cbCR.Checked = false;
            }
            else
            {
                cbCR.Checked = true;
            }
        }

        private void WorkingUnitsReports_KeyUp(object sender, KeyEventArgs e)
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
        /*
        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
                chbHierarhiclyWU.Checked = check;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        */
        private void WorkingUnitsReports_Load(object sender, EventArgs e)
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
            finally { 
                this.Cursor = Cursors.Arrow;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
	}
}

