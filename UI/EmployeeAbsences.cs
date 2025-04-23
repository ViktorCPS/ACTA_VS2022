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

using TransferObjects;
using Common;
using Util;

namespace UI
{
    /// <summary>
    /// Summary description for EmployeeAbsences.
    /// </summary>
    public class EmployeeAbsences : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblAbsence;
        private System.Windows.Forms.ComboBox cbAbsenceType;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private EmployeeAbsenceTO currentEmplAbs = null;

        // List View indexes
        //const int RecIDIndex = 0;
        const int EmployeeIDIndex = 0;
        const int PassTypeIDIndex = 1;
        const int DateStartIndex = 2;
        const int DateEndIndex = 3;
        const int UsedIndex = 4;
        const int VacationYearIndex = 5;
        const int DescriptionIndex = 6;

        private CultureInfo culture;
        private ResourceManager rm;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ListView lvAbsences;
        private System.Windows.Forms.GroupBox gbEmployeeAbsence;
        DebugLog log;
        ApplUserTO logInUser;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;

        List<EmployeeAbsenceTO> currentEmployeesAbsList;
        private int sortOrder;
        private int sortField;
        private int startIndex;
        private System.Windows.Forms.Label lblTotal;
        private Button btnWUTree;
        private CheckBox chbHierarhicly;
        private Button btnGenerate;
        private GroupBox gbReport;
        private RadioButton rbNo;
        private RadioButton rbYes;

        // Date Time format
        protected string dateTimeformat = "";
        private GroupBox gbIncludeHolidays;
        private Label lblOnlyVacation;

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        //for employeePersonalRecordsMDI
        List<EmployeeTO> currentEmplArray;

        private Filter filter;
        string absence = "Neopravdano odsustvo";
        private CheckBox chbIncludeUnjustified;
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        public EmployeeAbsences()
        {
            InitializeComponent();

            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
            setLanguage();
            currentEmplAbs = new EmployeeAbsenceTO();
            currentEmployeesAbsList = new List<EmployeeAbsenceTO>();
            logInUser = NotificationController.GetLogInUser();

            populateAbsenceTypeCombo();

            // Set datetime format
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;            
        }

