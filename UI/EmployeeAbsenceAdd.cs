using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;

using System.Data;
using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for EmployeeAbsences.
	/// </summary>
	public class EmployeeAbsencesAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.Label lblAbsence;
		private System.Windows.Forms.ComboBox cbAbsenceType;
		private System.Windows.Forms.Label lblStartDate;
		private System.Windows.Forms.DateTimePicker dtpStartDate;
		private System.Windows.Forms.Label lblEndDate;
		private System.Windows.Forms.DateTimePicker dtpEndDate;
		private System.Windows.Forms.Button btnUpdate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EmployeeAbsenceTO currentEmplAbs = null;

		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		DebugLog log;
		ApplUserTO logInUser;

		private List<WorkingUnitTO> wUnits;
		private string wuString = "";
        private Button btnWUTree;
        private CheckBox chbHierarhicly;

		//Indicate if calling form needs to be reload
		public bool doReloadOnBack = true;

        // if licence consist vacation evidence 
        private bool vacationLicence = false;
        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();
        private Button btnDetailes;
        private TextBox tbDesc;
        private Label lblDesc;
        string intervalsString = "";

        List<EmployeeTO> emplArray;

        bool GSKLicence = false;
        bool selfServ = false;

		public EmployeeAbsencesAdd()
		{
			InitializeComponent();

			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(EmployeeAbsences).Assembly);
			currentEmplAbs = new EmployeeAbsenceTO();

			logInUser = NotificationController.GetLogInUser();

			setLanguage();

			wUnits = new List<WorkingUnitTO>();
				
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
            //btnPrintRequest.Visible = false;
			populateWorkingUnitCombo();
			populateEmployeeCombo(-1);
			populateAbsenceTypeCombo();

			this.btnUpdate.Visible = false;
		}

        public EmployeeAbsencesAdd(EmployeeTO empl)
        {
            InitializeComponent();
            selfServ = true;
            this.CenterToScreen();

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }

            // get all time schemas
            timeSchemas = new TimeSchema().Search();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
            currentEmplAbs = new EmployeeAbsenceTO();

            logInUser = NotificationController.GetLogInUser();

            setLanguage();

            wUnits = new List<WorkingUnitTO>();

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
            try
            {
                cbEmployee.SelectedValue = empl.EmployeeID;
                cbWU.Enabled = false;
                cbEmployee.Enabled = false;
            }
            catch 
            {
                rm.GetString("UnkonwnEmpl", culture);
            }

            populateAbsenceTypeCombo();

            this.btnUpdate.Visible = false;
        }

        public EmployeeAbsencesAdd(EmployeeVacEvidTO vacation)
        {
            vacationLicence = true;
            InitializeComponent();
            
            this.CenterToScreen();

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }

            // get all time schemas
            timeSchemas = new TimeSchema().Search();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
            currentEmplAbs = new EmployeeAbsenceTO();

            logInUser = NotificationController.GetLogInUser();

            setLanguage();

            wUnits = new List<WorkingUnitTO>();

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
            populateAbsenceTypeCombo();

            this.cbAbsenceType.Enabled = false;
            if (vacation != new EmployeeVacEvidTO())
            {
                this.cbWU.SelectedValue = vacation.WorkingUnitID;
                this.cbEmployee.SelectedValue = vacation.EmployeeID;
            }
            this.cbAbsenceType.SelectedValue = Constants.vacation;
            label1.Visible = false;
            this.btnUpdate.Visible = false;
           // btnPrintRequest.Visible = false;
        }

		public EmployeeAbsencesAdd(EmployeeAbsenceTO emplAbs)
		{
			InitializeComponent();

			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(EmployeeAbsences).Assembly);
			currentEmplAbs = new EmployeeAbsenceTO(emplAbs.RecID, emplAbs.EmployeeID, emplAbs.PassTypeID,
				emplAbs.DateStart, emplAbs.DateEnd, emplAbs.Used);

			logInUser = NotificationController.GetLogInUser();

			setLanguage();

			wUnits = new List<WorkingUnitTO>();
				
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
			populateAbsenceTypeCombo();
			populateUpdateForm();

            this.tbDesc.Text = emplAbs.Description;

			this.btnSave.Visible = false;
			this.cbWU.Enabled = false;
			this.cbEmployee.Enabled = false;
            this.btnWUTree.Enabled = false;
            this.chbHierarhicly.Enabled = false;
           // btnPrintRequest.Visible = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeAbsencesAdd));
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.cbAbsenceType = new System.Windows.Forms.ComboBox();
            this.lblAbsence = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnDetailes = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(112, 16);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(184, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 16);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd.MM.yyyy";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(394, 113);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(160, 20);
            this.dtpEndDate.TabIndex = 9;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(316, 110);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(72, 23);
            this.lblEndDate.TabIndex = 8;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd.MM.yyyy";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(112, 113);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(147, 20);
            this.dtpStartDate.TabIndex = 7;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(18, 110);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 23);
            this.lblStartDate.TabIndex = 6;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbAbsenceType
            // 
            this.cbAbsenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAbsenceType.Location = new System.Drawing.Point(112, 75);
            this.cbAbsenceType.Name = "cbAbsenceType";
            this.cbAbsenceType.Size = new System.Drawing.Size(147, 21);
            this.cbAbsenceType.TabIndex = 5;
            this.cbAbsenceType.SelectedIndexChanged += new System.EventHandler(this.cbAbsenceType_SelectedIndexChanged);
            // 
            // lblAbsence
            // 
            this.lblAbsence.Location = new System.Drawing.Point(21, 73);
            this.lblAbsence.Name = "lblAbsence";
            this.lblAbsence.Size = new System.Drawing.Size(88, 23);
            this.lblAbsence.TabIndex = 4;
            this.lblAbsence.Text = "Absence Type:";
            this.lblAbsence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(392, 16);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 3;
            this.cbEmployee.Leave += new EventHandler(cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(320, 16);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(64, 23);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 223);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(24, 223);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(479, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(280, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 32;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(280, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(560, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(560, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(302, 14);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 40;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(112, 43);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 43;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnDetailes
            // 
            this.btnDetailes.Location = new System.Drawing.Point(105, 223);
            this.btnDetailes.Name = "btnDetailes";
            this.btnDetailes.Size = new System.Drawing.Size(75, 23);
            this.btnDetailes.TabIndex = 44;
            this.btnDetailes.Text = "Detailes";
            this.btnDetailes.Visible = false;
            this.btnDetailes.Click += new System.EventHandler(this.btnDetailes_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(112, 149);
            this.tbDesc.MaxLength = 500;
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(442, 56);
            this.tbDesc.TabIndex = 46;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(6, 147);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(100, 23);
            this.lblDesc.TabIndex = 45;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EmployeeAbsencesAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(576, 258);
            this.ControlBox = false;
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.btnDetailes);
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbWU);
            this.Controls.Add(this.lblWU);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.cbAbsenceType);
            this.Controls.Add(this.lblAbsence);
            this.Controls.Add(this.cbEmployee);
            this.Controls.Add(this.lblEmployee);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "EmployeeAbsencesAdd";
            this.ShowInTaskbar = false;
            this.Text = "EmployeeAbsences";
            this.Load += new System.EventHandler(this.EmployeeAbsencesAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAbsencesAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void cbEmployee_Leave(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
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
				// Form name
				if (currentEmplAbs.RecID < 0)
				{
					this.Text = rm.GetString("menuEmployeeAbsencesAdd", culture);
				}
				else
				{
					this.Text = rm.GetString("menuEmployeeAbsencesUpdate", culture);
				}

                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                btnDetailes.Text = rm.GetString("btnDetailes", culture);
               // btnPrintRequest.Text = rm.GetString("btnPrintRequest", culture);
				
				// label's text
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblAbsence.Text = rm.GetString("lblAbsence", culture);
				lblStartDate.Text = rm.GetString("lblStartDate", culture);
				lblEndDate.Text = rm.GetString("lblEndDate", culture);
                lblDesc.Text = rm.GetString("lblDesc", culture);
			}
			catch(Exception ex)
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
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsences.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Employee Combo Box
		/// </summary>
		private void populateEmployeeCombo(int wuID)
		{
			try
			{
				emplArray = new List<EmployeeTO>();

                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().SearchByWU(wuString);
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
                    emplArray = new Employee().SearchByWU(workUnitID);
                }

				foreach(EmployeeTO employee in emplArray)
				{
					employee.LastName += " " + employee.FirstName;
				}

				EmployeeTO empl = new EmployeeTO();
				empl.LastName = rm.GetString("all", culture);
				emplArray.Insert(0, empl);
								
				cbEmployee.DataSource = emplArray;
				cbEmployee.DisplayMember = "LastName";
				cbEmployee.ValueMember = "EmployeeID";
				cbEmployee.Invalidate();
			}
			catch(Exception ex)
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
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsences.populateAbsenceTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateUpdateForm()
		{
			try
			{
				cbEmployee.SelectedValue = currentEmplAbs.EmployeeID;
				cbAbsenceType.SelectedValue = currentEmplAbs.PassTypeID;
				dtpStartDate.Value = currentEmplAbs.DateStart;
				dtpEndDate.Value = currentEmplAbs.DateEnd;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsences.populateUpdateForm(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            try
            {
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
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Do not reload calling form on cancel
                doReloadOnBack = false;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsences.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
                intervalsString = "";
				if (cbEmployee.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("absNoEmpl", culture));
					return;
				}
				if (cbAbsenceType.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("absNoAbsType", culture));
					return;
				}
				if (dtpStartDate.Value.Equals(new DateTime(0)))
				{
					MessageBox.Show(rm.GetString("absNoStartDate", culture));
					return;
				}
				if (dtpStartDate.Value.Equals(new DateTime(0)))
				{
					MessageBox.Show(rm.GetString("absNoEndDate", culture));
					return;
				}
				if (dtpStartDate.Value.Date > dtpEndDate.Value.Date)
				{
					MessageBox.Show(rm.GetString("startGreaterThenEnd", culture));
					return;
				}
				
				currentEmplAbs.EmployeeID = (int) cbEmployee.SelectedValue;
				currentEmplAbs.PassTypeID = (int) cbAbsenceType.SelectedValue;
				currentEmplAbs.DateStart = dtpStartDate.Value;
				currentEmplAbs.DateEnd = dtpEndDate.Value;

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO = currentEmplAbs;
				List<EmployeeAbsenceTO> existingAbsences = ea.Search(wuString);

				if (existingAbsences.Count > 0)
				{
					MessageBox.Show(rm.GetString("absExists", culture));
				}
				else
				{                    
                    //11.02.2009 Natasa if form is for using Vacation check if employee has enought days
                    if (vacationLicence&&cbAbsenceType.SelectedValue.Equals(Constants.vacation))
                    {
                        bool saved = saveVacation();
                        if (selfServ)
                        {
                            int employeeID = (int)cbEmployee.SelectedValue;
                            DateTime selYear = new DateTime(dtpEndDate.Value.Year, 1, 1);
                            List<EmployeeVacEvidTO> vacations = new EmployeeVacEvid().getVacations(employeeID.ToString(), selYear, selYear, -1, -1);
                            VacationDetails details = null;// = new VacationDetails(new EmployeeVacEvid());
                            if (vacations.Count > 0)
                            {
                                details = new VacationDetails(vacations[0]);
                                //details.PrintRequest = true;
                                details.ShowDialog(this);
                            }
                        }
                        if (saved)
                        {
                            MessageBox.Show(rm.GetString("absInserted", culture));
                            this.Close();
                        }
                    }
                    else
                    {
                        int inserted = new EmployeeAbsence().Save(currentEmplAbs.EmployeeID, currentEmplAbs.PassTypeID,
                            currentEmplAbs.DateStart.Date, currentEmplAbs.DateEnd.Date, (int)Constants.Used.No, currentEmplAbs.VacationYear, tbDesc.Text.ToString().Trim());
                        if (inserted == 1)
                        {
                            MessageBox.Show(rm.GetString("absInserted", culture));
                            this.Close();
                        }
                    }
				}
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " EmployeeAbsence.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}
        
        //if licence include VacationEvidence this funkction is validating absence with plan from VacationEvidence
        //and saving vacation if it is valid
        private bool saveVacation()
        {
            bool saved = false;
            try
            {
                ArrayList vacations = new ArrayList();
                EmployeeVacEvid vacation = new EmployeeVacEvid();

                //if interval is in two diferent years show message
                if (dtpStartDate.Value.Year != dtpEndDate.Value.Year)
                {
                    MessageBox.Show(rm.GetString("inTwoYears", culture));
                    return false;
                }
                DateTime selectedYear = new DateTime(dtpStartDate.Value.Year, 1, 1, 0, 0, 0);

                int count = new EmployeeVacEvid().getVacationsCount(currentEmplAbs.EmployeeID.ToString(), selectedYear, selectedYear.AddYears(1), -1, -1);
                if (count <= 0)
                {
                    MessageBox.Show(rm.GetString("noVacEvidence", culture));
                    return false;
                }

                //list of vacationEvidences for selected employees and period
                List<EmployeeVacEvidTO> vactionlist = new List<EmployeeVacEvidTO>();
                vactionlist = new EmployeeVacEvid().getVacations(currentEmplAbs.EmployeeID.ToString(), selectedYear.AddYears(-1), selectedYear, -1, -1);

                //creating hashtable where key is vacation year and value is vacation 
                Dictionary<DateTime, EmployeeVacEvidTO> vacTable = new Dictionary<DateTime,EmployeeVacEvidTO>();
                foreach (EmployeeVacEvidTO vac in vactionlist)
                {
                    vacTable.Add(vac.VacYear, vac);
                }

                //if there is no vacations for selected year return false
                if (!vacTable.ContainsKey(selectedYear))
                {
                    MessageBox.Show(rm.GetString("noVacEvidence", culture));
                    return false;
                }

                //selected year vacation
                EmployeeVacEvidTO selYearVac = vacTable[selectedYear];

                //find all schedules for selected year and check if selected interval for absence is in schedule interval
                List<EmployeeVacEvidScheduleTO> vacationSchedules = vacation.getVacationSchedules(selYearVac.EmployeeID, selectedYear);
                bool schExist = false;
                foreach (EmployeeVacEvidScheduleTO schedule in vacationSchedules)
                {
                    intervalsString += "\n" + schedule.StartDate.ToString("dd.MM.yyyy") + " - " + schedule.EndDate.ToString("dd.MM.yyyy");
                    if (schedule.StartDate.Date <= dtpStartDate.Value.Date && schedule.EndDate.Date >= dtpEndDate.Value.Date)
                        schExist = true;
                }

                //if interval doesn't match with plan return false
                if (!schExist)
                {
                    MessageBox.Show(rm.GetString("noVacationMatch", culture) + intervalsString);
                    return schExist;
                }

                List<EmployeeTimeScheduleTO> emplSchedules = Common.Misc.GetEmployeeTimeSchedules(currentEmplAbs.EmployeeID, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                //count working days in selected interval for absence
                int usingDays = Common.Misc.getNumOfWorkingDays(emplSchedules, dtpStartDate.Value.Date, dtpEndDate.Value.Date, holidays, timeSchemas);

                //getting employeeAbsences  with passType = vacation to calculate used vacations for each employee
                EmployeeAbsence ea = new EmployeeAbsence();                
                ea.EmplAbsTO.PassTypeID = Constants.vacation;
                List<EmployeeAbsenceTO> absencesList = ea.SearchForVacEvid(currentEmplAbs.EmployeeID.ToString(), "", selectedYear.AddYears(-1), selectedYear.AddYears(1));

                //check if we can use vacation plan from year before
                bool noDaysFromYearBefore = false;

                //if there is no plan for year before at all pass to checking plan for selected year
                if (!vacTable.ContainsKey(selectedYear.AddYears(-1)))
                {
                    noDaysFromYearBefore = true;
                }
                else
                {
                    EmployeeVacEvidTO yearBeforeVacEvid = vacTable[selectedYear.AddYears(-1)];

                    //if plan for year before is no longer valid pass to checking plan for selected year
                    if (yearBeforeVacEvid.ValidTo < dtpStartDate.Value.Date)
                        noDaysFromYearBefore = true;

                    else
                    {
                        //from year vacation plan and absences conunt real used days and total time to use
                        yearBeforeVacEvid = Common.Misc.getVacationWithAbsences(yearBeforeVacEvid, absencesList, holidays, timeSchemas);

                        //if days left is 0 pass to checking plan for selected year
                        if (yearBeforeVacEvid.DaysLeft <= 0)
                            noDaysFromYearBefore = true;

                        //if there is days left from year before in this year 
                        //we can use minimum of days left from year before and duration from vacation start time to valid to date
                        else
                        {
                            int workingDays = Common.Misc.getNumOfWorkingDays(emplSchedules, dtpStartDate.Value.Date, yearBeforeVacEvid.ValidTo.Date, holidays, timeSchemas);
                            selYearVac.FromLastYear = Math.Min(yearBeforeVacEvid.DaysLeft, workingDays);
                           
                            //if days from last year is enough days for selected period just save absence related on last yaer
                            if (selYearVac.FromLastYear >= usingDays)
                            {
                                int inserted = new EmployeeAbsence().Save(currentEmplAbs.EmployeeID, currentEmplAbs.PassTypeID,
                                        currentEmplAbs.DateStart.Date, currentEmplAbs.DateEnd.Date, (int)Constants.Used.No, yearBeforeVacEvid.VacYear,tbDesc.Text.ToString().Trim());
                                if (inserted == 1)
                                {
                                    saved = true;
                                }
                            }
                        }
                    }
                }
                if (!saved)
                {
                    //getting days left for selected year
                    selYearVac = Common.Misc.getVacationWithAbsences(selYearVac, absencesList, holidays, timeSchemas);

                    //if days from last year is 0 check for days left in selected year and save absence
                    if (noDaysFromYearBefore || selYearVac.FromLastYear == 0)
                    {
                        //if not enough days left show message
                        if (selYearVac.DaysLeft < usingDays)
                        {
                            MessageBox.Show(rm.GetString("notEnoughVacDays", culture));
                            return false;
                        }
                        //insert just one absence related on selected year
                        int inserted = new EmployeeAbsence().Save(currentEmplAbs.EmployeeID, currentEmplAbs.PassTypeID,
                             currentEmplAbs.DateStart.Date, currentEmplAbs.DateEnd.Date, (int)Constants.Used.No, selYearVac.VacYear, tbDesc.Text.ToString().Trim());
                        if (inserted == 1)
                        {
                            saved = true;
                        }
                    }
                    else
                    {
                        //whan days from last year exist to save absence days left and from days need to be more than num of working days in sel interval
                        if ((selYearVac.DaysLeft + selYearVac.FromLastYear) < usingDays)
                        {
                            MessageBox.Show(rm.GetString("notEnoughVacDays", culture));
                            return false;
                        }
                        //get end date for absence related on last year
                        DateTime lastYearEndDate = Common.Misc.getDayIntervalStart(emplSchedules, dtpStartDate.Value.Date, dtpEndDate.Value.Date, selYearVac.FromLastYear, holidays, timeSchemas);
                        EmployeeAbsence emplAbs = new EmployeeAbsence();

                        if (emplAbs.BeginTransaction())
                        {
                            try
                            {
                                int inserted = emplAbs.Save(currentEmplAbs.EmployeeID, currentEmplAbs.PassTypeID,
                                        currentEmplAbs.DateStart.Date, lastYearEndDate, (int)Constants.Used.No, selYearVac.VacYear.AddYears(-1), false);
                                if (inserted > 0)
                                {
                                    int insertSelYear = emplAbs.Save(currentEmplAbs.EmployeeID, currentEmplAbs.PassTypeID,
                                         lastYearEndDate.AddDays(1), currentEmplAbs.DateEnd.Date, (int)Constants.Used.No, selYearVac.VacYear, false);
                                    if (insertSelYear > 0)
                                    {
                                        emplAbs.CommitTransaction();
                                        saved = true;
                                    }
                                    else
                                        emplAbs.RollbackTransaction();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (emplAbs.GetTransaction() != null)
                                    emplAbs.RollbackTransaction();
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                log.writeLog(DateTime.Now + " EmployeeAbsence.checkForVacation(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);               
            }
            return saved;
        }

      
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
            

				if (cbEmployee.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("absNoEmpl", culture));
					return;
				}
				if (cbAbsenceType.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("absNoAbsType", culture));
					return;
				}
				if (dtpStartDate.Value.Equals(new DateTime(0)))
				{
					MessageBox.Show(rm.GetString("absNoStartDate", culture));
					return;
				}
				if (dtpStartDate.Value.Equals(new DateTime(0)))
				{
					MessageBox.Show(rm.GetString("absNoEndDate", culture));
					return;
				}
				if (dtpStartDate.Value > dtpEndDate.Value)
				{
					MessageBox.Show(rm.GetString("startGreaterThenEnd", culture));
					return;
				}

				int passTypeIDOld = currentEmplAbs.PassTypeID;
				DateTime startDateOld = currentEmplAbs.DateStart;
				DateTime endDateOld = currentEmplAbs.DateEnd;
				currentEmplAbs.EmployeeID = (int) cbEmployee.SelectedValue;
				currentEmplAbs.PassTypeID = (int) cbAbsenceType.SelectedValue;
				currentEmplAbs.DateStart = dtpStartDate.Value;
				currentEmplAbs.DateEnd = dtpEndDate.Value;

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO = currentEmplAbs;
				int existingAbsencesCount = ea.SearchExistingAbsences(wuString);

				if (existingAbsencesCount > 0)
				{
					MessageBox.Show(rm.GetString("absExists", culture));
				}
				else
				{
					// In same transaction, update Employee Absence, find IOPairs for that absence and delete them
					bool updated = new EmployeeAbsence().UpdateEADeleteIOP(currentEmplAbs.RecID, currentEmplAbs.EmployeeID, passTypeIDOld,
						currentEmplAbs.PassTypeID, startDateOld, endDateOld, currentEmplAbs.DateStart.Date, currentEmplAbs.DateEnd.Date, (int) Constants.Used.No, null,tbDesc.Text.ToString().Trim());

					if (updated)
					{
						MessageBox.Show(rm.GetString("absUpdated", culture));
						this.Close();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void EmployeeAbsencesAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmployeeAbsencesAdd.EmployeeAbsencesAdd_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAbsencesAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAbsenceAdd.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbAbsenceType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                this.Cursor = Cursors.WaitCursor;
            
            try
            {
                if (cbAbsenceType.SelectedIndex != 0)
                {
                    if (vacationLicence && (int)cbAbsenceType.SelectedValue == Constants.vacation)
                    {
                        btnDetailes.Visible = true;
                        if (GSKLicence)
                        {
                           // btnPrintRequest.Visible = true;
                        }
                        else
                        {
                           // btnPrintRequest.Visible = false;
                        }
                    }
                    else
                    {
                        btnDetailes.Visible = false;
                       // btnPrintRequest.Visible = false;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsenceAdd.cbAbsenceType_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDetailes_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("absNoEmpl", culture));
                    return;
                }
                //if interval is in two diferent years show message
                if (dtpStartDate.Value.Year != dtpEndDate.Value.Year)
                {
                    MessageBox.Show(rm.GetString("inTwoYears", culture));
                    return ;
                }
                int employeeID =  (int) cbEmployee.SelectedValue;
                DateTime selYear = new DateTime(dtpEndDate.Value.Year,1,1);
                List<EmployeeVacEvidTO> vacations = new EmployeeVacEvid().getVacations(employeeID.ToString(), selYear, selYear, -1, -1);
                VacationDetails details = null;// = new VacationDetails(new EmployeeVacEvid());
                if (vacations.Count > 0)
                {
                    details = new VacationDetails(vacations[0]);
                    details.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noVacDetails", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsenceAdd.btnDetailes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeeAbsencesAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Arrow;
                // list of moduls allowed
                List<int> modList = new List<int>();
                modList = Common.Misc.getLicenceModuls(null);

                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                if (cost == (int)Constants.Customers.GSK)
                {
                    GSKLicence = true;
                   // btnPrintRequest.Visible = true;
                }
                else
                {
                    GSKLicence = false;
                   // btnPrintRequest.Visible = false;
                }
                
                if ((modList.Contains((int)Constants.Moduls.Vacation)))
                {
                    vacationLicence = true;
                }
                else
                {
                    vacationLicence = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsenceAdd.EmployeeAbsencesAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrintRequest_Click(object sender, EventArgs e)
        {
            
            //try
            //{
            //    if (cbEmployee.SelectedIndex <= 0)
            //    {
            //        MessageBox.Show(rm.GetString("absNoEmpl", culture));
            //        return;
            //    }
            //    //if interval is in two diferent years show message
            //    if (dtpStartDate.Value.Year != dtpEndDate.Value.Year)
            //    {
            //        MessageBox.Show(rm.GetString("inTwoYears", culture));
            //        return;
            //    }
            //    int employeeID = (int)cbEmployee.SelectedValue;
            //    Employee selEmployee = new Employee();
            //    foreach (Employee empl in emplArray)
            //    {
            //        if (empl.EmployeeID == employeeID)
            //        {
            //            selEmployee = empl;
            //            break;
            //        }
            //    }
            //    DateTime selYear = new DateTime(dtpEndDate.Value.Year, 1, 1);
            //    ArrayList vacations = new EmployeeVacEvid().getVacations(employeeID.ToString(), selYear, selYear, -1, -1);
            //    EmployeeVacEvid vac;
            //    if (vacations.Count > 0)
            //    {
            //        vac = (EmployeeVacEvid)vacations[0];
            //        Reports.GSK.GSKVAcationRequest req = new Reports.GSK.GSKVAcationRequest(vac, selEmployee, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
            //        req.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show(rm.GetString("noVacationAdded", culture));
            //        return;
            //    }               
                
            //}
            //catch (Exception ex)
            //{
            //    log.writeLog(DateTime.Now + " EmployeeAbsenceAdd.btnPrintRequest_Click(): " + ex.Message + "\n");
            //    MessageBox.Show(ex.Message);
            //}
        }
	}
}
