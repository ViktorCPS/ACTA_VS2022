using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Working Time Schema Add/Update form
	/// </summary>
	public class WTSchemaAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblLength;
		private System.Windows.Forms.Label lblDays;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.ListView lvDetails;
		private System.Windows.Forms.GroupBox gbSchema;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDeleteInt;
		private System.Windows.Forms.Button btnUpdateInt;
		private System.Windows.Forms.Button btnAddInt;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.GroupBox gbDaysIntervals;

		// Current TimeSchema
		protected WorkTimeSchemaTO currentTimeSchema;

		// Observer
		private NotificationController Controller;
		private System.Windows.Forms.Label lblSchemaType;
		private System.Windows.Forms.ComboBox cbSchemaType;
		private NotificationObserverClient observerClient;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox numDays;

		// Debug log
		DebugLog log;
		ApplUserTO logInUser;

		private ListViewItemComparer _comp;

		const int DayIndex = 0;
		const int IntervalIndex = 1;
		const int StartWorkIndex = 2;
		const int EndWorkIndex = 3;
		const int ToleranceIndex = 4;
		private System.Windows.Forms.Label lblPause;
		private System.Windows.Forms.ComboBox cbPause;
		const int AutoCloseIndex = 5;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        private Button btnWUTree;
        private ComboBox cbWorkingUnitID;
        private Label lblWorkingUnit;

		private List<TimeSchemaPauseTO> pausesList;

        private List<WorkingUnitTO> wUnits;
        private CheckBox cbTurnus;
        private string wuString = "";

		// Add
		public WTSchemaAdd()
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			// Init Observer
			InitialiseObserverClient();

			currentTimeSchema = new WorkTimeSchemaTO();
			logInUser = NotificationController.GetLogInUser();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			pausesList = new TimeSchemaPause().Search("");

			btnUpdate.Visible = false;
			btnSave.Visible = true;
		}

		// Update 
		public WTSchemaAdd(WorkTimeSchemaTO currTimeSchema)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			log.writeLog(DateTime.Now + " WTSchemaAdd: Enter Constructor\n");

			currentTimeSchema = currTimeSchema;
			logInUser = NotificationController.GetLogInUser();

			// Init Observer
			InitialiseObserverClient();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			pausesList = new TimeSchemaPause().Search("");

			btnUpdate.Visible = true;
			btnSave.Visible = false;

			populateListView(currentTimeSchema.Days);

			log.writeLog(DateTime.Now + " WTSchemaAdd: Leave Constructor\n");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WTSchemaAdd));
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblDays = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDeleteInt = new System.Windows.Forms.Button();
            this.btnUpdateInt = new System.Windows.Forms.Button();
            this.btnAddInt = new System.Windows.Forms.Button();
            this.lvDetails = new System.Windows.Forms.ListView();
            this.gbSchema = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWorkingUnitID = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.cbPause = new System.Windows.Forms.ComboBox();
            this.lblPause = new System.Windows.Forms.Label();
            this.numDays = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSchemaType = new System.Windows.Forms.ComboBox();
            this.lblSchemaType = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDaysIntervals = new System.Windows.Forms.GroupBox();
            this.cbTurnus = new System.Windows.Forms.CheckBox();
            this.gbSchema.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(104, 16);
            this.tbName.MaxLength = 64;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(240, 20);
            this.tbName.TabIndex = 1;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(24, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(64, 23);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(104, 48);
            this.tbDesc.MaxLength = 128;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(240, 20);
            this.tbDesc.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(48, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(40, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLength
            // 
            this.lblLength.Location = new System.Drawing.Point(40, 80);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(48, 23);
            this.lblLength.TabIndex = 4;
            this.lblLength.Text = "Length:";
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDays
            // 
            this.lblDays.Location = new System.Drawing.Point(168, 80);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(32, 23);
            this.lblDays.TabIndex = 6;
            this.lblDays.Text = "Days";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(562, 540);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(11, 540);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDeleteInt
            // 
            this.btnDeleteInt.Enabled = false;
            this.btnDeleteInt.Location = new System.Drawing.Point(395, 492);
            this.btnDeleteInt.Name = "btnDeleteInt";
            this.btnDeleteInt.Size = new System.Drawing.Size(112, 23);
            this.btnDeleteInt.TabIndex = 14;
            this.btnDeleteInt.Text = "Delete Interval";
            this.btnDeleteInt.Click += new System.EventHandler(this.btnDeleteInt_Click);
            // 
            // btnUpdateInt
            // 
            this.btnUpdateInt.Location = new System.Drawing.Point(267, 492);
            this.btnUpdateInt.Name = "btnUpdateInt";
            this.btnUpdateInt.Size = new System.Drawing.Size(112, 23);
            this.btnUpdateInt.TabIndex = 13;
            this.btnUpdateInt.Text = "Update Interval";
            this.btnUpdateInt.Click += new System.EventHandler(this.btnUpdateInt_Click);
            // 
            // btnAddInt
            // 
            this.btnAddInt.Location = new System.Drawing.Point(115, 492);
            this.btnAddInt.Name = "btnAddInt";
            this.btnAddInt.Size = new System.Drawing.Size(136, 23);
            this.btnAddInt.TabIndex = 12;
            this.btnAddInt.Text = "Add Interval";
            this.btnAddInt.Click += new System.EventHandler(this.btnAddInt_Click);
            // 
            // lvDetails
            // 
            this.lvDetails.FullRowSelect = true;
            this.lvDetails.GridLines = true;
            this.lvDetails.HideSelection = false;
            this.lvDetails.Location = new System.Drawing.Point(27, 268);
            this.lvDetails.Name = "lvDetails";
            this.lvDetails.Size = new System.Drawing.Size(594, 200);
            this.lvDetails.TabIndex = 11;
            this.lvDetails.UseCompatibleStateImageBehavior = false;
            this.lvDetails.View = System.Windows.Forms.View.Details;
            this.lvDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDetails_ColumnClick);
            // 
            // gbSchema
            // 
            this.gbSchema.Controls.Add(this.cbTurnus);
            this.gbSchema.Controls.Add(this.btnWUTree);
            this.gbSchema.Controls.Add(this.cbWorkingUnitID);
            this.gbSchema.Controls.Add(this.lblWorkingUnit);
            this.gbSchema.Controls.Add(this.cbPause);
            this.gbSchema.Controls.Add(this.lblPause);
            this.gbSchema.Controls.Add(this.numDays);
            this.gbSchema.Controls.Add(this.label1);
            this.gbSchema.Controls.Add(this.label3);
            this.gbSchema.Controls.Add(this.cbSchemaType);
            this.gbSchema.Controls.Add(this.lblSchemaType);
            this.gbSchema.Controls.Add(this.lblDays);
            this.gbSchema.Controls.Add(this.tbName);
            this.gbSchema.Controls.Add(this.lblName);
            this.gbSchema.Controls.Add(this.tbDesc);
            this.gbSchema.Controls.Add(this.lblDesc);
            this.gbSchema.Controls.Add(this.lblLength);
            this.gbSchema.Location = new System.Drawing.Point(16, 8);
            this.gbSchema.Name = "gbSchema";
            this.gbSchema.Size = new System.Drawing.Size(618, 230);
            this.gbSchema.TabIndex = 0;
            this.gbSchema.TabStop = false;
            this.gbSchema.Text = "Schema";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(352, 173);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 35;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWorkingUnitID
            // 
            this.cbWorkingUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitID.ItemHeight = 13;
            this.cbWorkingUnitID.Location = new System.Drawing.Point(104, 175);
            this.cbWorkingUnitID.Name = "cbWorkingUnitID";
            this.cbWorkingUnitID.Size = new System.Drawing.Size(240, 21);
            this.cbWorkingUnitID.TabIndex = 33;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(6, 175);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(86, 23);
            this.lblWorkingUnit.TabIndex = 32;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPause
            // 
            this.cbPause.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPause.Location = new System.Drawing.Point(104, 143);
            this.cbPause.Name = "cbPause";
            this.cbPause.Size = new System.Drawing.Size(180, 21);
            this.cbPause.TabIndex = 10;
            // 
            // lblPause
            // 
            this.lblPause.Location = new System.Drawing.Point(40, 143);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(48, 23);
            this.lblPause.TabIndex = 9;
            this.lblPause.Text = "Pause:";
            this.lblPause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numDays
            // 
            this.numDays.Enabled = false;
            this.numDays.Location = new System.Drawing.Point(104, 80);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(48, 20);
            this.numDays.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(256, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(352, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "*";
            // 
            // cbSchemaType
            // 
            this.cbSchemaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchemaType.Location = new System.Drawing.Point(104, 112);
            this.cbSchemaType.Name = "cbSchemaType";
            this.cbSchemaType.Size = new System.Drawing.Size(144, 21);
            this.cbSchemaType.TabIndex = 8;
            // 
            // lblSchemaType
            // 
            this.lblSchemaType.Location = new System.Drawing.Point(8, 112);
            this.lblSchemaType.Name = "lblSchemaType";
            this.lblSchemaType.Size = new System.Drawing.Size(80, 23);
            this.lblSchemaType.TabIndex = 7;
            this.lblSchemaType.Text = "Schema Type:";
            this.lblSchemaType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(11, 540);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbDaysIntervals
            // 
            this.gbDaysIntervals.Location = new System.Drawing.Point(11, 244);
            this.gbDaysIntervals.Name = "gbDaysIntervals";
            this.gbDaysIntervals.Size = new System.Drawing.Size(626, 280);
            this.gbDaysIntervals.TabIndex = 15;
            this.gbDaysIntervals.TabStop = false;
            this.gbDaysIntervals.Text = "Days and Intervals";
            // 
            // cbTurnus
            // 
            this.cbTurnus.AutoSize = true;
            this.cbTurnus.Location = new System.Drawing.Point(103, 207);
            this.cbTurnus.Name = "cbTurnus";
            this.cbTurnus.Size = new System.Drawing.Size(59, 17);
            this.cbTurnus.TabIndex = 36;
            this.cbTurnus.Text = "Turnus";
            this.cbTurnus.UseVisualStyleBackColor = true;
            // 
            // WTSchemaAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(650, 582);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDeleteInt);
            this.Controls.Add(this.btnUpdateInt);
            this.Controls.Add(this.btnAddInt);
            this.Controls.Add(this.lvDetails);
            this.Controls.Add(this.gbSchema);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.gbDaysIntervals);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WTSchemaAdd";
            this.ShowInTaskbar = false;
            this.Text = "Add Work Time Schema";
            this.Load += new System.EventHandler(this.WTSchemaAdd_Load);
            this.Closed += new System.EventHandler(this.WTSchemaAdd_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTSchemaAdd_KeyUp);
            this.gbSchema.ResumeLayout(false);
            this.gbSchema.PerformLayout();
            this.ResumeLayout(false);

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

				if (ListView.Sorting == SortOrder.Descending)
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
					case WTSchemaAdd.DayIndex:
					case WTSchemaAdd.IntervalIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
					}
					case WTSchemaAdd.StartWorkIndex:
					case WTSchemaAdd.EndWorkIndex:
					case WTSchemaAdd.ToleranceIndex:
					case WTSchemaAdd.AutoCloseIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");

				}
			}
		}

		#endregion

		private void setLanguage()
		{
			try
			{
				if (currentTimeSchema.TimeSchemaID.Equals(-1))
				{
					this.Text = rm.GetString("WTSchemaAddTitle", culture);
				}
				else
				{
					this.Text = rm.GetString("WTSchemaUpdateTitle", culture);
				}
			
				gbSchema.Text = rm.GetString("lblSchema", culture);
				lblName.Text = rm.GetString("lblName1", culture);
				lblDesc.Text = rm.GetString("lblDesc", culture);
				lblLength.Text = rm.GetString("lblLength", culture);
				lblDays.Text = rm.GetString("lblDays", culture);
                lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);

				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);

				btnAddInt.Text = rm.GetString("btnAddInterval", culture);
				btnUpdateInt.Text = rm.GetString("btnUpdateInterval", culture);
				btnDeleteInt.Text = rm.GetString("btnDeleteInterval", culture);
				gbDaysIntervals.Text = rm.GetString("gbDaysIntervals", culture);
				lblSchemaType.Text = rm.GetString("lblType1", culture);
				lblPause.Text = rm.GetString("lblPause", culture);

				lvDetails.BeginUpdate();

				lvDetails.Columns.Add(rm.GetString("hdrDayNum", culture),  (lvDetails.Width - 2) / 8, HorizontalAlignment.Center);
				lvDetails.Columns.Add(rm.GetString("hdrIntervalNumber", culture),  (lvDetails.Width  - 3) / 8, HorizontalAlignment.Center);
				lvDetails.Columns.Add(rm.GetString("hdrSartTime", culture),  (lvDetails.Width - 2) / 8, HorizontalAlignment.Center);
                lvDetails.Columns.Add(rm.GetString("hdrEndTime", culture), (lvDetails.Width - 3) / 8, HorizontalAlignment.Center);
                lvDetails.Columns.Add(rm.GetString("hdrDescription", culture), (lvDetails.Width - 3) / 8, HorizontalAlignment.Center);
				lvDetails.Columns.Add(rm.GetString("hdrTolerance", culture),  (lvDetails.Width - 2) / 8, HorizontalAlignment.Center);
				lvDetails.Columns.Add(rm.GetString("hdrAutoClose", culture),  (lvDetails.Width - 3) / 8, HorizontalAlignment.Center);
				lvDetails.Columns.Add(rm.GetString("gbPause", culture),  2 * (lvDetails.Width - 2) / 8, HorizontalAlignment.Center);

				lvDetails.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().SearchRootWU("");                

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnitID.DataSource = wuArray;
                cbWorkingUnitID.DisplayMember = "Name";
                cbWorkingUnitID.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }
		private void WTSchemaAdd_Load(object sender, System.EventArgs e)
		{
			log.writeLog(DateTime.Now + " WTSchemaAdd: Enter Load method\n");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvDetails);
                lvDetails.ListViewItemSorter = _comp;
                lvDetails.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                populateSchemaTypeCombo();
                populatePauseCombo();
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
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

                if (currentTimeSchema != null && currentTimeSchema.TimeSchemaID != -1)
                {
                    tbName.Text = currentTimeSchema.Name;
                    tbDesc.Text = currentTimeSchema.Description;
                    cbSchemaType.SelectedIndex = cbSchemaType.FindStringExact(currentTimeSchema.Type);
                    if (currentTimeSchema.WorkingUnitID != -1)
                    {
                        cbWorkingUnitID.SelectedValue = currentTimeSchema.WorkingUnitID;
                    }
                    if (currentTimeSchema.Turnus == Constants.yesInt)
                        cbTurnus.Checked = true;
                    if (currentTimeSchema.CycleDuration != -1)
                    {
                        numDays.Text = currentTimeSchema.CycleDuration.ToString();
                    }
                    else
                    {
                        numDays.Text = "0";
                    }

                    cbPause.SelectedIndex = 0; //On Upd, select *
                }
                else
                {
                    cbSchemaType.SelectedIndex = cbSchemaType.FindStringExact(Constants.schemaTypeNormal);
                    cbPause.SelectedValue = 0; //On Add, select Default Pause
                }

                log.writeLog(DateTime.Now + " WTSchemaAdd: Leave Load method\n");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.WTSchemaAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateListView(Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days)
		{
		
			try
			{
				lvDetails.BeginUpdate();
				lvDetails.Items.Clear();

                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
				int tolerance = 0;

                Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int,WorkTimeIntervalTO>();

				foreach(int dayKey in days.Keys)
				{
					intervals = days[dayKey];

					foreach(int intKey in intervals.Keys)
					{
						interval = intervals[intKey];

						ListViewItem item = new ListViewItem();
						item.Text = (interval.DayNum + 1).ToString();

						item.SubItems.Add((interval.IntervalNum + 1).ToString());
						item.SubItems.Add(interval.StartTime.ToString("HH:mm"));
                        item.SubItems.Add(interval.EndTime.ToString("HH:mm"));
                        item.SubItems.Add(interval.Description);

						tolerance = setTolerance(interval);
										
						if (tolerance > 0)
						{
							item.SubItems.Add(tolerance.ToString().Trim());
						}
						else
						{
							item.SubItems.Add("");
						}

						switch (interval.AutoClose)
						{
							case (int) Constants.AutoClose.WithoutClose:
								item.SubItems.Add(rm.GetString("rbWithout", culture));
								break;
							case (int) Constants.AutoClose.StartClose:
								item.SubItems.Add(rm.GetString("rbStart", culture));
								break;
							case (int) Constants.AutoClose.EndClose:
								item.SubItems.Add(rm.GetString("rbEnd", culture));
								break;
							case (int) Constants.AutoClose.StartEndClose:
								item.SubItems.Add(rm.GetString("rbStartEnd", culture));
								break;
							default:
								item.SubItems.Add(rm.GetString("rbWithout", culture));
								break;
						}

						string pauseDescription = "";
						foreach (TimeSchemaPauseTO currentPause in pausesList)
						{
							if (currentPause.PauseID == interval.PauseID)
							{
								pauseDescription = currentPause.Description;
								break;
							}
						}
						item.SubItems.Add(pauseDescription);
						
						item.Tag = interval;
						lvDetails.Items.Add(item);
					}
				}

				lvDetails.EndUpdate();
				lvDetails.Sorting = SortOrder.Ascending;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnAddInt_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                currentTimeSchema.Name = tbName.Text;
                currentTimeSchema.Description = tbDesc.Text;
                currentTimeSchema.Type = cbSchemaType.Text;

                WTSchemaPeriod period = new WTSchemaPeriod(currentTimeSchema, cbPause.SelectedValue.ToString());
                period.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnAddInt_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdateInt_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvDetails.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneSchemaInt", culture));
                }
                else
                {
                    WorkTimeIntervalTO tempInterval = (WorkTimeIntervalTO)lvDetails.SelectedItems[0].Tag;

                    WTSchemaPeriod period = new WTSchemaPeriod(currentTimeSchema, tempInterval.DayNum, tempInterval.IntervalNum, tempInterval.PauseID.ToString());
                    period.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnUpdateInt_Click(): " + ex.Message + "\n");
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
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.SchemaChangedEvent);
		}

		public void SchemaChangedEvent(object sender, NotificationEventArgs args)
		{
			try
			{
				if (args.schema != null)
				{
					//MessageBox.Show("Time Schema changed");

					currentTimeSchema = args.schema;
					numDays.Text = currentTimeSchema.CycleDuration.ToString();
					populateListView(currentTimeSchema.Days);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.SchemaChangedEvent(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void WTSchemaAdd_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.validate())
                {
                    currentTimeSchema.Description = tbDesc.Text;
                    currentTimeSchema.Type = cbSchemaType.Text;
                    if (cbWorkingUnitID.SelectedIndex > 0)
                    {
                        currentTimeSchema.WorkingUnitID = (int)cbWorkingUnitID.SelectedValue;
                    }
                    if (cbTurnus.Checked)
                        currentTimeSchema.Turnus = Constants.yesInt;
                    else
                        currentTimeSchema.Turnus = Constants.noInt;
                    if (this.validate())
                    {
                        TimeSchema sch = new TimeSchema();
                        sch.TimeSchemaTO = currentTimeSchema;
                        int affectedRows = sch.Save();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show(rm.GetString("schemaSaved", culture));
                            Controller.TimeShemaChanged(currentTimeSchema);
                        }
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateSchemaTypeCombo()
		{
			try
			{
				ArrayList typeArray = new ArrayList();

				typeArray.Add(Constants.schemaTypeNormal);
				typeArray.Add(Constants.schemaTypeFlexi);

                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                if (cost == (int)Constants.Customers.PMC)
                    typeArray.Add(Constants.schemaTypeNightFlexi);

                int permission;
                string timeTableMenuItemID = rm.GetString("menuConfiguration", culture) + "_"
                    + rm.GetString("menuItemTimeTable", culture);
                
                bool readPermission = false;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[timeTableMenuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                }

                if (readPermission)
                {
                    typeArray.Add(Constants.schemaTypeIndustrial);
                }

				cbSchemaType.DataSource = typeArray;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.populateSchemaTypeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populatePauseCombo()
		{
			try
			{
				List<TimeSchemaPauseTO> pauseArray = new List<TimeSchemaPauseTO>();

				pauseArray = new TimeSchemaPause().Search("");
				pauseArray.Insert(0, new TimeSchemaPauseTO(-1, rm.GetString("all", culture), -1, -1, -1, -1, -1));

				cbPause.DataSource    = pauseArray;
				cbPause.DisplayMember = "Description";
				cbPause.ValueMember   = "PauseID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.populatePauseCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (validate())
                {
                    if (!tbName.Text.Trim().Equals(""))
                    {
                        currentTimeSchema.Name = tbName.Text;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("msgSchemaName", culture));
                        return;
                    }
                    currentTimeSchema.Description = tbDesc.Text;
                    currentTimeSchema.Type = cbSchemaType.Text;
                    if (cbWorkingUnitID.SelectedIndex >= 0)
                        currentTimeSchema.WorkingUnitID = (int)cbWorkingUnitID.SelectedValue;
                    if (cbTurnus.Checked)
                        currentTimeSchema.Turnus = Constants.yesInt;
                    else
                        currentTimeSchema.Turnus = Constants.noInt;

                    TimeSchema sch = new TimeSchema();
                    sch.TimeSchemaTO = currentTimeSchema;
                    bool isUpdated = sch.Update();

                    MessageBox.Show(rm.GetString("schemaChanged", culture));
                    Controller.TimeShemaChanged(currentTimeSchema);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDeleteInt_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvDetails.SelectedItems.Count > 0)
                {
                    WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                    interval = (WorkTimeIntervalTO)lvDetails.SelectedItems[0].Tag;

                    Dictionary<int, WorkTimeIntervalTO> intervals = currentTimeSchema.Days[interval.DayNum];
                    intervals.Remove(interval.IntervalNum);

                    // If Day contain 0 intervals
                    if (intervals.Count == 0)
                    {
                        currentTimeSchema.Days.Remove(interval.DayNum);
                    }

                    populateListView(this.currentTimeSchema.Days);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.btnDeleteInt_Click(): " + ex.Message + "\n");
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
				if (tbName.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("msgSchemaName", culture));
					return false;
				}

				if (cbSchemaType.SelectedIndex < 0)
				{
					MessageBox.Show(rm.GetString("msgSchemaType", culture));
					return false;
				}

				if (currentTimeSchema.Days.Count == 0)
				{
					MessageBox.Show(rm.GetString("msgNoDaysFound", culture));
					return false;
				}

				// Check Keys
				int correctIntervals = 0;
                Dictionary<int, WorkTimeIntervalTO> Intervals = new Dictionary<int,WorkTimeIntervalTO>();
				int correctDays = 0;
				
				for(int j=0; j<currentTimeSchema.Days.Keys.Count; j++)
				{
					if (currentTimeSchema.Days.ContainsKey(j))
					{
						Intervals = currentTimeSchema.Days[j];
						int intCount = currentTimeSchema.Days[j].Keys.Count;
						correctIntervals = 0;	

						for(int i=0; i<intCount; i++)
						{
							if (Intervals.ContainsKey(i))
							{
								correctIntervals ++;
							}
						}

						if (correctIntervals == intCount)
						{
							correctDays ++;						
						}
					}					
				}
				
				if (currentTimeSchema.Days.Keys.Count != correctDays)
				{
					MessageBox.Show(rm.GetString("msgIntervalsNotValid", culture));
					return false;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.validate(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return true;
		}

        private int setTolerance(WorkTimeIntervalTO currentInterval)
		{
			int tolerance = 0;;

			try
			{
				TimeSpan ts0  = new TimeSpan();
				ts0 = currentInterval.StartTime.Subtract(currentInterval.EarliestArrived);

				int tm0;
				if (ts0.TotalMinutes < 0)
				{
					tm0 = 60 + Convert.ToInt32(ts0.TotalMinutes);
				}
				else
				{
					tm0 = Convert.ToInt32(ts0.TotalMinutes);
				}

				TimeSpan ts1 = new TimeSpan();
				ts1 = currentInterval.LatestArrivaed.Subtract(currentInterval.StartTime);
				int tm1;
				if (ts1.TotalMinutes < 0)
				{
					tm1 = 60 + Convert.ToInt32(ts1.TotalMinutes);
				}
				else
				{
					tm1 = Convert.ToInt32(ts1.TotalMinutes);
				}

				TimeSpan ts2 = new TimeSpan();
				ts2 = currentInterval.EndTime.Subtract(currentInterval.EarliestLeft);
				int tm2;
				if (ts2.TotalMinutes < 0)
				{
					tm2 = 60 + Convert.ToInt32(ts2.TotalMinutes);
				}
				else
				{
					tm2 = Convert.ToInt32(ts2.TotalMinutes);
				}
				
				TimeSpan ts3 = new TimeSpan();
				ts3 = currentInterval.LatestLeft.Subtract(currentInterval.EndTime);
				int tm3;
				if (ts3.TotalMinutes < 0)
				{
					tm3 = 60 + Convert.ToInt32(ts3.TotalMinutes);
				}
				else
				{
					tm3 = Convert.ToInt32(ts3.TotalMinutes);
				}
				
				if ((tm0 == tm1) && (tm0 == tm2) && (tm0 == tm3) && (tm1 == tm2) && (tm1 == tm3) && (tm2 == tm3)
					&& (tm0 >= 0) && (tm1 >= 0) && (tm2 >= 0) && (tm3 >= 0))  
				{
					tolerance = Convert.ToInt32(ts0.TotalMinutes.ToString());
				}
				else
				{
					tolerance = 0;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaPeriod.setTolerance(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return tolerance;
		}

		private void lvDetails_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvDetails.Sorting;
                lvDetails.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvDetails.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvDetails.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvDetails.Sorting = SortOrder.Ascending;
                }
                lvDetails.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdd.lvDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void WTSchemaAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTSchemaAdd.WTSchemaAdd_KeyUp(): " + ex.Message + "\n");
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
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getRootWorkingUnits("");
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWorkingUnitID.SelectedIndex = cbWorkingUnitID.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
