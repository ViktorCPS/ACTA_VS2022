using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Util;
using Common;
using ACTAConfigManipulation;

namespace UI
{
	/// <summary>
	/// Summary description for Passes.
	/// </summary>
	public class PassesAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbPasses;
		private System.Windows.Forms.Label lblDirection;
		private System.Windows.Forms.ComboBox cbDirection;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.Label lblEmployee;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private PassTO currentPass = null;

        private PassHistTO currentPassHist = null;

		private CultureInfo culture;
		private ResourceManager rm;

		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblEventTime;
		private System.Windows.Forms.CheckBox chkbIsWrkHrs;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker dtpEventTime;
		private string from = "";
		private List<LocationTO> locations1 = new List<LocationTO>();
		private List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
		private List<EmployeeTO> employees1 = new List<EmployeeTO>();		
		private List<PassTypeTO> passtypes = new List<PassTypeTO>();
        private List<PassTO> passes1 = new List<PassTO>();
		DebugLog log;
		ApplUserTO logInUser;

        //Indicate if calling form needs to be reload
        public bool doReloadOnBack = true;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.RichTextBox rtbRemarks;
        private TextBox tbEmployee;
        private TextBox tbWU;
        private Button btnWUTree;
        public bool moreThanOneADD = false;
        private Button btnLocationTree;

        List<WorkingUnitTO> wuArray;
        private CheckBox chbHierarhicly;
        private Button btnDelete;
        List<LocationTO> locArray;

        bool selfServForm = false;

		public PassesAdd()
		{
			InitializeComponent();
			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(PassesAdd).Assembly);
			currentPass = new PassTO();
			setLanguage();
			
			populateWorkingUnitCombo();
			populateEmployeeCombo(-1);
			populateDirectionCombo();
			populatePassTypeCombo();
			populateLocationCombo();

