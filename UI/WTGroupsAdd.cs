using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for WTGroupsAdd.
	/// </summary>
	public class WTGroupsAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbGroupAdd;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.ListView lvEmployee1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private WorkingGroupTO currentGroup = null;

		private CultureInfo culture;
		ResourceManager rm;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.ListView lvEmployee2;

		DebugLog log;
		ApplUserTO logInUser;

		//private ArrayList removedEmployees;

		// List View indexes
		const int EmployeeIDIndex = 0;
		const int FirstNameIndex = 1;
		const int LastNameIndex = 2;
		const int WorkingUnitIDIndex = 3;
        const int UsingDateIndex = 4;

		private ListViewItemComparer _comp1;
		private System.Windows.Forms.Button btnTimeScheduleAssigning;
		private ListViewItemComparer _comp2;
        private Label lblEmployeesInGroup;
        private Label lblEmployeesNotInGroup;
        private GroupBox gbTimeSchema;
        private GroupBox gbStartDate;
        private RadioButton rbSelectDate;
        private RadioButton rbToday;
        private RadioButton rbNextMonth;
        private DateTimePicker dtpSelectDate;
        private Label lblStartDate;
        private ListView lvTimeSchemaDetails;
        private Label lblTimeSchema;
        private TextBox tbTimeSchema;

		//ArrayList originalEmployeeList;

        // Schema Details List View indexes
        const int DayNum = 0;
        const int IntervalNum = 1;
        const int StartTime = 2;
        const int EndTime = 3;
        const int Tolerance = 4;
        private ListViewItemComparer1 _comp3;

        private List<WorkTimeSchemaTO> timeSchemas;
        private Button btnWUTree;
        private DateTime minDate;
        private CheckBox chbHierarhicly;
        private TextBox tbFirstName;
        private TextBox tbLastName;
        private Label lblFirstName;
        private Label lblLastName;
        private TextBox tbBelongFirstName;
        private TextBox tbBelongLastName;
        private Label lblBelongFirstName;
        private Label lblBelongLastName;
        List<WorkingUnitTO> wuArray;


		public WTGroupsAdd()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				InitializeComponent();
				this.CenterToScreen();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentGroup = new WorkingGroupTO();
				logInUser = NotificationController.GetLogInUser();
				//removedEmployees = new ArrayList();

				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(WTGroupsAdd).Assembly);
				setLanguage();

				populateWorkingUnitCombo();
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                			
				List<EmployeeTO> employeeList = new Employee().SearchWithStatuses(statuses, "");

				if (employeeList.Count > 0)
				{
					populateListView(employeeList);
				}

				this.cbWU.SelectedIndex = 0;

				btnUpdate.Visible = false;
				btnTimeScheduleAssigning.Visible = false;

                rbToday.Checked = true;

                tbTimeSchema.ReadOnly = true;
                tbTimeSchema.Enabled = false;
                cbWU.Enabled = false;
                btnWUTree.Enabled = false;
                lvEmployee1.Enabled = false;
                lvEmployee2.Enabled = false;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                gbStartDate.Enabled = false;
                dtpSelectDate.Enabled = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		public WTGroupsAdd(WorkingGroupTO wrkGrp)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				InitializeComponent();
				this.CenterToScreen();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentGroup = new WorkingGroupTO(wrkGrp.EmployeeGroupID, wrkGrp.GroupName, wrkGrp.Description);
				logInUser = NotificationController.GetLogInUser();
				//removedEmployees = new ArrayList();

				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(WTGroupsAdd).Assembly);
				setLanguage();

				populateWorkingUnitCombo();

				populateUpdateForm(wrkGrp);

				btnSave.Visible = false;

                rbToday.Checked = true;

                tbTimeSchema.ReadOnly = true;
                dtpSelectDate.Enabled = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WTGroupsAdd));
            this.gbGroupAdd = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lvEmployee1 = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lvEmployee2 = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTimeScheduleAssigning = new System.Windows.Forms.Button();
            this.lblEmployeesInGroup = new System.Windows.Forms.Label();
            this.lblEmployeesNotInGroup = new System.Windows.Forms.Label();
            this.gbTimeSchema = new System.Windows.Forms.GroupBox();
            this.tbTimeSchema = new System.Windows.Forms.TextBox();
            this.lblTimeSchema = new System.Windows.Forms.Label();
            this.lvTimeSchemaDetails = new System.Windows.Forms.ListView();
            this.gbStartDate = new System.Windows.Forms.GroupBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpSelectDate = new System.Windows.Forms.DateTimePicker();
            this.rbSelectDate = new System.Windows.Forms.RadioButton();
            this.rbToday = new System.Windows.Forms.RadioButton();
            this.rbNextMonth = new System.Windows.Forms.RadioButton();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbBelongFirstName = new System.Windows.Forms.TextBox();
            this.tbBelongLastName = new System.Windows.Forms.TextBox();
            this.lblBelongFirstName = new System.Windows.Forms.Label();
            this.lblBelongLastName = new System.Windows.Forms.Label();
            this.gbGroupAdd.SuspendLayout();
            this.gbTimeSchema.SuspendLayout();
            this.gbStartDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGroupAdd
            // 
            this.gbGroupAdd.Controls.Add(this.tbDescription);
            this.gbGroupAdd.Controls.Add(this.lblDesc);
            this.gbGroupAdd.Controls.Add(this.tbName);
            this.gbGroupAdd.Controls.Add(this.lblName);
            this.gbGroupAdd.Location = new System.Drawing.Point(16, 20);
            this.gbGroupAdd.Name = "gbGroupAdd";
            this.gbGroupAdd.Size = new System.Drawing.Size(395, 115);
            this.gbGroupAdd.TabIndex = 0;
            this.gbGroupAdd.TabStop = false;
            this.gbGroupAdd.Text = "Add Group";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(112, 64);
            this.tbDescription.MaxLength = 256;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(200, 20);
            this.tbDescription.TabIndex = 3;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(16, 64);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(112, 24);
            this.tbName.MaxLength = 64;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(200, 20);
            this.tbName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 225);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(85, 23);
            this.lblWU.TabIndex = 4;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(110, 225);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(206, 21);
            this.cbWU.TabIndex = 5;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lvEmployee1
            // 
            this.lvEmployee1.FullRowSelect = true;
            this.lvEmployee1.GridLines = true;
            this.lvEmployee1.HideSelection = false;
            this.lvEmployee1.Location = new System.Drawing.Point(16, 324);
            this.lvEmployee1.Name = "lvEmployee1";
            this.lvEmployee1.Size = new System.Drawing.Size(395, 195);
            this.lvEmployee1.TabIndex = 17;
            this.lvEmployee1.UseCompatibleStateImageBehavior = false;
            this.lvEmployee1.View = System.Windows.Forms.View.Details;
            this.lvEmployee1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployee1_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(430, 375);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 23);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(430, 420);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(32, 23);
            this.btnRemove.TabIndex = 19;
            this.btnRemove.Text = "<";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvEmployee2
            // 
            this.lvEmployee2.FullRowSelect = true;
            this.lvEmployee2.GridLines = true;
            this.lvEmployee2.HideSelection = false;
            this.lvEmployee2.Location = new System.Drawing.Point(483, 324);
            this.lvEmployee2.Name = "lvEmployee2";
            this.lvEmployee2.Size = new System.Drawing.Size(445, 195);
            this.lvEmployee2.TabIndex = 20;
            this.lvEmployee2.UseCompatibleStateImageBehavior = false;
            this.lvEmployee2.View = System.Windows.Forms.View.Details;
            this.lvEmployee2.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployee2_ColumnClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 685);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 685);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(853, 685);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTimeScheduleAssigning
            // 
            this.btnTimeScheduleAssigning.Location = new System.Drawing.Point(267, 145);
            this.btnTimeScheduleAssigning.Name = "btnTimeScheduleAssigning";
            this.btnTimeScheduleAssigning.Size = new System.Drawing.Size(144, 23);
            this.btnTimeScheduleAssigning.TabIndex = 1;
            this.btnTimeScheduleAssigning.Text = "Time Schedule Assigning";
            this.btnTimeScheduleAssigning.Click += new System.EventHandler(this.btnTimeScheduleAssigning_Click);
            // 
            // lblEmployeesInGroup
            // 
            this.lblEmployeesInGroup.Location = new System.Drawing.Point(483, 235);
            this.lblEmployeesInGroup.Name = "lblEmployeesInGroup";
            this.lblEmployeesInGroup.Size = new System.Drawing.Size(445, 30);
            this.lblEmployeesInGroup.TabIndex = 12;
            this.lblEmployeesInGroup.Text = "List of all employees who belong to selected group:";
            this.lblEmployeesInGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEmployeesNotInGroup
            // 
            this.lblEmployeesNotInGroup.Location = new System.Drawing.Point(32, 184);
            this.lblEmployeesNotInGroup.Name = "lblEmployeesNotInGroup";
            this.lblEmployeesNotInGroup.Size = new System.Drawing.Size(395, 30);
            this.lblEmployeesNotInGroup.TabIndex = 3;
            this.lblEmployeesNotInGroup.Text = "List of all employees who do not belong to selected group:";
            this.lblEmployeesNotInGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbTimeSchema
            // 
            this.gbTimeSchema.Controls.Add(this.tbTimeSchema);
            this.gbTimeSchema.Controls.Add(this.lblTimeSchema);
            this.gbTimeSchema.Controls.Add(this.lvTimeSchemaDetails);
            this.gbTimeSchema.Location = new System.Drawing.Point(483, 20);
            this.gbTimeSchema.Name = "gbTimeSchema";
            this.gbTimeSchema.Size = new System.Drawing.Size(445, 194);
            this.gbTimeSchema.TabIndex = 2;
            this.gbTimeSchema.TabStop = false;
            this.gbTimeSchema.Text = "Current group time schema";
            // 
            // tbTimeSchema
            // 
            this.tbTimeSchema.Location = new System.Drawing.Point(123, 22);
            this.tbTimeSchema.MaxLength = 64;
            this.tbTimeSchema.Name = "tbTimeSchema";
            this.tbTimeSchema.Size = new System.Drawing.Size(299, 20);
            this.tbTimeSchema.TabIndex = 1;
            // 
            // lblTimeSchema
            // 
            this.lblTimeSchema.Location = new System.Drawing.Point(22, 20);
            this.lblTimeSchema.Name = "lblTimeSchema";
            this.lblTimeSchema.Size = new System.Drawing.Size(88, 23);
            this.lblTimeSchema.TabIndex = 0;
            this.lblTimeSchema.Text = "Time Schema:";
            this.lblTimeSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvTimeSchemaDetails
            // 
            this.lvTimeSchemaDetails.BackColor = System.Drawing.SystemColors.Control;
            this.lvTimeSchemaDetails.FullRowSelect = true;
            this.lvTimeSchemaDetails.GridLines = true;
            this.lvTimeSchemaDetails.HideSelection = false;
            this.lvTimeSchemaDetails.Location = new System.Drawing.Point(22, 47);
            this.lvTimeSchemaDetails.MultiSelect = false;
            this.lvTimeSchemaDetails.Name = "lvTimeSchemaDetails";
            this.lvTimeSchemaDetails.Size = new System.Drawing.Size(400, 136);
            this.lvTimeSchemaDetails.TabIndex = 2;
            this.lvTimeSchemaDetails.UseCompatibleStateImageBehavior = false;
            this.lvTimeSchemaDetails.View = System.Windows.Forms.View.Details;
            this.lvTimeSchemaDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTimeSchemaDetails_ColumnClick);
            // 
            // gbStartDate
            // 
            this.gbStartDate.Controls.Add(this.lblStartDate);
            this.gbStartDate.Controls.Add(this.dtpSelectDate);
            this.gbStartDate.Controls.Add(this.rbSelectDate);
            this.gbStartDate.Controls.Add(this.rbToday);
            this.gbStartDate.Controls.Add(this.rbNextMonth);
            this.gbStartDate.Location = new System.Drawing.Point(16, 525);
            this.gbStartDate.Name = "gbStartDate";
            this.gbStartDate.Size = new System.Drawing.Size(395, 145);
            this.gbStartDate.TabIndex = 21;
            this.gbStartDate.TabStop = false;
            this.gbStartDate.Text = "Using date";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(16, 99);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(374, 43);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "* Look into schema definition list to see time schedule for selected using date";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpSelectDate
            // 
            this.dtpSelectDate.CustomFormat = "dd/MM/yyyy";
            this.dtpSelectDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSelectDate.Location = new System.Drawing.Point(180, 70);
            this.dtpSelectDate.Name = "dtpSelectDate";
            this.dtpSelectDate.ShowUpDown = true;
            this.dtpSelectDate.Size = new System.Drawing.Size(160, 20);
            this.dtpSelectDate.TabIndex = 3;
            this.dtpSelectDate.ValueChanged += new System.EventHandler(this.dtpSelectDate_ValueChanged);
            // 
            // rbSelectDate
            // 
            this.rbSelectDate.Location = new System.Drawing.Point(16, 70);
            this.rbSelectDate.Name = "rbSelectDate";
            this.rbSelectDate.Size = new System.Drawing.Size(160, 24);
            this.rbSelectDate.TabIndex = 2;
            this.rbSelectDate.Text = "Select date";
            this.rbSelectDate.CheckedChanged += new System.EventHandler(this.rbSelectDate_CheckedChanged);
            // 
            // rbToday
            // 
            this.rbToday.Checked = true;
            this.rbToday.Location = new System.Drawing.Point(16, 20);
            this.rbToday.Name = "rbToday";
            this.rbToday.Size = new System.Drawing.Size(160, 24);
            this.rbToday.TabIndex = 0;
            this.rbToday.TabStop = true;
            this.rbToday.Text = "Today";
            this.rbToday.CheckedChanged += new System.EventHandler(this.rbToday_CheckedChanged);
            // 
            // rbNextMonth
            // 
            this.rbNextMonth.Location = new System.Drawing.Point(16, 45);
            this.rbNextMonth.Name = "rbNextMonth";
            this.rbNextMonth.Size = new System.Drawing.Size(160, 24);
            this.rbNextMonth.TabIndex = 1;
            this.rbNextMonth.Text = "First day next month";
            this.rbNextMonth.CheckedChanged += new System.EventHandler(this.rbNextMonth_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(320, 225);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 6;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(349, 225);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(101, 24);
            this.chbHierarhicly.TabIndex = 7;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(110, 259);
            this.tbFirstName.MaxLength = 50;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(206, 20);
            this.tbFirstName.TabIndex = 9;
            this.tbFirstName.TextChanged += new System.EventHandler(this.tbFirstName_TextChanged);
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(110, 291);
            this.tbLastName.MaxLength = 50;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(206, 20);
            this.tbLastName.TabIndex = 11;
            this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(22, 259);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(80, 23);
            this.lblFirstName.TabIndex = 8;
            this.lblFirstName.Text = "First Name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(22, 291);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(80, 23);
            this.lblLastName.TabIndex = 10;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbBelongFirstName
            // 
            this.tbBelongFirstName.Location = new System.Drawing.Point(571, 265);
            this.tbBelongFirstName.MaxLength = 50;
            this.tbBelongFirstName.Name = "tbBelongFirstName";
            this.tbBelongFirstName.Size = new System.Drawing.Size(206, 20);
            this.tbBelongFirstName.TabIndex = 14;
            this.tbBelongFirstName.TextChanged += new System.EventHandler(this.tbBelongFirstName_TextChanged);
            // 
            // tbBelongLastName
            // 
            this.tbBelongLastName.Location = new System.Drawing.Point(571, 297);
            this.tbBelongLastName.MaxLength = 50;
            this.tbBelongLastName.Name = "tbBelongLastName";
            this.tbBelongLastName.Size = new System.Drawing.Size(206, 20);
            this.tbBelongLastName.TabIndex = 16;
            this.tbBelongLastName.TextChanged += new System.EventHandler(this.tbBelongLastName_TextChanged);
            // 
            // lblBelongFirstName
            // 
            this.lblBelongFirstName.Location = new System.Drawing.Point(483, 265);
            this.lblBelongFirstName.Name = "lblBelongFirstName";
            this.lblBelongFirstName.Size = new System.Drawing.Size(80, 23);
            this.lblBelongFirstName.TabIndex = 13;
            this.lblBelongFirstName.Text = "First Name:";
            this.lblBelongFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBelongLastName
            // 
            this.lblBelongLastName.Location = new System.Drawing.Point(483, 297);
            this.lblBelongLastName.Name = "lblBelongLastName";
            this.lblBelongLastName.Size = new System.Drawing.Size(80, 23);
            this.lblBelongLastName.TabIndex = 15;
            this.lblBelongLastName.Text = "Last Name:";
            this.lblBelongLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WTGroupsAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(934, 712);
            this.ControlBox = false;
            this.Controls.Add(this.tbBelongFirstName);
            this.Controls.Add(this.tbBelongLastName);
            this.Controls.Add(this.lblBelongFirstName);
            this.Controls.Add(this.lblBelongLastName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.gbStartDate);
            this.Controls.Add(this.gbTimeSchema);
            this.Controls.Add(this.lblEmployeesInGroup);
            this.Controls.Add(this.lblEmployeesNotInGroup);
            this.Controls.Add(this.btnTimeScheduleAssigning);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lvEmployee2);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvEmployee1);
            this.Controls.Add(this.cbWU);
            this.Controls.Add(this.lblWU);
            this.Controls.Add(this.gbGroupAdd);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(950, 750);
            this.MinimumSize = new System.Drawing.Size(950, 736);
            this.Name = "WTGroupsAdd";
            this.ShowInTaskbar = false;
            this.Text = "WTGroupsAdd";
            this.Load += new System.EventHandler(this.WTGroupsAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTGroupsAdd_KeyUp);
            this.gbGroupAdd.ResumeLayout(false);
            this.gbGroupAdd.PerformLayout();
            this.gbTimeSchema.ResumeLayout(false);
            this.gbTimeSchema.PerformLayout();
            this.gbStartDate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List View 
		*/
		private class ListViewItemComparer : IComparer
		{
			private ListView _listView;

			public ListViewItemComparer(ListView lv)
			{
				_listView = lv;
			}
			public ListView ListView
			{
				get{return _listView;}
			}

			private int _sortColumn = 0;

			public int SortColumn
			{
				get { return _sortColumn; }
				set { _sortColumn = value; }

			}

			public int Compare(object a, object b)
			{
				ListViewItem item1 = (ListViewItem) a;
				ListViewItem item2 = (ListViewItem) b;

				if (ListView.Sorting == System.Windows.Forms.SortOrder.Descending)
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
					case WTGroupsAdd.EmployeeIDIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()),Int32.Parse(sub2.Text.Trim()));
					}
					case WTGroupsAdd.WorkingUnitIDIndex:
					case WTGroupsAdd.FirstNameIndex:
					case WTGroupsAdd.LastNameIndex:
                    case WTGroupsAdd.UsingDateIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					
					default:
					{
						return 0;
					}

				}
			}

		}

		#endregion

        #region Inner Class for sorting items in Schema Details View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer1 : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer1(ListView lv)
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

                if (ListView.Sorting == System.Windows.Forms.SortOrder.Descending)
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
                    case WTGroupsAdd.DayNum:
                    case WTGroupsAdd.IntervalNum:
                    case WTGroupsAdd.Tolerance:
                        {
                            int prvi = -1;
                            int drugi = -1;
                            if (!sub1.Text.Trim().Equals(""))
                            {
                                prvi = Int32.Parse(sub1.Text.Trim());
                            }
                            if (!sub2.Text.Trim().Equals(""))
                            {
                                drugi = Int32.Parse(sub2.Text.Trim());
                            }
                            if ((SortColumn == 0) && (prvi == drugi))
                            {
                                if (!item1.SubItems[1].Text.Trim().Equals(""))
                                {
                                    prvi = Int32.Parse(item1.SubItems[1].Text.Trim());
                                }
                                if (!item2.SubItems[1].Text.Trim().Equals(""))
                                {
                                    drugi = Int32.Parse(item2.SubItems[1].Text.Trim());
                                }
                            }
                            return CaseInsensitiveComparer.Default.Compare(prvi, drugi);
                        }
                    case WTGroupsAdd.StartTime:
                    case WTGroupsAdd.EndTime:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
        }

        #endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		public void setLanguage()
		{
			try
			{
				// Form name
				if (currentGroup.EmployeeGroupID.Equals(-1))
				{
					this.Text = rm.GetString("wtGroupsAddForm", culture);

                    // group box text
                    gbGroupAdd.Text = rm.GetString("gbGroupsAdd", culture);
				}
				else
				{
					this.Text = rm.GetString("wtGroupsUpdForm", culture);

                    // group box text
                    gbGroupAdd.Text = rm.GetString("gbGroupsUpd", culture);
				}

                // group box text
                gbTimeSchema.Text = rm.GetString("gbTimeSchema", culture);
                gbStartDate.Text = rm.GetString("gbStartDate", culture);

                //check box text
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

                // radio button's text
                rbToday.Text = rm.GetString("rbToday", culture);
                rbNextMonth.Text = rm.GetString("rbNextMonth", culture);
                rbSelectDate.Text = rm.GetString("rbSelectDate", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnTimeScheduleAssigning.Text = rm.GetString("btnTimeSchedule", culture);

				// label's text
				lblDesc.Text = rm.GetString("lblDescription", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployeesNotInGroup.Text = rm.GetString("lblEmployeesNotInGroup", culture);
                lblEmployeesInGroup.Text = rm.GetString("lblEmployeesInGroup", culture);
                lblTimeSchema.Text = rm.GetString("lblSchema", culture);
                lblStartDate.Text = rm.GetString("lblStartDateDef", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblBelongFirstName.Text = rm.GetString("lblFirstName", culture);
                lblBelongLastName.Text = rm.GetString("lblLastName", culture);

				// list view
				lvEmployee1.BeginUpdate();
				lvEmployee1.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployee1.Width - 4) / 4 - 25, HorizontalAlignment.Left);
				lvEmployee1.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployee1.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee1.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployee1.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee1.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployee1.Width - 4) / 4 + 10, HorizontalAlignment.Left);
				lvEmployee1.EndUpdate();

				lvEmployee2.BeginUpdate();
				lvEmployee2.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployee2.Width - 4) / 5 - 10, HorizontalAlignment.Left);
				lvEmployee2.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployee2.Width - 4) / 5, HorizontalAlignment.Left);
				lvEmployee2.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployee2.Width - 4) / 5, HorizontalAlignment.Left);
				lvEmployee2.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployee2.Width - 4) / 5, HorizontalAlignment.Left);
                lvEmployee2.Columns.Add(rm.GetString("hdrUsingDate", culture), (lvEmployee2.Width - 4) / 5 + 10, HorizontalAlignment.Left);
				lvEmployee2.EndUpdate();

                lvTimeSchemaDetails.BeginUpdate();
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrDayNum", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrIntervalNumber", culture), lvTimeSchemaDetails.Width / 5 - 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrSartTime", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrEndTime", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrTolerance", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.setLanguage(): " + ex.Message + "\n");
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
				WorkingUnit wu = new WorkingUnit();
                wu.WUTO.Status = Constants.DefaultStateActive;
				wuArray = wu.Search();
				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWU.DataSource = wuArray;
				cbWU.DisplayMember = "Name";
				cbWU.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.populateWorkingUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Populate form with data of working group to update
		/// </summary>
		/// <param name="loc"></param>
		public void populateUpdateForm(WorkingGroupTO wrkGrp)
		{
			try
			{
				this.tbName.Text = wrkGrp.GroupName.Trim();
				this.tbDescription.Text = wrkGrp.Description.Trim();
				this.cbWU.SelectedIndex = 0;

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                
				List<EmployeeTO> employeeList = new Employee().SearchWithStatusesNotInGroup(statuses, "", currentGroup.EmployeeGroupID);

				if (employeeList.Count > 0)
				{
					populateListView(employeeList);
				}

                Employee empl = new Employee();
                empl.EmplTO.WorkingGroupID = wrkGrp.EmployeeGroupID;

				employeeList = empl.SearchWithStatuses(statuses, "");

				if (employeeList.Count > 0)
				{
					populateListViewSelected(employeeList);
					//originalEmployeeList = employeeList;
				}

				foreach(ListViewItem item2 in lvEmployee2.Items)
				{
					foreach(ListViewItem item1 in lvEmployee1.Items)
					{
						if ( item2.Text.Equals(item1.Text) )
						{
							lvEmployee1.Items.Remove(item1);
						}
					}					
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.populateUpdateForm(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Save new Working Group
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                currentGroup.GroupName = this.tbName.Text.Trim();
                currentGroup.Description = this.tbDescription.Text.Trim();

                int insertedID = new WorkingGroup().Save(currentGroup.GroupName, currentGroup.Description,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), Constants.defaultSchemaID,
                    Constants.defaultStartDay);
                if (insertedID > 1)
                {
                    /*EmployeeTO employeeTO = new EmployeeTO();

                    foreach(ListViewItem item in lvEmployee2.Items)
                    {
                        employeeTO = new Employee().Find(item.Text);
                        new Employee().Update(employeeTO.EmployeeID.ToString(), employeeTO.FirstName, employeeTO.LastName,
                            employeeTO.WorkingUnitID.ToString(), employeeTO.Status, employeeTO.Password,
                            employeeTO.AddressID.ToString(), employeeTO.Picture, insertedID.ToString(), 
                            employeeTO.Type, employeeTO.AccessGroupID.ToString().Trim());
                    }*/

                    DialogResult result = MessageBox.Show(rm.GetString("groupInserted", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        currentGroup = new WorkingGroupTO(insertedID, tbName.Text, tbDescription.Text);

                        populateTimeSchemaDetailsListView(DateTime.Now.Date);
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        btnTimeScheduleAssigning.Visible = true;
                        tbTimeSchema.Enabled = true;
                        cbWU.Enabled = true;
                        btnWUTree.Enabled = true;
                        lvEmployee1.Enabled = true;
                        lvEmployee2.Enabled = true;
                        btnAdd.Enabled = true;
                        btnRemove.Enabled = true;
                        gbStartDate.Enabled = true;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                    MessageBox.Show(mysqlex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                bool isUpdated = false;
                bool succesful = true;

                currentGroup.GroupName = this.tbName.Text.Trim();
                currentGroup.Description = this.tbDescription.Text.Trim();

                string emplIDs = "";

                foreach (ListViewItem item in lvEmployee2.Items)
                {                    
                    string startDate = item.SubItems[4].Text;
                    if (!startDate.Equals(@"N/A"))
                    {
                        string[] dateElements = item.SubItems[4].Text.Split('.');
                        if (dateElements.Length == 3)
                        {
                            emplIDs += item.Text.Trim() + ",";
                        }
                    }
                }

                if (emplIDs.Trim().Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();
                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(emplIDs);
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

                WorkingGroup wg = new WorkingGroup();
                bool transBegin = wg.BeginTransaction();

                if (transBegin)
                {
                    try
                    {
                        if (isUpdated = wg.Update(currentGroup.EmployeeGroupID, currentGroup.GroupName, currentGroup.Description, false))
                        {
                            EmployeeTO employeeTO = new EmployeeTO();

                            /*foreach(ListViewItem item in removedEmployees)
                            {
                                foreach(Employee employee in originalEmployeeList)
                                {
                                    if (item.Text == employee.EmployeeID.ToString().Trim())
                                    {
                                        employeeTO = new Employee().Find(item.Text);
                                        new Employee().Update(employeeTO.EmployeeID.ToString(), employeeTO.FirstName, employeeTO.LastName,
                                            employeeTO.WorkingUnitID.ToString(), employeeTO.Status, employeeTO.Password,
                                            employeeTO.AddressID.ToString(), employeeTO.Picture, 
                                            Constants.defaultWorkingGroupID.ToString().Trim(), 
                                            employeeTO.Type, employeeTO.AccessGroupID.ToString().Trim());
                                        break;
                                    }
                                }						
                            }*/

                            List<EmployeeGroupsTimeScheduleTO> groupSchedules = new EmployeeGroupsTimeSchedule().SearchFromSchedules(
                                currentGroup.EmployeeGroupID, minDate, wg.GetTransaction());
                                                        
                            DateTime startingDate = new DateTime();
                            foreach (ListViewItem item in lvEmployee2.Items)
                            {
                                bool emplSuccesful = true;

                                //ako se transakcija otvara na nivou jednog zaposlenog, uraditi to ovde
                                string startDate = item.SubItems[4].Text;
                                if (!startDate.Equals(@"N/A"))
                                {
                                    string[] dateElements = item.SubItems[4].Text.Split('.');
                                    if (dateElements.Length == 3)
                                    {
                                        int day = Int32.Parse(dateElements[0]);
                                        int month = Int32.Parse(dateElements[1]);
                                        int year = Int32.Parse(dateElements[2]);
                                        startingDate = new DateTime(year, month, day, 0, 0, 0);

                                        Employee tempEmpl = new Employee();
                                        tempEmpl.SetTransaction(wg.GetTransaction());
                                        employeeTO = new EmployeeTO();
                                        int id = -1;
                                        if (int.TryParse(item.Text.Trim(), out id) && emplDict.ContainsKey(id))
                                            employeeTO = emplDict[id];
                                        //emplIDs += employeeTO.EmployeeID.ToString().Trim() + ",";
                                        emplSuccesful = tempEmpl.Update(employeeTO.EmployeeID.ToString(), employeeTO.FirstName, employeeTO.LastName,
                                            employeeTO.WorkingUnitID.ToString(), employeeTO.Status, employeeTO.Password,
                                            employeeTO.AddressID.ToString(), employeeTO.Picture, currentGroup.EmployeeGroupID.ToString(),
                                            employeeTO.Type, employeeTO.AccessGroupID.ToString().Trim(), false) && emplSuccesful;

                                        if (emplSuccesful)
                                        {
                                            EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
                                            ets.SetTransaction(wg.GetTransaction());
                                            emplSuccesful = ets.DeleteFromToSchedule(employeeTO.EmployeeID, startingDate.Date, new DateTime(), "", false) && emplSuccesful;

                                            if (emplSuccesful)
                                            {
                                                int timeScheduleIndex = -1;
                                                for (int scheduleIndex = 0; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                                {
                                                    if (startingDate.Date >= groupSchedules[scheduleIndex].Date)
                                                    {
                                                        timeScheduleIndex = scheduleIndex;
                                                    }
                                                }
                                                if (timeScheduleIndex >= 0)
                                                {
                                                    EmployeeGroupsTimeScheduleTO egts = groupSchedules[timeScheduleIndex];
                                                    int startDay = egts.StartCycleDay;
                                                    int schemaID = egts.TimeSchemaID;

                                                    WorkTimeSchemaTO actualTimeSchema = null;
                                                    foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                                                    {
                                                        if (currentTimeSchema.TimeSchemaID == schemaID)
                                                        {
                                                            actualTimeSchema = currentTimeSchema;
                                                            break;
                                                        }
                                                    }
                                                    if (actualTimeSchema != null)
                                                    {
                                                        int cycleDuration = actualTimeSchema.CycleDuration;

                                                        TimeSpan ts = new TimeSpan(startingDate.Date.Ticks - egts.Date.Date.Ticks);
                                                        int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                                        int insert = ets.Save(employeeTO.EmployeeID, startingDate.Date,
                                                            schemaID, dayNum, "", false);
                                                        emplSuccesful = (insert > 0) && emplSuccesful;

                                                        if (emplSuccesful)
                                                        {
                                                            for (int scheduleIndex = timeScheduleIndex + 1; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                                            {
                                                                egts = groupSchedules[scheduleIndex];

                                                                insert = ets.Save(employeeTO.EmployeeID, egts.Date,
                                                                    egts.TimeSchemaID, egts.StartCycleDay, "", false);
                                                                emplSuccesful = (insert > 0) && emplSuccesful;

                                                                if (!emplSuccesful)
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (emplSuccesful)
                                                {
                                                    // delete absences pairs and update absences to unused
                                                    deleteIOPUpdateEA(employeeTO.EmployeeID, startingDate.Date, wg.GetTransaction());

                                                    //recalculate pauses
                                                    if (startingDate.Date <= DateTime.Now.Date)
                                                    {
                                                        IOPair ioPair = new IOPair();
                                                        ioPair.recalculatePause(employeeTO.EmployeeID.ToString(), startingDate.Date, DateTime.Now.Date, wg.GetTransaction());
                                                    }
                                                } //if (emplSuccesful)
                                            } //if (emplSuccesful)
                                        } //if (emplSuccesful)
                                    } //if (dateElements.Length == 3)
                                } //if (!startDate.Equals(@"N/A"))

                                //ako se transakcija otvara na nivou jednog zaposlenog, i
                                //ako nije emplSuccesful, treba ponistiti transakciju
                                //a ako je emplSuccesful, potvrditi je

                                succesful = succesful && emplSuccesful;
                            }                            

                            if (succesful)
                            {
                                // validate new employee schedule
                                bool validFundHrs = true;
                                DateTime scheduleInvalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, emplIDs.Trim(), startingDate.Date.Date, 
                                    startingDate.Date.AddMonths(1).Date, wg.GetTransaction(), null, false, ref validFundHrs, true);
                                if (startingDate == new DateTime() || scheduleInvalidDate.Equals(new DateTime()))
                                {
                                    wg.CommitTransaction();
                                    MessageBox.Show(rm.GetString("grpUpd", culture));
                                    this.Close();
                                }
                                else
                                {
                                    wg.RollbackTransaction();
                                    if (validFundHrs)
                                    MessageBox.Show(rm.GetString("notValidScheduleAssigned", culture)
                                         + " " + scheduleInvalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + scheduleInvalidDate.Date.ToString(Constants.dateFormat));
                                    else
                                        MessageBox.Show(rm.GetString("notValidFundHrs", culture), " " + scheduleInvalidDate.Date.ToString(Constants.dateFormat) + "-" + scheduleInvalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                                }
                            }
                            else
                            {
                                wg.RollbackTransaction();
                                MessageBox.Show(rm.GetString("grpNotUpd", culture));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (wg.GetTransaction() != null)
                        {
                            wg.RollbackTransaction();
                        }
                        throw ex;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " WTGroupsAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                    MessageBox.Show(mysqlex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        // delete absences pairs and update absences to unused
        private void deleteIOPUpdateEA(int employeeID, DateTime startingDate, IDbTransaction trans)
        {
            try
            {
                #region Reprocess Dates
                List<DateTime> datesList = new List<DateTime>();

                DateTime endDate = new IOPairProcessed().getMaxDateOfPair(employeeID.ToString(), trans);

                if (endDate.Date < DateTime.Now.Date)
                    endDate = DateTime.Now.Date;

                for (DateTime dt = startingDate.Date; dt <= endDate; dt = dt.AddDays(1))
                {
                    datesList.Add(dt);
                }

                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                emplDateWholeDayList.Add(employeeID, datesList);
                if (datesList.Count > 0)
                {
                    Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), startingDate.Date, DateTime.Now.Date, trans, emplDateWholeDayList, null, "");
                }

                #endregion

                DateTime end = new DateTime(0);
                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = employeeID;
                ea.EmplAbsTO.DateStart = startingDate.Date;
                ea.EmplAbsTO.DateEnd = end;
                List<EmployeeAbsenceTO> emplAbsences = ea.Search("", trans);

                foreach (EmployeeAbsenceTO abs in emplAbsences)
                {
                    new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
                        abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int)Constants.Used.No, trans);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.deleteIOPUpdateEA(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		/// <summary>
		/// Populate ListView
		/// </summary>
		/// <param name="employeesList">Array of Employee object</param>
		public void populateListView(List<EmployeeTO> employeesList)
		{
			try
			{				
				lvEmployee1.BeginUpdate();
				lvEmployee1.Items.Clear();

				if (employeesList.Count > 0)
				{
					foreach(EmployeeTO employee in employeesList)
					{
						ListViewItem item = new ListViewItem();

						item.Text = employee.EmployeeID.ToString().Trim();
						item.SubItems.Add(employee.FirstName.Trim());
						item.SubItems.Add(employee.LastName.Trim());

						// Get Working Unit name for the particular user
                        //wu = new WorkingUnit();
                        //if (wu.Find(employee.WorkingUnitID))
                        //{
                        //    /*if (wu.WorkingUnitID == 0)
                        //    {
                        //        wu.Name = "DEFAULT";
                        //    }*/
                        //    item.SubItems.Add(wu.Name.Trim());
                        //}
                        //else
                        //    item.SubItems.Add("");

                        item.SubItems.Add(employee.WorkingUnitName.Trim());
						lvEmployee1.Items.Add(item);
					}
				}

				lvEmployee1.EndUpdate();
				lvEmployee1.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void populateListViewSelected(List<EmployeeTO> employeesList)
		{
			try
			{
				lvEmployee2.BeginUpdate();
				lvEmployee2.Items.Clear();

				if (employeesList.Count > 0)
				{
					foreach(EmployeeTO employee in employeesList)
					{
						ListViewItem item = new ListViewItem();

						item.Text = employee.EmployeeID.ToString().Trim();
						item.SubItems.Add(employee.FirstName.Trim());
						item.SubItems.Add(employee.LastName.Trim());

						// Get Working Unit name for the particular user
                        //wu = new WorkingUnit();
                        //if (wu.Find(employee.WorkingUnitID))
                        //{
                        //    item.SubItems.Add(wu.Name.Trim());
                        //}
                        //else
                        //    item.SubItems.Add("");
                        item.SubItems.Add(employee.WorkingUnitName.Trim());
                        item.SubItems.Add(@"N/A");
						
						lvEmployee2.Items.Add(item);
					}
				}

				lvEmployee2.EndUpdate();
				lvEmployee2.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateTimeSchemaDetailsListView(DateTime date)
        {
            try
            {               
                List<EmployeeGroupsTimeScheduleTO> timeScheduleList = new EmployeeGroupsTimeSchedule().SearchMonthSchedule(currentGroup.EmployeeGroupID, date);

                int timeScheduleIndex = -1;
                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();
                int dayNum = -1;
                if (timeScheduleIndex >= 0)
                {
                    EmployeeGroupsTimeScheduleTO egts = timeScheduleList[timeScheduleIndex];
                    int startDay = egts.StartCycleDay;
                    int schemaID = egts.TimeSchemaID;

                    WorkTimeSchemaTO actualTimeSchema = null;
                    foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                    {
                        if (currentTimeSchema.TimeSchemaID == schemaID)
                        {
                            actualTimeSchema = currentTimeSchema;
                            break;
                        }
                    }
                    if (actualTimeSchema != null)
                    {
                        tbTimeSchema.Text = actualTimeSchema.TimeSchemaID + " - " + actualTimeSchema.Name;

                        days = actualTimeSchema.Days;
                        int cycleDuration = actualTimeSchema.CycleDuration;

                        TimeSpan ts = new TimeSpan(date.Date.Ticks - egts.Date.Date.Ticks);
                        dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;
                    }
                    else
                        tbTimeSchema.Text = "";
                }
                else
                    tbTimeSchema.Text = "";

                lvTimeSchemaDetails.BeginUpdate();
                lvTimeSchemaDetails.Items.Clear();

                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int,WorkTimeIntervalTO>();
                foreach (int dayKey in days.Keys)
                {
                    bool select = false;
                    if (dayKey == dayNum)
                        select = true;

                    intervals = days[dayKey];

                    foreach (int intKey in intervals.Keys)
                    {
                        interval = intervals[intKey];

                        ListViewItem item = new ListViewItem();
                        item.Text = (interval.DayNum + 1).ToString();

                        item.SubItems.Add((interval.IntervalNum + 1).ToString());
                        item.SubItems.Add(interval.StartTime.ToString("HH:mm"));
                        item.SubItems.Add(interval.EndTime.ToString("HH:mm"));

                        // Check tolerance
                        TimeSpan ts0 = new TimeSpan();
                        ts0 = interval.StartTime.Subtract(interval.EarliestArrived);
                        int tm0;
                        if (ts0.Minutes < 0)
                        {
                            tm0 = 60 + ts0.Minutes;
                        }
                        else
                        {
                            tm0 = ts0.Minutes;
                        }

                        TimeSpan ts1 = new TimeSpan();
                        ts1 = interval.LatestArrivaed.Subtract(interval.StartTime);
                        int tm1;
                        if (ts1.Minutes < 0)
                        {
                            tm1 = 60 + ts1.Minutes;
                        }
                        else
                        {
                            tm1 = ts1.Minutes;
                        }

                        TimeSpan ts2 = new TimeSpan();
                        ts2 = interval.EndTime.Subtract(interval.EarliestLeft);
                        int tm2;
                        if (ts2.Minutes < 0)
                        {
                            tm2 = 60 + ts2.Minutes;
                        }
                        else
                        {
                            tm2 = ts2.Minutes;
                        }

                        TimeSpan ts3 = new TimeSpan();
                        ts3 = interval.LatestLeft.Subtract(interval.EndTime);
                        int tm3;
                        if (ts3.Minutes < 0)
                        {
                            tm3 = 60 + ts3.Minutes;
                        }
                        else
                        {
                            tm3 = ts3.Minutes;
                        }

                        if ((tm0 == tm1) && (tm0 == tm2) && (tm0 == tm3) && (tm1 == tm2) && (tm1 == tm3) && (tm2 == tm3)
                            && (tm0 > 0) && (tm1 > 0) && (tm2 > 0) && (tm3 > 0))
                        {
                            item.SubItems.Add(Convert.ToString(ts0.Minutes));
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }

                        item.Tag = interval;

                        if (select)
                            item.Selected = true;

                        lvTimeSchemaDetails.Items.Add(item);
                    }
                }

                lvTimeSchemaDetails.EndUpdate();
                lvTimeSchemaDetails.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.populateTimeSchemaDetailsListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (!(this.cbWU.SelectedValue is int))
                    return;
                this.Cursor = Cursors.WaitCursor;
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

                string wuID = "";

                if ((int)this.cbWU.SelectedValue == -1)
                {
                    wuID = "";
                }
                else
                {
                    wuID = this.cbWU.SelectedValue.ToString().Trim();
                }

                //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                WorkingUnit wu = new WorkingUnit();
                if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
                {
                    List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                    WorkingUnit workUnit = new WorkingUnit();
                    workUnit.WUTO = workUnit.FindWU((int)this.cbWU.SelectedValue);                        
                    wuList.Add(workUnit.WUTO);
                    if (workUnit.WUTO.ChildWUNumber > 0)
                        wuList = wu.FindAllChildren(wuList);
                    wuID = "";
                    foreach (WorkingUnitTO wunit in wuList)
                    {
                        wuID += wunit.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (wuID.Length > 0)
                    {
                        wuID = wuID.Substring(0, wuID.Length - 1);
                    }
                }

                List<EmployeeTO> employeeList = new Employee().SearchWithStatusesNotInGroup(statuses, wuID, currentGroup.EmployeeGroupID);

                populateListView(employeeList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroups.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                if (lvTimeSchemaDetails.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("emplGrpNotHaveTimeSchedule", culture));
                    return;
                }

                ListViewItem lastItem = null;
                foreach (ListViewItem item in lvEmployee1.SelectedItems)
                {
                   
                    DateTime startDate = DateTime.Now.Date;
                    if (rbNextMonth.Checked)
                    {
                        DateTime nextMonth = DateTime.Now.Date.AddMonths(1);
                        DateTime firstNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                        startDate = firstNextMonth.Date;
                    }
                    if (rbSelectDate.Checked)
                    {
                        startDate = dtpSelectDate.Value.Date;
                        if (dtpSelectDate.Value.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                        {
                            Employee tempEmpl = new Employee();
                            EmployeeTO empl = tempEmpl.Find(item.Text);

                            // get dictionary of all rules, key is company and value are rules by employee type id
                            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule().SearchWUEmplTypeDictionary();
                            Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit().getWUDictionary();

                            int cutOffDate = -1;
                            int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                            if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID) && emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                                cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                            if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > cutOffDate && dtpSelectDate.Value.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                            {
                                MessageBox.Show(rm.GetString("cutOffDayPessed", culture));
                                return;
                            }
                        }
                    }
                    lvEmployee1.Items.Remove(item);
                    item.SubItems.Add(startDate.ToString("dd.MM.yyyy"));
                    lvEmployee2.Items.Add(item);

                    if ((minDate == new DateTime(0)) || (minDate.Date > startDate.Date))
                        minDate = startDate.Date;
                    lastItem = item;
                }

                if (lastItem != null)
                {
                    lvEmployee2.SelectedItems.Clear();

                    foreach (ListViewItem item in lvEmployee2.Items)
                    {
                        if (item.Text.Trim().Equals(lastItem.Text.Trim()))
                        {
                            item.Selected = true;
                            lvEmployee2.Select();
                            lvEmployee2.EnsureVisible(lvEmployee2.Items.IndexOf(lvEmployee2.SelectedItems[0]));

                            break;
                        }
                    }

                    lvEmployee2.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroups.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ListViewItem item in lvEmployee2.SelectedItems)
                {
                    string startDate = item.SubItems[4].Text;
                    if (!startDate.Equals(@"N/A"))
                    {
                        lvEmployee2.Items.Remove(item);
                        item.SubItems.RemoveAt(4);
                        lvEmployee1.Items.Add(item);
                        //removedEmployees.Add(item);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotRemoveNAEmployees", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroups.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvEmployee1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				System.Windows.Forms.SortOrder prevOrder = lvEmployee1.Sorting;
                lvEmployee1.Sorting = System.Windows.Forms.SortOrder.None;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == System.Windows.Forms.SortOrder.Ascending)
					{
						lvEmployee1.Sorting = System.Windows.Forms.SortOrder.Descending;
					}
					else
					{
						lvEmployee1.Sorting = System.Windows.Forms.SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvEmployee1.Sorting = System.Windows.Forms.SortOrder.Ascending;
				}
                lvEmployee1.ListViewItemSorter = _comp1;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.lvEmployee1_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void lvEmployee2_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				System.Windows.Forms.SortOrder prevOrder = lvEmployee2.Sorting;
                lvEmployee2.Sorting = System.Windows.Forms.SortOrder.None;

				if (e.Column == _comp2.SortColumn)
				{
					if (prevOrder == System.Windows.Forms.SortOrder.Ascending)
					{
						lvEmployee2.Sorting = System.Windows.Forms.SortOrder.Descending;
					}
					else
					{
						lvEmployee2.Sorting = System.Windows.Forms.SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp2.SortColumn = e.Column;
					lvEmployee2.Sorting = System.Windows.Forms.SortOrder.Ascending;
				}
                lvEmployee2.ListViewItemSorter = _comp2;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.lvEmployee2_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void WTGroupsAdd_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer objects
                _comp1 = new ListViewItemComparer(lvEmployee1);
                lvEmployee1.ListViewItemSorter = _comp1;
                lvEmployee1.Sorting = System.Windows.Forms.SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvEmployee2);
                lvEmployee2.ListViewItemSorter = _comp2;
                lvEmployee2.Sorting = System.Windows.Forms.SortOrder.Ascending;

                _comp3 = new ListViewItemComparer1(lvTimeSchemaDetails);
                lvTimeSchemaDetails.ListViewItemSorter = _comp3;
                lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;

                timeSchemas = new TimeSchema().Search();
                minDate = new DateTime(0);

                if (currentGroup.EmployeeGroupID != -1)
                    populateTimeSchemaDetailsListView(DateTime.Now.Date);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " WTGroupsAdd.WTGroupsAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnTimeScheduleAssigning_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				//string[] statuses = {Constants.statusActive, Constants.statusBlocked}; 
				//ArrayList employeeList = new Employee().SearchWithStatuses("", "", "", "", statuses, "", "", "", currentGroup.EmployeeGroupID.ToString(), "");

				//if (employeeList.Count > 0)
				//{
					WTWorkingGroupTimeSchedules grpTimeSchedule = new WTWorkingGroupTimeSchedules(currentGroup);
					grpTimeSchedule.ShowDialog(this);

                    DateTime populateForDate = DateTime.Now.Date;
                    if (rbNextMonth.Checked)
                    {
                        DateTime nextMonth = DateTime.Now.Date.AddMonths(1);
                        DateTime firstNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                        populateForDate = firstNextMonth.Date;
                    }
                    if (rbSelectDate.Checked)
                    {
                        populateForDate = dtpSelectDate.Value.Date;
                    }
                    populateTimeSchemaDetailsListView(populateForDate);
				/*}
				else
				{
					MessageBox.Show(rm.GetString("noEmplInGroup", culture));
				}*/
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroupsAdd.btnTimeScheduleAssigning_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvTimeSchemaDetails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                System.Windows.Forms.SortOrder prevOrder = lvTimeSchemaDetails.Sorting;

                if (e.Column == _comp3.SortColumn)
                {
                    if (prevOrder == System.Windows.Forms.SortOrder.Ascending)
                    {
                        lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp3.SortColumn = e.Column;
                    lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;
                }
                lvTimeSchemaDetails.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.lvTimeSchemaDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbToday_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbToday.Checked)
                {
                    populateTimeSchemaDetailsListView(DateTime.Now.Date);
                    dtpSelectDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.rbToday_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbNextMonth_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbNextMonth.Checked)
                {
                    DateTime nextMonth = DateTime.Now.Date.AddMonths(1);
                    DateTime firstNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    populateTimeSchemaDetailsListView(firstNextMonth.Date);
                    dtpSelectDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.rbNextMonth_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbSelectDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbSelectDate.Checked)
                {
                    dtpSelectDate.Enabled = true;
                    populateTimeSchemaDetailsListView(dtpSelectDate.Value.Date);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.rbSelectDate_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpSelectDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);
                DateTime firstLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                if (Common.Misc.isLockedDate(dtpSelectDate.Value.Date))
                {
                    string exceptionString = Common.Misc.dataLockedMessage(dtpSelectDate.Value.Date);
                    DateTime firstUnlock = Common.Misc.lastLockDate().AddDays(1);

                    if (firstUnlock < firstLastMonth.Date)
                    {
                        dtpSelectDate.Value = DateTime.Now.Date;
                    }
                    else
                    {
                        dtpSelectDate.Value = firstUnlock;
                    }
                    throw new Exception(exceptionString);
                }

                
                if (dtpSelectDate.Value.Date < firstLastMonth.Date)
                {
                    dtpSelectDate.Value = DateTime.Now.Date;
                    MessageBox.Show(rm.GetString("minSelectDate", culture));
                }
               
                
                populateTimeSchemaDetailsListView(dtpSelectDate.Value.Date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.dtpSelectDate_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void WTGroupsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTGroupsAdd.WTGroupsAdd_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " WTGroupsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            cbWU_SelectedIndexChanged(this, new EventArgs());
        }

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //tbLastName.Text = "";
                foreach (ListViewItem item in lvEmployee1.SelectedItems)
                    item.Selected = false;
                foreach (ListViewItem item in lvEmployee1.Items)
                {
                    if (item.SubItems[WTGroupsAdd.FirstNameIndex].Text.Trim().ToUpper().StartsWith(tbFirstName.Text.Trim().ToUpper()) 
                        && item.SubItems[WTGroupsAdd.LastNameIndex].Text.Trim().ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper()))
                    {
                        item.Selected = true;
                        lvEmployee1.Select();
                        lvEmployee1.EnsureVisible(lvEmployee1.Items.IndexOf(lvEmployee1.SelectedItems[0]));
                        break;
                    }
                }

                tbFirstName.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.tbFirstName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbLastName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //tbFirstName.Text = "";
                foreach (ListViewItem item in lvEmployee1.SelectedItems)
                    item.Selected = false;
                foreach (ListViewItem item in lvEmployee1.Items)
                {
                    if (item.SubItems[WTGroupsAdd.LastNameIndex].Text.Trim().ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper())
                        && item.SubItems[WTGroupsAdd.FirstNameIndex].Text.Trim().ToUpper().StartsWith(tbFirstName.Text.Trim().ToUpper()))
                    {
                        item.Selected = true;
                        lvEmployee1.Select();
                        lvEmployee1.EnsureVisible(lvEmployee1.Items.IndexOf(lvEmployee1.SelectedItems[0]));
                        break;
                    }
                }

                tbLastName.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.tbLastName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbBelongFirstName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //tbLastName.Text = "";
                foreach (ListViewItem item in lvEmployee2.SelectedItems)
                    item.Selected = false;
                foreach (ListViewItem item in lvEmployee2.Items)
                {
                    if (item.SubItems[WTGroupsAdd.FirstNameIndex].Text.Trim().ToUpper().StartsWith(tbBelongFirstName.Text.Trim().ToUpper())
                        && item.SubItems[WTGroupsAdd.LastNameIndex].Text.Trim().ToUpper().StartsWith(tbBelongLastName.Text.Trim().ToUpper()))
                    {
                        item.Selected = true;
                        lvEmployee2.Select();
                        lvEmployee2.EnsureVisible(lvEmployee2.Items.IndexOf(lvEmployee2.SelectedItems[0]));
                        break;
                    }
                }

                tbBelongFirstName.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.tbBelongFirstName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbBelongLastName_TextChanged(object sender, EventArgs e)
        {
            try
            {                
                foreach (ListViewItem item in lvEmployee2.SelectedItems)
                    item.Selected = false;
                foreach (ListViewItem item in lvEmployee2.Items)
                {
                    if (item.SubItems[WTGroupsAdd.LastNameIndex].Text.Trim().ToUpper().StartsWith(tbBelongLastName.Text.Trim().ToUpper())
                        && item.SubItems[WTGroupsAdd.FirstNameIndex].Text.Trim().ToUpper().StartsWith(tbBelongFirstName.Text.Trim().ToUpper()))
                    {
                        item.Selected = true;
                        lvEmployee2.Select();
                        lvEmployee2.EnsureVisible(lvEmployee2.Items.IndexOf(lvEmployee2.SelectedItems[0]));
                        break;
                    }
                }

                tbBelongLastName.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroupsAdd.tbBelongLastName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