        #region MDI child method's
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                dtpStartDate.Value = from;
                dtpEndDate.Value = to;
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWU.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currentEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmployee.SelectedValue = selectedEmployeeID;
                }

                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.changeSelectedEmployee(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
       
        #endregion
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeAbsences));
            this.gbEmployeeAbsence = new System.Windows.Forms.GroupBox();
            this.chbIncludeUnjustified = new System.Windows.Forms.CheckBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.cbAbsenceType = new System.Windows.Forms.ComboBox();
            this.lblAbsence = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lvAbsences = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.gbIncludeHolidays = new System.Windows.Forms.GroupBox();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.lblOnlyVacation = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbEmployeeAbsence.SuspendLayout();
            this.gbReport.SuspendLayout();
            this.gbIncludeHolidays.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmployeeAbsence
            // 
            this.gbEmployeeAbsence.Controls.Add(this.chbIncludeUnjustified);
            this.gbEmployeeAbsence.Controls.Add(this.chbHierarhicly);
            this.gbEmployeeAbsence.Controls.Add(this.btnWUTree);
            this.gbEmployeeAbsence.Controls.Add(this.cbWU);
            this.gbEmployeeAbsence.Controls.Add(this.lblWU);
            this.gbEmployeeAbsence.Controls.Add(this.btnSearch);
            this.gbEmployeeAbsence.Controls.Add(this.dtpEndDate);
            this.gbEmployeeAbsence.Controls.Add(this.lblEndDate);
            this.gbEmployeeAbsence.Controls.Add(this.dtpStartDate);
            this.gbEmployeeAbsence.Controls.Add(this.lblStartDate);
            this.gbEmployeeAbsence.Controls.Add(this.cbAbsenceType);
            this.gbEmployeeAbsence.Controls.Add(this.lblAbsence);
            this.gbEmployeeAbsence.Controls.Add(this.cbEmployee);
            this.gbEmployeeAbsence.Controls.Add(this.lblEmployee);
            this.gbEmployeeAbsence.Location = new System.Drawing.Point(16, 16);
            this.gbEmployeeAbsence.Name = "gbEmployeeAbsence";
            this.gbEmployeeAbsence.Size = new System.Drawing.Size(418, 254);
            this.gbEmployeeAbsence.TabIndex = 0;
            this.gbEmployeeAbsence.TabStop = false;
            this.gbEmployeeAbsence.Text = "Employee Absences";
            // 
            // chbIncludeUnjustified
            // 
            this.chbIncludeUnjustified.Location = new System.Drawing.Point(104, 216);
            this.chbIncludeUnjustified.Name = "chbIncludeUnjustified";
            this.chbIncludeUnjustified.Size = new System.Drawing.Size(160, 24);
            this.chbIncludeUnjustified.TabIndex = 13;
            this.chbIncludeUnjustified.Text = "Include unjustified absence";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(104, 46);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(303, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(104, 24);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(193, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(8, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(327, 216);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd.MM.yyyy";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(104, 187);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(160, 20);
            this.dtpEndDate.TabIndex = 11;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(10, 186);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(88, 23);
            this.lblEndDate.TabIndex = 10;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd.MM.yyyy";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpStartDate.Location = new System.Drawing.Point(104, 151);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(160, 20);
            this.dtpStartDate.TabIndex = 9;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(10, 150);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 23);
            this.lblStartDate.TabIndex = 8;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbAbsenceType
            // 
            this.cbAbsenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAbsenceType.Location = new System.Drawing.Point(104, 112);
            this.cbAbsenceType.Name = "cbAbsenceType";
            this.cbAbsenceType.Size = new System.Drawing.Size(160, 21);
            this.cbAbsenceType.TabIndex = 7;
            // 
            // lblAbsence
            // 
            this.lblAbsence.Location = new System.Drawing.Point(10, 110);
            this.lblAbsence.Name = "lblAbsence";
            this.lblAbsence.Size = new System.Drawing.Size(88, 23);
            this.lblAbsence.TabIndex = 6;
            this.lblAbsence.Text = "Absence Type:";
            this.lblAbsence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(104, 76);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 5;
            this.cbEmployee.Leave += new EventHandler(cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(24, 76);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvAbsences
            // 
            this.lvAbsences.FullRowSelect = true;
            this.lvAbsences.GridLines = true;
            this.lvAbsences.HideSelection = false;
            this.lvAbsences.Location = new System.Drawing.Point(16, 276);
            this.lvAbsences.Name = "lvAbsences";
            this.lvAbsences.ShowItemToolTips = true;
            this.lvAbsences.Size = new System.Drawing.Size(702, 192);
            this.lvAbsences.TabIndex = 15;
            this.lvAbsences.UseCompatibleStateImageBehavior = false;
            this.lvAbsences.View = System.Windows.Forms.View.Details;
            this.lvAbsences.SelectedIndexChanged += new System.EventHandler(this.lvAbsences_SelectedIndexChanged);
            this.lvAbsences.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAbsences_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 603);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 23;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(136, 603);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(245, 603);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(643, 603);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(686, 247);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(648, 247);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 13;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(562, 471);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(156, 16);
            this.lblTotal.TabIndex = 17;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(289, 30);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(76, 23);
            this.btnGenerate.TabIndex = 22;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.gbIncludeHolidays);
            this.gbReport.Controls.Add(this.btnGenerate);
            this.gbReport.Location = new System.Drawing.Point(16, 509);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(371, 69);
            this.gbReport.TabIndex = 18;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Report";
            // 
            // gbIncludeHolidays
            // 
            this.gbIncludeHolidays.Controls.Add(this.rbNo);
            this.gbIncludeHolidays.Controls.Add(this.rbYes);
            this.gbIncludeHolidays.Location = new System.Drawing.Point(6, 14);
            this.gbIncludeHolidays.Name = "gbIncludeHolidays";
            this.gbIncludeHolidays.Size = new System.Drawing.Size(277, 42);
            this.gbIncludeHolidays.TabIndex = 19;
            this.gbIncludeHolidays.TabStop = false;
            this.gbIncludeHolidays.Text = "Include holidays and weekends";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(107, 19);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 21;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(6, 19);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 20;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // lblOnlyVacation
            // 
            this.lblOnlyVacation.Location = new System.Drawing.Point(15, 472);
            this.lblOnlyVacation.Name = "lblOnlyVacation";
            this.lblOnlyVacation.Size = new System.Drawing.Size(277, 23);
            this.lblOnlyVacation.TabIndex = 16;
            this.lblOnlyVacation.Text = "*For vacation only";
            this.lblOnlyVacation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(439, 16);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 27;
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
            // EmployeeAbsences
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(730, 641);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblOnlyVacation);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvAbsences);
            this.Controls.Add(this.gbEmployeeAbsence);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "EmployeeAbsences";
            this.ShowInTaskbar = false;
            this.Text = " ";
            this.Load += new System.EventHandler(this.EmployeeAbsences_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAbsences_KeyUp);
            this.gbEmployeeAbsence.ResumeLayout(false);
            this.gbReport.ResumeLayout(false);
            this.gbIncludeHolidays.ResumeLayout(false);
            this.gbIncludeHolidays.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void cbEmployee_Leave(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
        
        }
        #endregion

        #region Inner Class for sorting Array List of Employees Absences

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort : IComparer<EmployeeAbsenceTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeAbsenceTO x, EmployeeAbsenceTO y)
            {
                EmployeeAbsenceTO empl1 = null;
                EmployeeAbsenceTO empl2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    empl1 = x;
                    empl2 = y;
                }
                else
                {
                    empl1 = y;
                    empl2 = x;
                }

                switch (compField)
                {
                    case EmployeeAbsences.EmployeeIDIndex:
                        return empl1.EmployeeName.CompareTo(empl2.EmployeeName);
                    case EmployeeAbsences.PassTypeIDIndex:
                        return empl1.PassType.CompareTo(empl2.PassType);
                    case EmployeeAbsences.DateStartIndex:
                        return empl1.DateStart.CompareTo(empl2.DateStart);
                    case EmployeeAbsences.DateEndIndex:
                        return empl1.DateEnd.CompareTo(empl2.DateEnd);
                    case EmployeeAbsences.UsedIndex:
                        return empl1.Used.CompareTo(empl2.Used);
                    case EmployeeAbsences.VacationYearIndex:
                        return empl1.VacationYear.CompareTo(empl2.VacationYear);
                    case EmployeeAbsences.DescriptionIndex:
                        return empl1.Description.CompareTo(empl2.Description);
                    default:
                        return empl1.DateStart.CompareTo(empl2.DateStart);
                }
            }
        }

        #endregion

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnGenerate.Text = rm.GetString("btnGenerate", culture);

                // Form name
                this.Text = rm.GetString("menuEmployeeAbsences", culture);

                // group box text
                this.gbEmployeeAbsence.Text = rm.GetString("gbEmployeeAbsences", culture);
                this.gbReport.Text = rm.GetString("gbReport", culture);
                this.gbIncludeHolidays.Text = rm.GetString("gbIncludeHolidays", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);


                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblAbsence.Text = rm.GetString("lblAbsence", culture);
                lblStartDate.Text = rm.GetString("lblStartDate", culture);
                lblEndDate.Text = rm.GetString("lblEndDate", culture);
                lblOnlyVacation.Text = rm.GetString("lblOnlyVacation", culture);

                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbIncludeUnjustified.Text = rm.GetString("chbIncludeUnjustified", culture);

                //radio button's text
                rbNo.Text = rm.GetString("no", culture);
                rbYes.Text = rm.GetString("yes", culture);

                // list view initialization
                lvAbsences.BeginUpdate();
                lvAbsences.Columns.Add(rm.GetString("hdrEmployee", culture), (lvAbsences.Width - 5) / 7, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrAbsID", culture), (lvAbsences.Width - 5) / 7, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrStartDate", culture), (lvAbsences.Width - 5) / 7-25, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrEndDate", culture), (lvAbsences.Width - 5) / 7-25, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrAbsUsed", culture), (lvAbsences.Width - 5) / 7-30, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrYear", culture)+"*", (lvAbsences.Width - 5) / 7-45, HorizontalAlignment.Left);
                lvAbsences.Columns.Add(rm.GetString("hdrDescription", culture), (lvAbsences.Width - 5) / 3-15, HorizontalAlignment.Left);
                lvAbsences.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currentEmplArray = new List<EmployeeTO>();

                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currentEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
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
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);

                cbEmployee.DataSource = currentEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Absence Type Combo Box
        /// </summary>
        private void populateAbsenceTypeCombo()
        {
            try
            {
                PassType pt = new PassType();
                pt.PTypeTO.IsPass = Constants.wholeDayAbsence;
                List<PassTypeTO> ptArray = pt.Search();
                ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

                cbAbsenceType.DataSource = ptArray;
                cbAbsenceType.DisplayMember = "Description";
                cbAbsenceType.ValueMember = "PassTypeID";
                cbAbsenceType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.populateAbsenceTypeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Employee Absences found
        /// </summary>
        /// <param name="locationsList"></param>
        private void populateListView(List<EmployeeAbsenceTO> emplAbsList, int startIndex)
        {
            try
            {
                if (emplAbsList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvAbsences.BeginUpdate();
                lvAbsences.Items.Clear();

                if (emplAbsList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < emplAbsList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= emplAbsList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = emplAbsList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            EmployeeAbsenceTO emplAbs = emplAbsList[i];
                            ListViewItem item = new ListViewItem();

                            if (emplAbs.EmployeeID != -1)
                            {
                                item.Text = emplAbs.EmployeeName;
                            }
                            else
                            {
                                item.Text = "";
                            }

                            if (emplAbs.PassTypeID != -1)
                            {
                                item.SubItems.Add(emplAbs.PassType);
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }

                            if (!emplAbs.DateStart.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(emplAbs.DateStart.Date.ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            if (!emplAbs.DateEnd.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(emplAbs.DateEnd.Date.ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            if (emplAbs.Used == (int)Constants.Used.No)
                            {
                                item.SubItems.Add(rm.GetString("no", culture));
                            }
                            else if (emplAbs.Used == (int)Constants.Used.Yes)
                            {
                                item.SubItems.Add(rm.GetString("yes", culture));
                            }
                            else if (emplAbs.Used == (int)Constants.Used.Error)
                            {
                                item.SubItems.Add(rm.GetString("error", culture));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            if (emplAbs.VacationYear != new DateTime())
                            {
                                item.SubItems.Add(emplAbs.VacationYear.ToString("yyyy"));
                            }
                            else
                            {
                                item.SubItems.Add("N/A");
                            }
                            item.SubItems.Add(emplAbs.Description.Replace('\n', ' ').Replace('\r',' '));
                            item.ToolTipText = emplAbs.Description;
                       
                            item.Tag = emplAbs;

                            lvAbsences.Items.Add(item);
                        }
                    }
                }

                lvAbsences.EndUpdate();
                lvAbsences.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Clears Form
        /// </summary>
        private void formClear()
        {
            try
            {
                this.cbWU.SelectedIndex = 0;
                this.cbEmployee.SelectedIndex = 0;
                this.cbAbsenceType.SelectedIndex = 0;
                this.dtpStartDate.Value = DateTime.Today;
                this.dtpEndDate.Value = DateTime.Today;
                this.lblTotal.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.formClear(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWU.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearListView()
        {
            lvAbsences.BeginUpdate();
            lvAbsences.Items.Clear();
            lvAbsences.EndUpdate();
            lvAbsences.Invalidate();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int employeeID = -1;
                int passTypeID = -1;

                if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                {
                    employeeID = (int)cbEmployee.SelectedValue;
                }

                if (cbAbsenceType.SelectedIndex > 0)
                {
                    passTypeID = (int)cbAbsenceType.SelectedValue;
                }

                string selectedWU = wuString;
                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                {
                    selectedWU = cbWU.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                    WorkingUnit wu = new WorkingUnit();
                    if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = wu.FindAllChildren(wuList);
                        selectedWU = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (selectedWU.Length > 0)
                        {
                            selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                        }
                    }
                }

                //ArrayList emplAbsList = currentEmplAbs.Search(employeeID, passTypeID, dtpStartDate.Value.Date.ToString(dateTimeformat), dtpEndDate.Value.Date.ToString(dateTimeformat));
                if (!wuString.Equals(""))
                {
                    if (cbEmployee.Items.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noEmplAbsFound", culture));
                        return;
                    }

                    //currentEmployeesAbsList = currentEmplAbs.Search(employeeID, passTypeID, dtpStartDate.Value.Date, dtpEndDate.Value.Date, "", wuString);
                    //Added SearchForAbsences function because Search algorithm is changed for Absences form, 
                    //and original Search function is used in IOPair, for insertWholeDayAbsences
                    EmployeeAbsence ea = new EmployeeAbsence();
                    ea.EmplAbsTO.EmployeeID = employeeID;
                    ea.EmplAbsTO.PassTypeID = passTypeID;
                    ea.EmplAbsTO.DateStart = dtpStartDate.Value.Date;
                    ea.EmplAbsTO.DateEnd = dtpEndDate.Value.Date;
                    currentEmployeesAbsList = ea.SearchForAbsences(selectedWU);
                    if (chbIncludeUnjustified.Checked)
                        currentEmployeesAbsList.AddRange(getAbsences(employeeID, selectedWU));
                    if (currentEmployeesAbsList.Count > 0)
                    {
                        currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                        startIndex = 0;
                        populateListView(currentEmployeesAbsList, startIndex);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesAbsList.Count.ToString().Trim();
                    }
                    else
                    {
                        //MessageBox.Show(rm.GetString("noEmplAbsFound", culture));
                        clearListView();
                        btnPrev.Visible = false;
                        btnNext.Visible = false;
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplAbsPrivilege", culture));
                }

                currentEmplAbs = new EmployeeAbsenceTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private List<EmployeeAbsenceTO> getAbsences(int employeeID, string selectedWU)
        {
            List<EmployeeAbsenceTO> absences = new List<EmployeeAbsenceTO>();
            
            try
            {
                //get IOPairs for employees and period
                IOPair iopair = new IOPair();
                iopair.PairTO.StartTime = dtpStartDate.Value.Date;
                iopair.PairTO.EndTime = dtpEndDate.Value.Date;
                iopair.PairTO.EmployeeID = employeeID;
                List<IOPairTO> pairsList = iopair.SearchWithType(dtpStartDate.Value.Date, dtpEndDate.Value.Date, selectedWU, -1);

                //key is employeeID, value is hashtable
                Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> employeesTable = new Dictionary<int,Dictionary<DateTime,List<IOPairTO>>>();

                foreach (IOPairTO pair in pairsList)
                {
                    if (!employeesTable.ContainsKey(pair.EmployeeID))
                    {
                        employeesTable.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());
                    }
                    if (!employeesTable[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                    {
                        employeesTable[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairTO>());
                    }
                    employeesTable[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                //all selected employees
                List<EmployeeTO> emplList = new List<EmployeeTO>();
                if (employeeID != -1)
                {
                    foreach (EmployeeTO empl in currentEmplArray)
                    {
                        if (empl.EmployeeID == employeeID)
                            emplList.Add(empl);
                    }
                }
                else
                {
                    emplList = currentEmplArray;
                }
                //currentNOIOPairsList = new ArrayList();

                foreach (EmployeeTO employee in emplList)
                {
                    if (employee.EmployeeID == -1)
                        continue;
                    List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                    if (!employeesTable.ContainsKey(employee.EmployeeID))
                    {
                        EmployeeAbsenceTO iop = new EmployeeAbsenceTO();
                        for (DateTime date = dtpStartDate.Value.Date; date <= dtpEndDate.Value.Date; date = date.AddDays(1))
                        {
                            if ((!isWeekend(date) && !isHoliday(date)) && isWorkingDay(date, emplDaySchedules, employee))
                            {
                                if (iop.PassTypeID < 0)
                                {
                                    iop.EmployeeID = employee.EmployeeID;
                                    iop.EmployeeName = employee.LastName;
                                    iop.PassType = absence;
                                    iop.PassTypeID = 100000;
                                    iop.DateStart = date;
                                    iop.DateEnd = date;
                                    //currentNOIOPairsList.Add(iop);
                                }
                                if (date == dtpEndDate.Value.Date)
                                {
                                    if (iop.PassTypeID > 0)
                                    {
                                        iop.DateEnd = date;
                                        absences.Add(iop);
                                    }
                                    iop = new EmployeeAbsenceTO();
                                }
                            }
                            else
                            {
                                if (iop.PassTypeID > 0)
                                {
                                    iop.DateEnd = date.AddDays(-1);
                                    absences.Add(iop);
                                }
                                iop = new EmployeeAbsenceTO();
                            }
                        }
                    }
                    else
                    {
                        EmployeeAbsenceTO iop = new EmployeeAbsenceTO();
                        for (DateTime date = dtpStartDate.Value.Date; date <= dtpEndDate.Value.Date; date = date.AddDays(1))
                        {
                            if (!employeesTable[employee.EmployeeID].ContainsKey(date))
                            {
                                if ((!isWeekend(date) && !isHoliday(date)) && isWorkingDay(date, emplDaySchedules, employee))
                                {
                                    if (iop.PassTypeID < 0)
                                    {
                                        iop.EmployeeID = employee.EmployeeID;
                                        iop.EmployeeName = employee.LastName;
                                        iop.PassType = absence;
                                        iop.PassTypeID = 100000;
                                        iop.DateStart = date;
                                        iop.DateEnd = date;
                                        //currentNOIOPairsList.Add(iop);
                                    }
                                    if (date == dtpEndDate.Value.Date)
                                    {
                                        if (iop.PassTypeID > 0)
                                        {
                                            iop.DateEnd = date;
                                            absences.Add(iop);
                                        }
                                        iop = new EmployeeAbsenceTO();
                                    }
                                    //absences.Add(iop);
                                }
                                else
                                {
                                    if (iop.PassTypeID > 0)
                                    {
                                        iop.DateEnd = date.AddDays(-1);
                                        absences.Add(iop);
                                    }
                                    iop = new EmployeeAbsenceTO();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
              
            }
            return absences;
        }

        private bool isWorkingDay(DateTime day1, List<EmployeeTimeScheduleTO> emplDaySchedules, EmployeeTO currentEmployee)
        {
            bool isWorkingDay = false;
            try
            {
                //TimeSpan shiftTotal = new TimeSpan();               

                TimeSpan shift = new TimeSpan();
                bool is2DayShift = false;
                bool is2DaysShiftPrevious = false;
                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                //get intervls for employee and day
                Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day1, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                Employee empl = new Employee();
                empl.EmplTO = currentEmployee;
                ArrayList schemas = empl.findTimeSchema(day1);
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
                
                if (shift.TotalMinutes > 0)
                    isWorkingDay = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.isWorkingDay(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isWorkingDay;
        }

        private void lvAbsences_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                //if (lvAbsences.SelectedItems.Count > 0)
                //{
                //    EmployeeAbsenceTO emplAbsTO = (EmployeeAbsenceTO)lvAbsences.SelectedItems[0].Tag;
                //    if (emplAbsTO.RecID >= 0)
                //    {
                //        cbEmployee.SelectedIndex = cbEmployee.FindStringExact(lvAbsences.SelectedItems[0].Text);
                //        cbAbsenceType.SelectedIndex = cbAbsenceType.FindStringExact(lvAbsences.SelectedItems[0].SubItems[1].Text);
                //        dtpStartDate.Value = emplAbsTO.DateStart;
                //        dtpEndDate.Value = emplAbsTO.DateEnd;
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.lvAbsences_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvAbsences_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesAbsList, startIndex);
            }
                
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.lvAbsences_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvAbsences.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelAbsDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteEmplAbsences", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        foreach (ListViewItem item in lvAbsences.SelectedItems)
                        {
                            if (!item.SubItems[1].Text.ToString().Equals(absence))
                                isDeleted = new EmployeeAbsence().DeleteEADeleteIOP(((EmployeeAbsenceTO)item.Tag).RecID) && isDeleted;
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("AbsencesDel", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noAbsDel", culture));
                        }

                        formClear();
                        btnSearch_Click(sender, e);
                        /*if (!wuString.Equals(""))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            currentEmployeesAbsList = currentEmplAbs.Search("", "", new DateTime(), new DateTime(), "", wuString);
                            startIndex = 0;
                            currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                            populateListView(currentEmployeesAbsList, startIndex);
                            formClear();
                            this.Cursor = Cursors.Arrow;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noEmplAbsPrivilege", culture));
                        }*/
                    }
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                EmployeeAbsencesAdd emplAbsAdd = new EmployeeAbsencesAdd();
                emplAbsAdd.ShowDialog(this);

                //Reload form only if some absence is added
                if (emplAbsAdd.doReloadOnBack)
                {
                    formClear();
                    btnSearch_Click(sender, e);
                }
                /*if (!wuString.Equals(""))
                {
                    this.Cursor = Cursors.WaitCursor;
                    currentEmployeesAbsList = currentEmplAbs.Search("", "", new DateTime(), new DateTime(), "", wuString);
                    startIndex = 0;
                    currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentEmployeesAbsList, startIndex);
                    formClear();
                    this.Cursor = Cursors.Arrow;
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplAbsPrivilege", culture));
                }*/
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvAbsences.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selectOneAbs", culture));
                }
                else
                {                    
                    if (!lvAbsences.SelectedItems[0].SubItems[1].Text.ToString().Trim().Equals(absence))
                    {
                        currentEmplAbs = (EmployeeAbsenceTO)lvAbsences.SelectedItems[0].Tag;
                        EmployeeAbsencesAdd emplAbsAdd = new EmployeeAbsencesAdd(currentEmplAbs);
                        emplAbsAdd.ShowDialog(this);

                        //Reload form only if some absence is updated
                        if (emplAbsAdd.doReloadOnBack)
                        {
                            formClear();
                            btnSearch_Click(sender, e);
                        }
                        /*if (!wuString.Equals(""))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            currentEmployeesAbsList = currentEmplAbs.Search("", "", new DateTime(), new DateTime(), "", wuString);
                            startIndex = 0;
                            currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                            populateListView(currentEmployeesAbsList, startIndex);
                            formClear();
                            this.Cursor = Cursors.Arrow;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noEmplAbsPrivilege", culture));
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permission;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
                }

                btnSearch.Enabled = readPermission;
                btnAdd.Enabled = addPermission;
                btnUpdate.Enabled = updatePermission;
                btnDelete.Enabled = deletePermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrev_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateListView(currentEmployeesAbsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateListView(currentEmployeesAbsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeeAbsences_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmployeeAbsences.EmployeeAbsences_KeyUp(): " + ex.Message + "\n");
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
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (currentEmployeesAbsList.Count > 0)
                {
                    
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_absences");
                        DataTable tableCR1 = new DataTable("total_absences");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("employee_name", typeof(System.String));
                        tableCR.Columns.Add("absence_type", typeof(System.String));
                        tableCR.Columns.Add("processed", typeof(System.String));
                        tableCR.Columns.Add("start_time", typeof(System.String));
                        tableCR.Columns.Add("end_time", typeof(System.String));
                        tableCR.Columns.Add("desc", typeof(System.String));
                        tableCR.Columns.Add("days", typeof(System.Int64));
                        tableCR.Columns.Add("totalDays", typeof(System.Int64));
                        tableCR.Columns.Add("imageID", typeof(byte));

                        tableCR1.Columns.Add("type_name", typeof(System.String));
                        tableCR1.Columns.Add("total", typeof(System.Int32));
                        tableCR1.Columns.Add("date", typeof(System.DateTime));
                        tableCR1.Columns.Add("imageID", typeof(byte));

                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableCR1);
                        dataSetCR.Tables.Add(tableI);

                        Hashtable TotalForDays = new Hashtable();
                        string totals = "";
                    
                        foreach (EmployeeAbsenceTO employeeAbsence in currentEmployeesAbsList)
                        {
                           
                            DataRow row = tableCR.NewRow();
                            
                            row["employee_name"] = employeeAbsence.EmployeeName;
                            row["absence_type"] = employeeAbsence.PassType;
                            DateTime start = new DateTime();
                            DateTime end = new DateTime();
                            if (employeeAbsence.DateStart > dtpStartDate.Value.Date)
                            {
                                start = employeeAbsence.DateStart;
                            }
                            else
                            {
                                start = dtpStartDate.Value.Date;
                            }
                            if (employeeAbsence.DateEnd > dtpEndDate.Value.Date)
                            {
                                end = dtpEndDate.Value.Date;
                            }
                            else
                            {
                                end = employeeAbsence.DateEnd;
                            }
                            for (DateTime date = dtpStartDate.Value.Date; date <= dtpEndDate.Value.Date; date = date.AddDays(1))
                            {
                                if(!TotalForDays.ContainsKey(date))
                                    TotalForDays.Add(date,new Hashtable());
                                if(!((Hashtable)TotalForDays[date]).ContainsKey(employeeAbsence.PassType))
                                {
                                    ((Hashtable)TotalForDays[date]).Add(employeeAbsence.PassType,0);
                                }
                                if (date.Date <= end.Date && date >= start.Date)
                                {
                                    ((Hashtable)TotalForDays[date])[employeeAbsence.PassType] = ((int)((Hashtable)TotalForDays[date])[employeeAbsence.PassType])+1;
                                }

                            }
                            row["start_time"] = employeeAbsence.DateStart.ToString("dd.MM.yyyy");
                            row["end_time"] = employeeAbsence.DateEnd.ToString("dd.MM.yyyy");
                            TimeSpan span = employeeAbsence.DateEnd.AddDays(1) - employeeAbsence.DateStart;
                            int num = (int)span.TotalDays;
                            if (rbYes.Checked)
                            {
                                num -= getNumberOfNotWorkingDays(employeeAbsence.DateStart, employeeAbsence.DateEnd);
                            }
                            row["days"] = num;
                            TimeSpan span1 = end.AddDays(1) - start;
                            int num1 = (int)span1.TotalDays;
                            if (rbYes.Checked)
                            {
                                num1 -= getNumberOfNotWorkingDays(start, end);
                            }
                            row["totalDays"] = num1;
                            if (employeeAbsence.Used == (int)Constants.Used.No)
                            {
                                row["processed"] = rm.GetString("no", culture);
                            }
                            else if (employeeAbsence.Used == (int)Constants.Used.Yes)
                            {
                                row["processed"] = rm.GetString("yes", culture);
                            }
                            row["desc"] = employeeAbsence.Description.Replace('\n', ' ').Replace('\r', ' ');
                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        foreach (DateTime date in TotalForDays.Keys)
                        {
                            
                            //totals +=date.ToString("dd.MM.yyyy");
                            foreach (String type in ((Hashtable)TotalForDays[date]).Keys)
                            {
                                DataRow row = tableCR1.NewRow();
                                row["date"] = date;
                                row["type_name"] = type;
                                row["total"] = (int)(((Hashtable)TotalForDays[date])[type]);

                                tableCR1.Rows.Add(row);
                                tableCR1.AcceptChanges();
                                //totals += "                       " + type+":     ";
                                //totals += ((int)(((Hashtable)TotalForDays[date])[type])).ToString();
                            }
                           
                        }

                            if (tableCR.Rows.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                                return;
                            }

                            string selAbsenceType = "*";
                            string selWorkingUnit = "*";
                            string selEmplName = "*";
                            string lblFootNote = "*";

                            if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                                selWorkingUnit = cbWU.Text;
                            if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                                selEmplName = cbEmployee.Text;
                            if (cbAbsenceType.SelectedIndex >= 0)
                                selAbsenceType = cbAbsenceType.Text;
                            if (rbYes.Checked)
                            {
                                lblFootNote = rm.GetString("emplAbsenceFootNoteYes", culture);
                            }
                            else
                            {
                                lblFootNote = rm.GetString("emplAbsenceFootNoteNo", culture);
                            }


                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports.Reports_sr.EmployeeAbsencesCRView view = new Reports.Reports_sr.EmployeeAbsencesCRView(dataSetCR,
                                     selWorkingUnit, selEmplName, selAbsenceType,lblFootNote, dtpStartDate.Value, dtpEndDate.Value, logInUser.Name, totals);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports.Reports_en.EmployeeAbsencesCRView_en view = new Reports.Reports_en.EmployeeAbsencesCRView_en(dataSetCR,
                                     selWorkingUnit, selEmplName, selAbsenceType,lblFootNote, dtpStartDate.Value, dtpEndDate.Value, logInUser.Name, totals);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                            {
                                Reports.Reports_fi.EmployeeAbsencesCRView_fi view = new Reports.Reports_fi.EmployeeAbsencesCRView_fi(dataSetCR,
                                     selWorkingUnit, selEmplName, selAbsenceType,lblFootNote, dtpStartDate.Value, dtpEndDate.Value, logInUser.Name, totals);
                                view.ShowDialog(this);
                            }
                        
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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

        private int getNumberOfNotWorkingDays(DateTime dateStart, DateTime dateEnd)
        {
            int num = 0;
            try
            {
                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (isHoliday(date) || isWeekend(date))
                        num++;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.getNumberOfNotWorkingDays(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return num;
        }

        private void EmployeeAbsences_Load(object sender, EventArgs e)
        {
            try
            {
                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                rbNo.Checked = true;
                this.Cursor = Cursors.WaitCursor;
                wUnits = new List<WorkingUnitTO>();

                sortOrder = Constants.sortDesc;
                sortField = EmployeeAbsences.DateStartIndex;
                startIndex = 0;
                this.lblTotal.Visible = false;

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.AbsencesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);

                timeSchemas = new TimeSchema().Search();

                btnSearch_Click(this, new EventArgs());
                /*if (!wuString.Equals(""))
                {
                    currentEmployeesAbsList = currentEmplAbs.Search("", "", new DateTime(0), new DateTime(0), "", wuString);
                    currentEmployeesAbsList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentEmployeesAbsList, startIndex);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplAbsPrivilege", culture));
                }*/

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.EmployeeAbsences_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAbsences.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        
    }
}