			this.btnUpdate.Visible = false;
            this.btnDelete.Visible = false;
            this.lblRemarks.Visible = false;
            //this.rtbRemarks.Visible = false;
            this.tbEmployee.Visible = false;
            this.tbWU.Visible = false;
		}

		public PassesAdd(string from, List<EmployeeTO> employees, List<WorkingUnitTO> workingunits, List<LocationTO> locations, List<PassTypeTO> passtypes)
		{
			this.from = from;
			this.employees1 = (List<EmployeeTO>)deepCopy(employees, true);
			this.workingunits = workingunits;
			this.locations1 = (List<LocationTO>)deepCopy(locations, true);
			this.passtypes = passtypes;

			logInUser = NotificationController.GetLogInUser();

			InitializeComponent();
			this.CenterToScreen();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(PassesAdd).Assembly);

			LocationTO loc1 = new LocationTO();
			loc1.Name = rm.GetString("all", culture);
			locations1.Insert(0, loc1);

			string name = "";
			foreach(EmployeeTO employee in employees1)
			{
				name = employee.LastName + " " + employee.FirstName;
				employee.LastName = name;
			}

			currentPass = new PassTO();
			setLanguage();
			populateWorkingUnitCombo();
			populateEmployeeCombo(-1);
			populateDirectionCombo();
			populatePassTypeCombo();
			populateLocationCombo();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.btnUpdate.Visible = false;
            this.btnDelete.Visible = false;
            this.tbEmployee.Visible = false;
            this.tbWU.Visible = false;
		}

		public PassesAdd(PassTO pass, List<PassTO> passes)
		{
			InitializeComponent();
			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(PassesAdd).Assembly);
			currentPass = pass;
            currentPassHist = new PassHistTO();
            this.passes1 = passes;

			setLanguage();

            this.tbEmployee.Text = pass.EmployeeName;
            this.tbWU.Text = pass.WUName;
            populateDirectionCombo();
			populatePassTypeCombo();
			populateLocationCombo();

			populateUpdateForm();

			this.btnSave.Visible = false;
            this.btnDelete.Visible = false;
            this.cbEmployee.Visible = false;
            this.cbWU.Visible = false;
            this.tbEmployee.Enabled = false;
            this.tbWU.Enabled = false;
            this.btnWUTree.Visible = false;
            this.chbHierarhicly.Enabled = false;
			
		}

        public PassesAdd(PassHistTO hist, PassTO pass)
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(PassesAdd).Assembly);
            currentPass = pass;
            currentPassHist = hist;
            this.passes1 = new List<PassTO>();

            setLanguage();

            this.tbEmployee.Text = pass.EmployeeName;
            this.tbWU.Text = pass.WUName;
            populateDirectionCombo();
            populatePassTypeCombo();
            populateLocationCombo();

            populateUpdateForm();

            this.btnSave.Visible = false;
            this.btnUpdate.Visible = false;
            this.cbEmployee.Visible = false;
            this.cbWU.Visible = false;
            this.tbEmployee.Enabled = false;
            this.tbWU.Enabled = false;
            this.btnWUTree.Visible = false;
            this.chbHierarhicly.Enabled = false;
            this.cbDirection.Enabled = this.dtpEventTime.Enabled = this.cbPassType.Enabled =
                this.cbLocation.Enabled = this.chkbIsWrkHrs.Enabled = false;
        }

        public PassesAdd(EmployeeTO employee)
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(PassesAdd).Assembly);
            currentPass = new PassTO();
            setLanguage();

            populateWorkingUnitCombo();
            populateEmployeeCombo(-1);
            try
            {
                cbEmployee.SelectedValue = employee.EmployeeID;
            }
            catch 
            {
                MessageBox.Show(rm.GetString("noEmplRight",culture));
                this.Close();
            }
            populateDirectionCombo();
            populatePassTypeCombo();
            populateLocationCombo();

            cbLocation.SelectedIndex = 1;

            this.btnUpdate.Visible = false;
            this.btnDelete.Visible = false;
            this.lblRemarks.Visible = false;
            //this.rtbRemarks.Visible = false;
            this.tbEmployee.Visible = false;
            this.tbWU.Visible = false;
            cbEmployee.Enabled = false;
            cbWU.Enabled = false;
            chbHierarhicly.Enabled = false; 
            cbPassType.SelectedValue = (int)Constants.PassType.Work;
            cbPassType.Enabled = false;
            selfServForm = true;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassesAdd));
            this.gbPasses = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.tbWU = new System.Windows.Forms.TextBox();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.rtbRemarks = new System.Windows.Forms.RichTextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkbIsWrkHrs = new System.Windows.Forms.CheckBox();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.dtpEventTime = new System.Windows.Forms.DateTimePicker();
            this.lblEventTime = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.gbPasses.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPasses
            // 
            this.gbPasses.Controls.Add(this.chbHierarhicly);
            this.gbPasses.Controls.Add(this.btnLocationTree);
            this.gbPasses.Controls.Add(this.btnWUTree);
            this.gbPasses.Controls.Add(this.tbWU);
            this.gbPasses.Controls.Add(this.tbEmployee);
            this.gbPasses.Controls.Add(this.rtbRemarks);
            this.gbPasses.Controls.Add(this.lblRemarks);
            this.gbPasses.Controls.Add(this.label5);
            this.gbPasses.Controls.Add(this.label4);
            this.gbPasses.Controls.Add(this.label2);
            this.gbPasses.Controls.Add(this.label1);
            this.gbPasses.Controls.Add(this.label3);
            this.gbPasses.Controls.Add(this.chkbIsWrkHrs);
            this.gbPasses.Controls.Add(this.cbEmployee);
            this.gbPasses.Controls.Add(this.lblEmployee);
            this.gbPasses.Controls.Add(this.cbWU);
            this.gbPasses.Controls.Add(this.cbLocation);
            this.gbPasses.Controls.Add(this.lblLocation);
            this.gbPasses.Controls.Add(this.cbPassType);
            this.gbPasses.Controls.Add(this.lblPassType);
            this.gbPasses.Controls.Add(this.dtpEventTime);
            this.gbPasses.Controls.Add(this.lblEventTime);
            this.gbPasses.Controls.Add(this.cbDirection);
            this.gbPasses.Controls.Add(this.lblDirection);
            this.gbPasses.Controls.Add(this.lblWU);
            this.gbPasses.Location = new System.Drawing.Point(16, 16);
            this.gbPasses.Name = "gbPasses";
            this.gbPasses.Size = new System.Drawing.Size(344, 428);
            this.gbPasses.TabIndex = 0;
            this.gbPasses.TabStop = false;
            this.gbPasses.Text = "Passes";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(110, 58);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 41;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(302, 245);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 40;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(300, 32);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 39;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // tbWU
            // 
            this.tbWU.Location = new System.Drawing.Point(110, 34);
            this.tbWU.Name = "tbWU";
            this.tbWU.Size = new System.Drawing.Size(160, 20);
            this.tbWU.TabIndex = 38;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(110, 89);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(162, 20);
            this.tbEmployee.TabIndex = 37;
            // 
            // rtbRemarks
            // 
            this.rtbRemarks.Location = new System.Drawing.Point(112, 328);
            this.rtbRemarks.MaxLength = 132;
            this.rtbRemarks.Name = "rtbRemarks";
            this.rtbRemarks.Size = new System.Drawing.Size(160, 88);
            this.rtbRemarks.TabIndex = 36;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(52, 331);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(52, 13);
            this.lblRemarks.TabIndex = 35;
            this.lblRemarks.Text = "Remarks:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(280, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 34;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(280, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(280, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 32;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(280, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(280, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "*";
            // 
            // chkbIsWrkHrs
            // 
            this.chkbIsWrkHrs.Checked = true;
            this.chkbIsWrkHrs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbIsWrkHrs.Enabled = false;
            this.chkbIsWrkHrs.Location = new System.Drawing.Point(112, 288);
            this.chkbIsWrkHrs.Name = "chkbIsWrkHrs";
            this.chkbIsWrkHrs.Size = new System.Drawing.Size(224, 24);
            this.chkbIsWrkHrs.TabIndex = 12;
            this.chkbIsWrkHrs.Text = "Is Work Hours";
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(112, 88);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 3;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(32, 88);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee ID:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(110, 34);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(184, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(112, 248);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(160, 21);
            this.cbLocation.TabIndex = 11;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(16, 248);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(88, 23);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(112, 208);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(160, 21);
            this.cbPassType.TabIndex = 9;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(16, 208);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 8;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEventTime
            // 
            this.dtpEventTime.CustomFormat = "dd.MM.yyy HH:mm:ss";
            this.dtpEventTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEventTime.Location = new System.Drawing.Point(112, 168);
            this.dtpEventTime.Name = "dtpEventTime";
            this.dtpEventTime.ShowUpDown = true;
            this.dtpEventTime.Size = new System.Drawing.Size(160, 20);
            this.dtpEventTime.TabIndex = 7;
            // 
            // lblEventTime
            // 
            this.lblEventTime.Location = new System.Drawing.Point(16, 168);
            this.lblEventTime.Name = "lblEventTime";
            this.lblEventTime.Size = new System.Drawing.Size(88, 23);
            this.lblEventTime.TabIndex = 6;
            this.lblEventTime.Text = "Event Time:";
            this.lblEventTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(112, 128);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(160, 21);
            this.cbDirection.TabIndex = 5;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.cbDirection_SelectedIndexChanged);
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(16, 128);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(88, 23);
            this.lblDirection.TabIndex = 4;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 32);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 451);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 451);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(285, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(16, 451);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // PassesAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(378, 486);
            this.ControlBox = false;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbPasses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "PassesAdd";
            this.ShowInTaskbar = false;
            this.Text = "Passes Add";
            this.Load += new System.EventHandler(this.PassesAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassesAdd_KeyUp);
            this.gbPasses.ResumeLayout(false);
            this.gbPasses.PerformLayout();
            this.ResumeLayout(false);

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
				btnSave.Text = rm.GetString("btnSave", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
               
                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// Form name
				if (currentPass.PassID < 0)
				{
					this.Text = rm.GetString("menuPassesAdd", culture);
				}
				else
				{
					this.Text = rm.GetString("menuPassesUpd", culture);
				}
				// group box text
				this.gbPasses.Text = rm.GetString("menuPasses", culture);

				// label's text
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblDirection.Text = rm.GetString("lblPrimDirect", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblLocation.Text = rm.GetString("lblLocation", culture);
				lblEventTime.Text = rm.GetString("lblEventTime", culture);
				chkbIsWrkHrs.Text = rm.GetString("chkbIsWrkHrs", culture);
                lblRemarks.Text = rm.GetString("lblRemarks", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.setLanguage(): " + ex.Message + "\n");
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
				wuArray = new List<WorkingUnitTO>();
                string vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];

                if (vistorsCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }

				if ( from.Equals("ACTAGate") )
				{

					foreach(WorkingUnitTO currentWU in workingunits)
					{
                        if (!currentWU.WorkingUnitID.ToString().Trim().Equals(vistorsCode.Trim()))
						{
							wuArray.Add(currentWU);
						}												
					}
				}
				else
				{
					WorkingUnit wu = new WorkingUnit();
					List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();

					if (logInUser != null)
					{
						wuList = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);
					}

					foreach(WorkingUnitTO currentWU in wuList)
					{
                        if (!currentWU.WorkingUnitID.ToString().Trim().Equals(vistorsCode.Trim()))
						{
							wuArray.Add(currentWU);
						}												
					}
				}
				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWU.DataSource = wuArray;
				cbWU.DisplayMember = "Name";
				cbWU.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
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
				List<EmployeeTO> emplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
				string[] statuses = {Constants.statusActive, Constants.statusBlocked}; 
				string vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];

                if (vistorsCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }

				if (from.Equals("ACTAGate"))
				{								
					if (wuID == -1)
					{
						foreach(EmployeeTO currentEmployee in employees1)
						{
							foreach( string status in statuses )
							{
								if( currentEmployee.Status.ToString().Trim().Equals(status.Trim()) && !currentEmployee.WorkingUnitID.ToString().Trim().Equals(vistorsCode.Trim()))
								{
									emplArray.Add(currentEmployee);
								}
							}							
						}
					}
					else
					{
						foreach(EmployeeTO currentEmployee in employees1)
						{
							foreach( string status in statuses )
							{
								if( currentEmployee.Status.ToString().Trim().Equals(status.Trim()) && currentEmployee.WorkingUnitID.ToString().Trim().Equals(wuID.ToString().Trim()))
								{
									emplArray.Add(currentEmployee);
								}
							}							
						}						
					}
				}
				else
				{
					List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
				
					if (logInUser != null)
					{
						wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);
					}

					string wuString = "";
					foreach (WorkingUnitTO wUnit in wUnits)
					{
						wuString += wUnit.WorkingUnitID.ToString().Trim() + ","; 
					}
				
					if (wuString.Length > 0)
					{
						wuString = wuString.Substring(0, wuString.Length - 1);
					}

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
				}

				foreach(EmployeeTO employee in emplArray)
				{
					employee.LastName += " " + employee.FirstName;
				}

				EmployeeTO empl1 = new EmployeeTO();
				empl1.LastName = rm.GetString("all", culture);
				emplArray.Insert(0, empl1);
								
				cbEmployee.DataSource = emplArray;
				cbEmployee.DisplayMember = "LastName";
				cbEmployee.ValueMember = "EmployeeID";
				cbEmployee.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Pass Type Combo Box
		/// </summary>
		private void populatePassTypeCombo()
		{
			try
			{				
				List<PassTypeTO> ptArray = new List<PassTypeTO>();

				if (from.Equals("ACTAGate"))
				{
					ptArray = passtypes;
				}
				else
				{
					PassType pt = new PassType();
                    pt.PTypeTO.IsPass = Constants.passOnReader;
					ptArray = pt.Search();
				}
				ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

				cbPassType.DataSource = ptArray;
				cbPassType.DisplayMember = "Description";
				cbPassType.ValueMember = "PassTypeID";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populatePassTypeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Direction Combo Box
		/// </summary>
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
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateDirectionCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Location Combo Box
		/// </summary>
		private void populateLocationCombo()
		{
			try
			{
				locArray = new List<LocationTO>();
				if ( from.Equals("ACTAGate") )
				{
					locArray = locations1;
				}
				else
				{
					Location loc = new Location();
                    loc.LocTO.Status = Constants.DefaultStateActive.Trim();
					locArray = loc.Search();
				}
				
				locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

				cbLocation.DataSource = locArray;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
				cbLocation.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateUpdateForm()
		{
			try
			{
				cbEmployee.SelectedValue = currentPass.EmployeeID;
				cbDirection.SelectedIndex = cbDirection.FindStringExact(currentPass.Direction);
				dtpEventTime.Value = currentPass.EventTime;
				cbPassType.SelectedValue = currentPass.PassTypeID;
				cbLocation.SelectedValue = currentPass.LocationID;
				if (currentPass.IsWrkHrsCount == 0)
				{
					chkbIsWrkHrs.Checked = false;
				}
				else
				{
					chkbIsWrkHrs.Checked = true;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateUpdateForm(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wuArray)
                {
                    if (cbWU.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
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
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            //Do not reload calling form on cancel
            if (!moreThanOneADD)
                doReloadOnBack = false;
			this.Close();
		}

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoEmpl", culture));
                    return;
                }
                if (cbDirection.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoDirect", culture));
                    return;
                }
                if (dtpEventTime.Value.Equals(new DateTime(0)))
                {
                    MessageBox.Show(rm.GetString("passNoEventTime", culture));
                    return;
                }
                if (cbPassType.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoPassType", culture));
                    return;
                }
                if (cbLocation.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoLoc", culture));
                    return;
                }

                currentPass.EmployeeID = (int)cbEmployee.SelectedValue;
                currentPass.Direction = cbDirection.SelectedItem.ToString();
                currentPass.EventTime = dtpEventTime.Value;
                currentPass.PassTypeID = (int)cbPassType.SelectedValue;
                currentPass.IsWrkHrsCount = chkbIsWrkHrs.Checked ? 1 : 0;
                currentPass.LocationID = (int)cbLocation.SelectedValue;
                currentPass.PairGenUsed = (int)Constants.PairGenUsed.Unused;
                currentPass.ManualyCreated = (int)Constants.recordCreated.Manualy;


                //int inserted = currentPass.Save(currentPass.EmployeeID, currentPass.Direction, currentPass.EventTime,
                //    currentPass.PassTypeID, currentPass.PairGenUsed, currentPass.LocationID, currentPass.ManualyCreated,
                //    currentPass.IsWrkHrsCount);

                //24.11.2009 Natasa insert pass into passes hist 
                Pass pass = new Pass();
                bool trans = pass.BeginTransaction();
                if (trans)
                {
                    try
                    {
                        pass.PssTO = currentPass;
                        int inserted = pass.SaveGetID(false);
                        if (inserted > 0)
                        {
                            currentPassHist = new PassHistTO();
                            currentPassHist.PassID = inserted;
                            currentPassHist.EmployeeID = currentPass.EmployeeID;
                            currentPassHist.Direction = currentPass.Direction;
                            currentPassHist.EventTime = currentPass.EventTime;
                            currentPassHist.PassTypeID = currentPass.PassTypeID;
                            currentPassHist.IsWrkHrsCount = currentPass.IsWrkHrsCount;
                            currentPassHist.LocationID = currentPass.LocationID;
                            currentPassHist.PairGenUsed = currentPass.PairGenUsed;
                            currentPassHist.ManualyCreated = currentPass.ManualyCreated;
                            ApplUserTO user = NotificationController.GetLogInUser();
                            currentPassHist.CreatedBy = user.Name;
                            currentPassHist.CreatedTime = DateTime.Now;

                            currentPassHist.Remarks = rtbRemarks.Text.ToString();

                            PassHist ph = new PassHist();
                            ph.SetTransaction(pass.GetTransaction());

                            inserted = ph.Save(currentPassHist.PassID, currentPassHist.EmployeeID, currentPassHist.Direction, currentPassHist.EventTime,
                                currentPassHist.PassTypeID, currentPassHist.IsWrkHrsCount, currentPassHist.LocationID, currentPassHist.PairGenUsed, currentPassHist.ManualyCreated,
                                currentPassHist.Remarks, currentPassHist.CreatedBy, currentPassHist.CreatedTime, false);
                        }
                        else
                        {
                            pass.RollbackTransaction();
                        }
                        if (inserted > 0)
                        {
                            pass.CommitTransaction();
                            if (from.Equals("ACTAGate"))
                            {
                                // Update DB from XML Data;
                                if (Constants.offlineWork.ToUpper().Equals(Constants.yes.ToUpper()))
                                {
                                    DataManager dataController = new DataManager();
                                    dataController.PushToDatabase();
                                }
                                //dataController.GetVisitsFromDatabase();
                            }

                            DialogResult result = MessageBox.Show(this, rm.GetString("passInserted", culture), "PassesAdd", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                if (!selfServForm)
                                    clearControls(this.Controls);
                                moreThanOneADD = true;
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            pass.RollbackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        pass.RollbackTransaction();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnSave_Click(): " + ex.Message + "\n");
                if (ex.Message.Trim().Equals("2627"))
                {
                    MessageBox.Show(rm.GetString("uniquePassExists", culture));
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                /*if (cbEmployee.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoEmpl", culture));
                    return;
                }*/
                if (cbDirection.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoDirect", culture));
                    return;
                }
                if (dtpEventTime.Value.Equals(new DateTime(0)))
                {
                    MessageBox.Show(rm.GetString("passNoEventTime", culture));
                    return;
                }
                if (cbPassType.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoPassType", culture));
                    return;
                }
                if (cbLocation.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("passNoLoc", culture));
                    return;
                }

                currentPassHist.PassID = currentPass.PassID;
                foreach (PassTO pass in passes1)
                {

                    if (pass.PassID == currentPass.PassID)
                    {
                        currentPassHist.EmployeeID = currentPass.EmployeeID;
                        currentPassHist.Direction = pass.Direction;
                        currentPassHist.EventTime = pass.EventTime;
                        currentPassHist.PassTypeID = pass.PassTypeID;
                        currentPassHist.IsWrkHrsCount = pass.IsWrkHrsCount;
                        currentPassHist.LocationID = pass.LocationID;
                        currentPassHist.PairGenUsed = pass.PairGenUsed;
                        currentPassHist.ManualyCreated = pass.ManualyCreated;
                        currentPassHist.CreatedBy = pass.CreatedBy;
                        currentPassHist.CreatedTime = pass.CreatedTime;
                        break;
                    }
                }
                currentPassHist.Remarks = rtbRemarks.Text.ToString();

                // currentPass.EmployeeID = (int)cbEmployee.SelectedValue;
                currentPass.Direction = cbDirection.SelectedItem.ToString();
                currentPass.EventTime = dtpEventTime.Value;
                currentPass.PassTypeID = (int)cbPassType.SelectedValue;
                currentPass.IsWrkHrsCount = chkbIsWrkHrs.Checked ? 1 : 0;
                currentPass.LocationID = (int)cbLocation.SelectedValue;
                currentPass.PairGenUsed = (int)Constants.PairGenUsed.Unused;
                currentPass.ManualyCreated = (int)Constants.recordCreated.Manualy;

                int saved = 0;
                bool updated = false;
                PassHist ph = new PassHist();
                bool trans = ph.BeginTransaction();

                if (trans)
                {
                    try
                    {
                        saved = ph.Save(currentPassHist.PassID, currentPassHist.EmployeeID, currentPassHist.Direction, currentPassHist.EventTime, currentPassHist.PassTypeID, currentPassHist.IsWrkHrsCount, currentPassHist.LocationID, currentPassHist.PairGenUsed, currentPassHist.ManualyCreated, currentPassHist.Remarks, currentPassHist.CreatedBy, currentPassHist.CreatedTime, false);

                        if (saved > 0)
                        {
                            Pass pass = new Pass();
                            pass.SetTransaction(ph.GetTransaction());

                            updated = pass.Update(currentPass.PassID, currentPass.EmployeeID, currentPass.Direction, currentPass.EventTime, currentPass.PassTypeID, currentPass.PairGenUsed, currentPass.LocationID, currentPass.ManualyCreated, currentPass.IsWrkHrsCount, false);
                        }
                        if (updated)
                        {
                            ph.CommitTransaction();

                            MessageBox.Show(rm.GetString("passUpdated", culture));
                            this.Close();
                        }
                        else
                        {
                            ph.RollbackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ph.GetTransaction() != null)
                            ph.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                    return;
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnUpdate_Click(): " + ex.Message + "\n");
                if (ex.Message.Trim().Equals("2627"))
                {
                    MessageBox.Show(rm.GetString("uniquePassExists", culture));
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }	
		}        

		private void PassesAdd_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (from.Equals("ACTAGate"))
                {
                    dtpEventTime.Enabled = false;
                    label3.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdd.PassesAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		public object deepCopy(object array, bool doDeepCopy)
		{
			if (doDeepCopy)
			{
				BinaryFormatter BF = new BinaryFormatter();
				MemoryStream memStream = new MemoryStream();
				BF.Serialize(memStream, array);
				memStream.Flush();
				memStream.Position = 0;
				return (BF.Deserialize(memStream));
			}
			else
			{
				return (this.MemberwiseClone());
			}
		}

		public void clearControls(Control.ControlCollection controls)
		{
			try
			{
				foreach(Control c in controls)
				{									
					if ( c is TextBox)
					{
						TextBox tb = (TextBox)c;	
						tb.Text = "";
					}
					if ( c is RichTextBox)
					{
						RichTextBox rtb = (RichTextBox)c;	
						rtb.Text = "";
					}
					if ( c is ComboBox)
					{
						ComboBox cb = (ComboBox)c;	
						cb.SelectedIndex = 0;
					}
					if ( c is DateTimePicker)
					{
						DateTimePicker dtp = (DateTimePicker)c;	
						dtp.Value = DateTime.Now;
					}
					if ( c.HasChildren )
					{
						clearControls(c.Controls);
					}
				}
			}
			catch(Exception e)
			{
				throw e;
			}			
		}

        private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (selfServForm)
                    return;
                if (cbDirection.SelectedIndex > 0)
                {
                    if (cbDirection.SelectedItem.ToString().Equals(Constants.DirectionIn))
                    {
                        cbPassType.SelectedValue = (int)Constants.PassType.Work;
                        cbPassType.Enabled = false;
                    }
                    else
                    {
                        cbPassType.Enabled = true;
                    }
                }
                else
                {
                    cbPassType.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesAdd.cbDirection_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void PassesAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " PassesAdd.PassesAdd_KeyUp(): " + ex.Message + "\n");
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
                string wuString = "";
                foreach (WorkingUnitTO wUnit in wuArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
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
                log.writeLog(DateTime.Now + " PassesAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesAdd.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                
                bool isDeleted = true;

                PassHist ph = new PassHist();
                bool trans = ph.BeginTransaction();

                if (trans)
                {
                    try
                    {
                        if (!rtbRemarks.Text.Trim().Equals(""))
                            currentPassHist.Remarks += "- " + rtbRemarks.Text.ToString().Trim();
                        int saved = ph.Save(currentPassHist.PassID, currentPassHist.EmployeeID, currentPassHist.Direction, currentPassHist.EventTime, currentPassHist.PassTypeID, currentPassHist.IsWrkHrsCount, currentPassHist.LocationID, currentPassHist.PairGenUsed, currentPassHist.ManualyCreated, currentPassHist.Remarks, currentPassHist.CreatedBy, currentPassHist.CreatedTime, false);

                        if (saved > 0)
                        {
                            Pass pass = new Pass();
                            pass.SetTransaction(ph.GetTransaction());
                            isDeleted = pass.Delete(currentPass.PassID.ToString(), false, currentPass.EventTime) && isDeleted;
                        }

                        if (isDeleted)
                        {
                            ph.CommitTransaction();
                            MessageBox.Show(rm.GetString("passesDel", culture));
                            this.Close();
                        }
                        else
                        {
                            ph.RollbackTransaction();
                            MessageBox.Show(rm.GetString("noPassDel", culture));
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ph.GetTransaction() != null)
                            ph.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                    this.Close();
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
