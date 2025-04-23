using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for IOPairsAdd.
	/// </summary>
	public class IOPairsAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.ComboBox cbEmplName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblStart;
		private System.Windows.Forms.DateTimePicker dtTo;
		private System.Windows.Forms.DateTimePicker dtFrom;
		private System.Windows.Forms.Label lblEnd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private CultureInfo culture;
		private ResourceManager rm;

		// Debug log
		DebugLog log;
		private System.Windows.Forms.CheckBox cbIsWrkHrsCounter;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.CheckBox cbEndUnknown;
		private System.Windows.Forms.CheckBox cbStartUnknown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;

		// Current IOPair
		public IOPairTO currentIOPair;
		private System.Windows.Forms.DateTimePicker dtDate;
		private System.Windows.Forms.Label lblDate;
		ApplUserTO logInUser;
        private TextBox tbWU;
        private TextBox tbEmployee;
        private Button btnWUTree;

        //Indicate if calling form needs to be reload
        public bool doReloadOnBack = true;
        private Button btnLocationTree;

        List<WorkingUnitTO> wuArray;
        private CheckBox chbHierarhicly;
        List<LocationTO> locArray;

        string wuString = "";

        private int selEmplID = -1;

		// Add
		public IOPairsAdd()
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentIOPair = new IOPairTO();
			logInUser = NotificationController.GetLogInUser();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			this.btnSave.Visible = true;
			this.btnUpdate.Visible = false;
            this.tbEmployee.Visible = false;
            this.tbWU.Visible = false;
            LoadWorkingUnitAndEmployeesCombo();
		}

		// Update
		public IOPairsAdd(IOPairTO ioPair)
		{
			InitializeComponent();
			this.CenterToScreen();
            chbHierarhicly.Enabled = false;
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentIOPair = ioPair;
			logInUser = NotificationController.GetLogInUser();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			this.btnUpdate.Visible = true;
			this.btnSave.Visible = false;
			this.cbWorkingUnit.Visible = false;
			this.cbEmplName.Visible = false;
            this.tbWU.Enabled = false;
            this.tbEmployee.Enabled = false;
            this.tbWU.Text = currentIOPair.WUName;
            this.tbEmployee.Text = currentIOPair.EmployeeLastName + " " + currentIOPair.EmployeeName;
            this.btnWUTree.Visible = false;

            selEmplID = ioPair.EmployeeID;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOPairsAdd));
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.cbIsWrkHrsCounter = new System.Windows.Forms.CheckBox();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.cbStartUnknown = new System.Windows.Forms.CheckBox();
            this.cbEndUnknown = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.tbWU = new System.Windows.Forms.TextBox();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(40, 96);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(56, 23);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(104, 96);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(184, 21);
            this.cbLocation.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(24, 64);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "First Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplName
            // 
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.Location = new System.Drawing.Point(104, 64);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(184, 21);
            this.cbEmplName.TabIndex = 3;
            this.cbEmplName.SelectedIndexChanged += new System.EventHandler(this.cbEmplName_SelectedIndexChanged);
           this.cbEmplName.DropDownStyle = ComboBoxStyle.DropDown;
           this.cbEmplName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
           this.cbEmplName.AutoCompleteSource = AutoCompleteSource.ListItems;
           this.cbEmplName.Leave += new EventHandler(cbEmplName_Leave);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(103, 12);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 1;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(9, 10);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(88, 23);
            this.lblWorkingUnit.TabIndex = 0;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 304);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(24, 304);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(296, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Delete";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(8, 192);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(88, 23);
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "End:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(8, 160);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(88, 23);
            this.lblStart.TabIndex = 8;
            this.lblStart.Text = "Start:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.Checked = false;
            this.dtTo.CustomFormat = "HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(104, 192);
            this.dtTo.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowUpDown = true;
            this.dtTo.Size = new System.Drawing.Size(56, 20);
            this.dtTo.TabIndex = 12;
            // 
            // dtFrom
            // 
            this.dtFrom.Checked = false;
            this.dtFrom.CustomFormat = "HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(104, 160);
            this.dtFrom.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowUpDown = true;
            this.dtFrom.Size = new System.Drawing.Size(56, 20);
            this.dtFrom.TabIndex = 9;
            // 
            // cbIsWrkHrsCounter
            // 
            this.cbIsWrkHrsCounter.Checked = true;
            this.cbIsWrkHrsCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsWrkHrsCounter.Enabled = false;
            this.cbIsWrkHrsCounter.Location = new System.Drawing.Point(104, 224);
            this.cbIsWrkHrsCounter.Name = "cbIsWrkHrsCounter";
            this.cbIsWrkHrsCounter.Size = new System.Drawing.Size(216, 24);
            this.cbIsWrkHrsCounter.TabIndex = 14;
            this.cbIsWrkHrsCounter.Text = "Is Working Hours Counter";
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(104, 256);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(121, 21);
            this.cbPassType.TabIndex = 16;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(24, 256);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(72, 23);
            this.lblPassType.TabIndex = 15;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbStartUnknown
            // 
            this.cbStartUnknown.Location = new System.Drawing.Point(176, 160);
            this.cbStartUnknown.Name = "cbStartUnknown";
            this.cbStartUnknown.Size = new System.Drawing.Size(144, 24);
            this.cbStartUnknown.TabIndex = 10;
            this.cbStartUnknown.Text = "Unknown";
            this.cbStartUnknown.CheckedChanged += new System.EventHandler(this.cbStartUnknown_CheckedChanged);
            // 
            // cbEndUnknown
            // 
            this.cbEndUnknown.Location = new System.Drawing.Point(176, 192);
            this.cbEndUnknown.Name = "cbEndUnknown";
            this.cbEndUnknown.Size = new System.Drawing.Size(144, 24);
            this.cbEndUnknown.TabIndex = 13;
            this.cbEndUnknown.Text = "Unknown";
            this.cbEndUnknown.CheckedChanged += new System.EventHandler(this.cbEndUnknown_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(296, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 32;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(232, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(296, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 34;
            this.label4.Text = "*";
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "dd.MM.yyyy";
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(104, 128);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(136, 20);
            this.dtDate.TabIndex = 7;
            this.dtDate.Value = new System.DateTime(2006, 10, 30, 0, 0, 0, 0);
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(40, 128);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(56, 23);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbWU
            // 
            this.tbWU.Location = new System.Drawing.Point(103, 12);
            this.tbWU.Name = "tbWU";
            this.tbWU.Size = new System.Drawing.Size(183, 20);
            this.tbWU.TabIndex = 35;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(104, 64);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(182, 20);
            this.tbEmployee.TabIndex = 36;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(357, 10);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 37;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(318, 93);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 42;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(104, 34);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly.TabIndex = 44;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // IOPairsAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(402, 346);
            this.ControlBox = false;
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnLocationTree);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.tbEmployee);
            this.Controls.Add(this.tbWU);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbEndUnknown);
            this.Controls.Add(this.cbStartUnknown);
            this.Controls.Add(this.lblPassType);
            this.Controls.Add(this.cbPassType);
            this.Controls.Add(this.cbIsWrkHrsCounter);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cbEmplName);
            this.Controls.Add(this.cbWorkingUnit);
            this.Controls.Add(this.lblWorkingUnit);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(410, 380);
            this.MinimumSize = new System.Drawing.Size(410, 380);
            this.Name = "IOPairsAdd";
            this.ShowInTaskbar = false;
            this.Text = "IOPairsAdd";
            this.Load += new System.EventHandler(this.IOPairsAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.IOPairsAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void cbEmplName_Leave(object sender, EventArgs e)
        {
            if (cbEmplName.SelectedIndex == -1)
            {
              cbEmplName.SelectedIndex = 0;
            }
        }
		#endregion

		private void setLanguage()
		{
			try
			{
				// Set Title
				if (currentIOPair.IOPairID < 0)
				{
					this.Text = rm.GetString("IOPairAdd", culture);
				}
				else
				{
					this.Text = rm.GetString("IOPairUpdate", culture);
				}

                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
				this.lblName.Text = rm.GetString("lblEmployee", culture);
				this.lblLocation.Text = rm.GetString("lblLocation", culture);
				this.lblDate.Text = rm.GetString("lblDate", culture);
				this.btnSave.Text = rm.GetString("btnSave", culture);
				this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
				this.btnCancel.Text = rm.GetString("btnCancel", culture);

				this.lblStart.Text = rm.GetString("lblStart", culture);
				this.lblEnd.Text = rm.GetString("lblEnd", culture);

				this.cbIsWrkHrsCounter.Text = rm.GetString("chkbIsWrkHrs", culture);
				this.lblPassType.Text = rm.GetString("lblPassType", culture);

				this.cbStartUnknown.Text = rm.GetString("lblUnknown", culture);
				this.cbEndUnknown.Text = rm.GetString("lblUnknown", culture);
		}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateLocationCb()
		{
			try
			{
				Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				locArray = loc.Search();
				locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), -1, 0, ""));

				cbLocation.DataSource = locArray;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsAdd.populateLocationCb(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateEmployeeCombo(List<EmployeeTO> array)
		{
			try
			{
				EmployeeTO empl = new EmployeeTO();
				empl.FirstName = rm.GetString("all", culture);
				array.Insert(0, empl);

				foreach(EmployeeTO employee in array)
				{
                    if (!employee.Status.Equals(Constants.statusRetired))//Don't show retired employees
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
				}
								
				cbEmplName.DataSource = array;
				cbEmplName.DisplayMember = "LastName";
				cbEmplName.ValueMember = "EmployeeID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsAdd.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
				wuArray = new List<WorkingUnitTO>();
				//WorkingUnit wUnit = new WorkingUnit();
				if (logInUser != null)
				{
					wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
				}
                foreach (WorkingUnitTO wUnit in wuArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";                
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Employee empl = new Employee();
                List<EmployeeTO> emplList = new List<EmployeeTO>();
                string workUnitID = cbWorkingUnit.SelectedValue.ToString();
                bool check = false;

                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    if (!chbHierarhicly.Checked)
                    {
                        foreach (WorkingUnitTO wu in wuArray)
                        {
                            if (cbWorkingUnit.SelectedIndex != 0)
                            {
                                if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                                {
                                    chbHierarhicly.Checked = true;
                                    check = true;
                                }
                            }
                        }
                    }

                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wuArray)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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
                    emplList = empl.SearchByWU(workUnitID);
                }
                else
                {
                    emplList = empl.SearchByWU(wuString);
                }
                if (!check)
                    populateEmployeeCombo(emplList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void IOPairsAdd_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateLocationCb();
                populatePassTypeCombo();


                if ((currentIOPair.LocationID != -1))
                {
                    this.cbLocation.SelectedValue = currentIOPair.LocationID;
                    this.cbPassType.SelectedValue = currentIOPair.PassTypeID;

                    if (((int)currentIOPair.IsWrkHrsCount).Equals((int)Constants.IsWrkCount.IsCounter))
                    {
                        cbIsWrkHrsCounter.Checked = true;
                        cbIsWrkHrsCounter.CheckState = CheckState.Checked;
                        cbIsWrkHrsCounter.Invalidate();
                        this.Invalidate();
                    }
                    else
                    {
                        cbIsWrkHrsCounter.Checked = false;
                        cbIsWrkHrsCounter.CheckState = CheckState.Unchecked;
                        cbIsWrkHrsCounter.Invalidate();
                        this.Invalidate();
                    }

                    setDate();
                    this.Invalidate();
                }
                else
                {
                    dtDate.Value = DateTime.Now.Date;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.IOPairsAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }
		}

        private void LoadWorkingUnitAndEmployeesCombo()
        {
            try
            {
              
                populateWorkigUnitCombo();
                
                Employee empl = new Employee();
                List<EmployeeTO> emplList = new List<EmployeeTO>();

                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
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

                emplList = empl.SearchByWU(wuString);

                populateEmployeeCombo(emplList);

                if ((currentIOPair.EmployeeID != -1) )
                {
                    this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(currentIOPair.WUName);
                    this.cbEmplName.SelectedValue = currentIOPair.EmployeeID;
                 
                 }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.LoadWorkingUnitAndEmployeesCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool emplSelected = true;
                if (Int32.Parse(cbEmplName.SelectedValue.ToString()) == -1)
                {
                    MessageBox.Show(rm.GetString("messageEmployeeNotSelected", culture));
                    emplSelected = false;
                }
                if (validate() && emplSelected)
                {
                    currentIOPair.IOPairDate = dtDate.Value.Date;
                    currentIOPair.LocationID = Int32.Parse(cbLocation.SelectedValue.ToString());
                    currentIOPair.EmployeeID = Int32.Parse(cbEmplName.SelectedValue.ToString());
                    currentIOPair.PassTypeID = Int32.Parse(cbPassType.SelectedValue.ToString());

                    if (cbIsWrkHrsCounter.Checked)
                    {
                        currentIOPair.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                    }
                    else
                    {
                        currentIOPair.IsWrkHrsCount = (int)Constants.IsWrkCount.IsNotCounter;
                    }

                    if (!cbStartUnknown.Checked)
                    {
                        currentIOPair.StartTime = new DateTime(dtDate.Value.Year, dtDate.Value.Month, dtDate.Value.Day,
                                                dtFrom.Value.Hour, dtFrom.Value.Minute, 0);
                    }
                    else
                    {
                        currentIOPair.StartTime = new DateTime();
                    }

                    if (!cbEndUnknown.Checked)
                    {
                        currentIOPair.EndTime = new DateTime(dtDate.Value.Year, dtDate.Value.Month, dtDate.Value.Day,
                            dtTo.Value.Hour, dtTo.Value.Minute, 0);
                    }
                    else
                    {
                        currentIOPair.EndTime = new DateTime();
                    }

                    currentIOPair.ManualCreated = (int)Constants.recordCreated.Manualy;

                    IOPair iop = new IOPair();
                    iop.PairTO = currentIOPair;
                    int rowsAffected = iop.SaveIOPairs();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show(rm.GetString("msgIOPairSaved", culture));
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

		}

		/// <summary>
		/// Populate Pass Type Combo Box
		/// </summary>
		private void populatePassTypeCombo()
		{
			try
			{				
				List<int> types = new List<int>();
                types.Add(Constants.passOnReader);
                types.Add(Constants.wholeDayAbsence);
				List<PassTypeTO> ptArray = new PassType().Search(types);
				ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

				cbPassType.DataSource = ptArray;
				cbPassType.DisplayMember = "Description";
				cbPassType.ValueMember = "PassTypeID";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " IOPairsAdd.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " IOPairsAdd.btnCancel_Click(): " + ex.Message + "\n");
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

                if (validate())
                {
                    currentIOPair.IOPairDate = dtDate.Value.Date;
                    currentIOPair.LocationID = Int32.Parse(cbLocation.SelectedValue.ToString());
                    //currentIOPair.EmployeeID = Int32.Parse(cbEmplName.SelectedValue.ToString());
                    currentIOPair.LocationName = cbLocation.Text.ToString();
                    currentIOPair.PassTypeID = Int32.Parse(cbPassType.SelectedValue.ToString());

                    if (cbIsWrkHrsCounter.CheckState.Equals(CheckState.Checked))
                    {
                        currentIOPair.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                    }
                    else
                    {
                        currentIOPair.IsWrkHrsCount = (int)Constants.IsWrkCount.IsNotCounter;
                    }

                    if (!cbStartUnknown.Checked)
                    {
                        currentIOPair.StartTime = new DateTime(dtDate.Value.Year, dtDate.Value.Month, dtDate.Value.Day,
                            dtFrom.Value.Hour, dtFrom.Value.Minute, 0);
                    }
                    else
                    {
                        currentIOPair.StartTime = new DateTime();
                    }

                    if (!cbEndUnknown.Checked)
                    {

                        currentIOPair.EndTime = new DateTime(dtDate.Value.Year, dtDate.Value.Month, dtDate.Value.Day,
                            dtTo.Value.Hour, dtTo.Value.Minute, 0);
                    }
                    else
                    {
                        currentIOPair.EndTime = new DateTime();
                    }

                    currentIOPair.ManualCreated = (int)Constants.recordCreated.Manualy;
                    bool isUpdated = new IOPair().UpdateIOPairs(currentIOPair, "");

                    if (isUpdated)
                    {
                        MessageBox.Show(rm.GetString("msgIOPairUpdate", culture));
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}


		private void setDate()
		{
			if ((!currentIOPair.EmployeeID.Equals(-1)) && (!currentIOPair.LocationID.Equals(-1)))
			{
				// Fill the fields
				if (currentIOPair.StartTime <= new DateTime(1,1,1,0,0,0))
				{
					cbStartUnknown.Checked = true;
					dtFrom.Enabled = false;
					dtFrom.Value = DateTime.Now;
				}
				else
				{
					cbStartUnknown.Checked = false;
					dtFrom.Enabled = true;
					dtFrom.Value = currentIOPair.StartTime;
				}

				if (currentIOPair.EndTime <= new DateTime(1,1,1,0,0,0))
				{
					cbEndUnknown.Checked = true;
					dtTo.Enabled = false;
					dtTo.Value = DateTime.Now;
				}
				else
				{
					cbEndUnknown.Checked = false;
					dtTo.Enabled = true;
					dtTo.Value = currentIOPair.EndTime;
				}

				if (currentIOPair.IOPairDate <= new DateTime(1,1,1,0,0,0))
				{
					dtDate.Value = DateTime.Now;
				}
				else
				{
					dtDate.Value = currentIOPair.IOPairDate;
				}
			}
		}

		private void cbStartUnknown_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbStartUnknown.Checked)
                {
                    dtFrom.Enabled = false;
                }
                else
                {
                    dtFrom.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.cbStartUnknown_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }
		}

		private void cbEndUnknown_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor; 

                if (cbEndUnknown.Checked)
                {
                    dtTo.Enabled = false;
                }
                else
                {
                    dtTo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.cbStartUnknown_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }

		}

		private bool validate()
		{
			try
			{
				
				if (Int32.Parse(cbLocation.SelectedValue.ToString()) == -1)
				{
					MessageBox.Show(rm.GetString("msgLocNotSelected", culture));
					return false;
				}

				if (Int32.Parse(cbPassType.SelectedValue.ToString()) == -1)
				{
					MessageBox.Show(rm.GetString("msgPTNotSet", culture));
					return false;
				}

				if (cbStartUnknown.Checked && cbEndUnknown.Checked)
				{
					MessageBox.Show(rm.GetString("msgToFromNull", culture));
					return false;
				}

				if (!cbStartUnknown.Checked && !cbEndUnknown.Checked && dtFrom.Value.TimeOfDay > dtTo.Value.TimeOfDay)
				{
					MessageBox.Show(rm.GetString("msgToLessThenFrom", culture));
					return false;
				}
                DateTime start = new DateTime(1, 1, 1, dtFrom.Value.Hour, dtFrom.Value.Minute, 0);
                DateTime end = new DateTime(1, 1, 1, dtTo.Value.Hour, dtTo.Value.Minute, 0);
                //if is work hours counter checked check if pair overlooping some other pair
                if (cbIsWrkHrsCounter.Checked)
                {
                    if (selEmplID > 0 && (new IOPair().ExistEmlpoyeeDatePair(selEmplID, dtDate.Value.Date, start, end, currentIOPair.IOPairID, (int)Constants.IsWrkCount.IsCounter)))
                    {
                        MessageBox.Show(rm.GetString("msgIOPairOverlop", culture));
                        return false;
                    }
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsAdd.validate(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

			return true;
		}

        private void IOPairsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " IOPairsAdd.IOPairsAdd_KeyUp(): " + ex.Message + "\n");
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
                    cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    Employee empl = new Employee();
                    List<EmployeeTO> emplList = new List<EmployeeTO>();
                    string workUnitID = cbWorkingUnit.SelectedValue.ToString();

                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wuArray)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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
                    emplList = empl.SearchByWU(workUnitID);

                    populateEmployeeCombo(emplList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmplName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbEmplName.SelectedIndex > 0)
                    selEmplID = (int)cbEmplName.SelectedValue;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsAdd.cbEmplName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

	}
}
