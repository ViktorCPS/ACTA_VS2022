using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using Microsoft.ReportingServices.ReportRendering;
using System.Data;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace Reports.UNIPROM
{
	/// <summary>
	/// Summary description for IOPairsUNIPROM.
	/// </summary>
	public class IOPairsUNIPROM : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbIOPairs;
		private System.Windows.Forms.ComboBox cbShowPairs;
		private System.Windows.Forms.Label lblShowPairs;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lvResults;
		private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReport;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.ComboBox cbEmplName;
		private System.Windows.Forms.DateTimePicker dtFrom;
		private System.Windows.Forms.DateTimePicker dtTo;

		// Debug log
		DebugLog log;

		// List View indexes
		const int LocationIndex = 0;
		const int LastNameIndex = 1;
		const int FirstNameIndex = 2;
		const int PassType = 3;
		const int StartTime = 4;
		const int EndTime = 5;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

        List<IOPairTO> currentIOPairsList;
		private int sortOrder;
		private int sortField;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private int startIndex;

		private List<WorkingUnitTO> wUnits;
        private System.Windows.Forms.Label lblTotal;
        private string wuString = "";
        private System.Windows.Forms.CheckBox chbHierarhicly;

        //for location tree view
        private List<LocationTO> locArray;
        private ComboBox cbPassType;
        private Label lblPassType;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        
        //for employeePersonalRecordsMDI
        private List<EmployeeTO> currentEmplArray;

        private Filter filter;

        private const string noData = "N/A";
        private const string dayOff = "Slobodan";
        private const string absent = "Neopravdano odsutan";

        List<WorkingUnitTO> currentWUList = new List<WorkingUnitTO>();
        
		public IOPairsUNIPROM()
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

            currentIOPairsList = new List<IOPairTO>();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource",typeof(IOPairsUNIPROM).Assembly);
			setLanguage();

        }
        #region MDI child method's        
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                dtFrom.Value = from;
                dtTo.Value = to;
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWorkingUnit.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currentEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmplName.SelectedValue = selectedEmployeeID;
                }

                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.changeSelectedEmployee(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
       
        #endregion

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOPairsUNIPROM));
            this.gbIOPairs = new System.Windows.Forms.GroupBox();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblShowPairs = new System.Windows.Forms.Label();
            this.cbShowPairs = new System.Windows.Forms.ComboBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbIOPairs.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbIOPairs
            // 
            this.gbIOPairs.Controls.Add(this.cbPassType);
            this.gbIOPairs.Controls.Add(this.lblPassType);
            this.gbIOPairs.Controls.Add(this.chbHierarhicly);
            this.gbIOPairs.Controls.Add(this.btnSearch);
            this.gbIOPairs.Controls.Add(this.lblTo);
            this.gbIOPairs.Controls.Add(this.lblFrom);
            this.gbIOPairs.Controls.Add(this.dtTo);
            this.gbIOPairs.Controls.Add(this.dtFrom);
            this.gbIOPairs.Controls.Add(this.lblLocation);
            this.gbIOPairs.Controls.Add(this.cbLocation);
            this.gbIOPairs.Controls.Add(this.lblName);
            this.gbIOPairs.Controls.Add(this.cbEmplName);
            this.gbIOPairs.Controls.Add(this.cbWorkingUnit);
            this.gbIOPairs.Controls.Add(this.lblWorkingUnit);
            this.gbIOPairs.Controls.Add(this.lblShowPairs);
            this.gbIOPairs.Controls.Add(this.cbShowPairs);
            this.gbIOPairs.Location = new System.Drawing.Point(24, 8);
            this.gbIOPairs.Name = "gbIOPairs";
            this.gbIOPairs.Size = new System.Drawing.Size(504, 192);
            this.gbIOPairs.TabIndex = 0;
            this.gbIOPairs.TabStop = false;
            this.gbIOPairs.Tag = "FILTERABLE";
            this.gbIOPairs.Text = "IO Pairs";
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(128, 23);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(184, 21);
            this.cbPassType.TabIndex = 14;
            this.cbPassType.Visible = false;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(34, 21);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 17;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPassType.Visible = false;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(411, 54);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly.TabIndex = 5;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(411, 152);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(24, 120);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(96, 23);
            this.lblTo.TabIndex = 14;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 88);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(104, 23);
            this.lblFrom.TabIndex = 12;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(128, 120);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(184, 20);
            this.dtTo.TabIndex = 12;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(128, 88);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(184, 20);
            this.dtFrom.TabIndex = 10;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(16, 21);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(104, 23);
            this.lblLocation.TabIndex = 9;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLocation.Visible = false;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(128, 21);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(184, 21);
            this.cbLocation.TabIndex = 8;
            this.cbLocation.Visible = false;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(104, 23);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "First Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Visible = false;
            // 
            // cbEmplName
            // 
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.Location = new System.Drawing.Point(128, 21);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(184, 21);
            this.cbEmplName.TabIndex = 6;
            this.cbEmplName.Visible = false;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(128, 56);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(246, 21);
            this.cbWorkingUnit.TabIndex = 3;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 56);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 3;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShowPairs
            // 
            this.lblShowPairs.Location = new System.Drawing.Point(16, 23);
            this.lblShowPairs.Name = "lblShowPairs";
            this.lblShowPairs.Size = new System.Drawing.Size(104, 23);
            this.lblShowPairs.TabIndex = 1;
            this.lblShowPairs.Text = "Show Pairs:";
            this.lblShowPairs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShowPairs.Visible = false;
            // 
            // cbShowPairs
            // 
            this.cbShowPairs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShowPairs.Location = new System.Drawing.Point(128, 24);
            this.cbShowPairs.Name = "cbShowPairs";
            this.cbShowPairs.Size = new System.Drawing.Size(184, 21);
            this.cbShowPairs.TabIndex = 2;
            this.cbShowPairs.Visible = false;
            // 
            // lvResults
            // 
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(24, 206);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(794, 306);
            this.lvResults.TabIndex = 19;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            this.lvResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResults_ColumnClick);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(24, 552);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(76, 23);
            this.btnReport.TabIndex = 24;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(743, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(786, 177);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 18;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(746, 177);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 17;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(666, 515);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(534, 8);
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
            // IOPairsUNIPROM
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(830, 582);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.gbIOPairs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "IOPairsUNIPROM";
            this.ShowInTaskbar = false;
            this.Text = "IOPairs";
            this.Load += new System.EventHandler(this.IOPairs_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.IOPairs_KeyUp);
            this.gbIOPairs.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<IOPairTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(IOPairTO x, IOPairTO y)        
			{
				IOPairTO iop1 = null;
				IOPairTO iop2 = null;

				if (compOrder == Constants.sortAsc)
				{
					iop1 = x;
					iop2 = y;
				}
				else
				{
					iop1 = y;
					iop2 = x;
				}

				switch(compField)            
				{                
					case IOPairsUNIPROM.LocationIndex: 
						return iop1.LocationName.CompareTo(iop2.LocationName); 
					case IOPairsUNIPROM.LastNameIndex:
						return iop1.EmployeeLastName.CompareTo(iop2.EmployeeLastName);                
					case IOPairsUNIPROM.FirstNameIndex:
						return iop1.EmployeeName.CompareTo(iop2.EmployeeName);                
					case IOPairsUNIPROM.PassType:
						return iop1.PassType.CompareTo(iop2.PassType);                
					case IOPairsUNIPROM.StartTime:
						return iop1.StartTime.CompareTo(iop2.StartTime);                
					case IOPairsUNIPROM.EndTime:
						return iop1.EndTime.CompareTo(iop2.EndTime);                
					default:                    
						return iop1.StartTime.CompareTo(iop2.StartTime);            
				}        
			}    
		}

		#endregion
		
		private void setLanguage()
		{
			this.Text = rm.GetString("DailyPreviewTitle", culture);
			this.gbIOPairs.Text = rm.GetString("gbIOPairs", culture);
            this.gbFilter.Text = rm.GetString("gbFilter", culture);

            chbHierarhicly.Text = rm.GetString("hierarchically", culture);

			this.lblShowPairs.Text = rm.GetString("lblShowPairs", culture);
			this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
			this.lblName.Text = rm.GetString("lblEmployee", culture);
			this.lblLocation.Text = rm.GetString("lblLocation", culture);
			this.lblTo.Text = rm.GetString("lblTo", culture);
			this.lblFrom.Text = rm.GetString("lblFrom", culture);
            lblPassType.Text = rm.GetString("lblPassType", culture);

			this.btnSearch.Text = rm.GetString("btnSearch", culture);
            this.btnReport.Text = rm.GetString("btnReport", culture);
			this.btnClose.Text = rm.GetString("btnClose", culture);
            
			// List View Header
			lvResults.BeginUpdate();

			lvResults.Columns.Add(rm.GetString("hdrName", culture), (lvResults.Width - 9) / 9+10, HorizontalAlignment.Left);
			lvResults.Columns.Add(rm.GetString("hdrPassType", culture), (lvResults.Width - 9) / 9, HorizontalAlignment.Left);
			lvResults.Columns.Add(rm.GetString("hdrDate", culture), (lvResults.Width - 9) / 9+10, HorizontalAlignment.Left);
			lvResults.Columns.Add(rm.GetString("hdrTime", culture), (lvResults.Width - 9) / 9+5, HorizontalAlignment.Left);
			lvResults.Columns.Add(rm.GetString("hdrDirection", culture), (lvResults.Width - 9) / 9-25, HorizontalAlignment.Left);
			lvResults.Columns.Add(rm.GetString("hdrDate", culture), (lvResults.Width - 9) / 9+10, HorizontalAlignment.Left);
            lvResults.Columns.Add(rm.GetString("hdrTime", culture), (lvResults.Width - 9) / 9+5, HorizontalAlignment.Left);
            lvResults.Columns.Add(rm.GetString("hdrDirection", culture), (lvResults.Width - 9) / 9-25, HorizontalAlignment.Left);
            lvResults.Columns.Add(rm.GetString("hdrHours", culture), (lvResults.Width - 9) / 9-5, HorizontalAlignment.Left);
			
			lvResults.EndUpdate();
		}

		private void populatePairStatusCombo()
		{
			try
			{
				ArrayList statusArray = new ArrayList();
				statusArray.Add(rm.GetString("all", culture));
				//statusArray.Add(ConfigurationManager.AppSettings["Complete"]);
                statusArray.Add(Constants.Complete);
				//statusArray.Add(ConfigurationManager.AppSettings["Incomplete"]);
                statusArray.Add(Constants.Incomplete);

				cbShowPairs.DataSource = statusArray;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.populatePairStatusCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.populateWorkigUnitCombo(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.populateLocationCb(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

			private void  cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                bool check = false;
                if (cbWorkingUnit.SelectedIndex != 0)
                {                   
                    foreach (WorkingUnitTO wu in wUnits)
                    {

                        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (cbWorkingUnit.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsUNIPROM.populatePassTypeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		private void populateEmployeeCombo(List<EmployeeTO> array)
		{
			try
			{
                EmployeeTO empl = new EmployeeTO();
				empl.LastName = rm.GetString("all", culture);
				array.Insert(0, empl);

				foreach(EmployeeTO employee in array)
				{
					employee.LastName += " " + employee.FirstName;
				}
                currentEmplArray = array;
				cbEmplName.DataSource = array;
				cbEmplName.DisplayMember = "LastName";
				cbEmplName.ValueMember = "EmployeeID";
 			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
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
                if (wuID < 0)
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
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);

                cbEmplName.DataSource = currentEmplArray;
                cbEmplName.DisplayMember = "LastName";
                cbEmplName.ValueMember = "EmployeeID";
                cbEmplName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (currentIOPairsList.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("iopairs");
                    DataTable tableCR1 = new DataTable("empl_presence");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("name", typeof(System.String));
                    tableCR.Columns.Add("type", typeof(System.String));
                    tableCR.Columns.Add("date1", typeof(System.String));
                    tableCR.Columns.Add("time1", typeof(System.String));
                    tableCR.Columns.Add("direction1", typeof(System.String));
                    tableCR.Columns.Add("date2", typeof(System.String));
                    tableCR.Columns.Add("time2", typeof(System.String));
                    tableCR.Columns.Add("direction2", typeof(System.String));
                    tableCR.Columns.Add("hours", typeof(System.Int32));
                    tableCR.Columns.Add("wu", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.DateTime));
                    tableCR.Columns.Add("imageID", typeof(byte));
                  
                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    
                    tableCR1.Columns.Add("wu", typeof(System.String));
                    tableCR1.Columns.Add("present", typeof(System.Int32));
                    tableCR1.Columns.Add("sickLeave", typeof(System.Int32));
                    tableCR1.Columns.Add("vacation", typeof(System.Int32));
                    tableCR1.Columns.Add("dayOff", typeof(System.Int32));
                    tableCR1.Columns.Add("absent", typeof(System.Int32));
                    tableCR1.Columns.Add("date", typeof(System.DateTime));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableCR1);
                    dataSetCR.Tables.Add(tableI);

                    string totalPresente = "";
                    string totalAbsent = "";
                    string totalDayOff = "";
                    string totalSickLeave = "";
                    string totalVacation = "";

                    List<WorkTimeSchemaTO> TimeSchemaList = new TimeSchema().Search();
                    List<WUPresence> wuPresenceList = new List<WUPresence>();
                    Dictionary<DateTime, List<int>> ht = new Dictionary<DateTime,List<int>>();
                    foreach (WorkingUnitTO wu in currentWUList)
                    {
                        if(!wu.Name.Trim().Equals(""))
                        for (DateTime date = dtFrom.Value.Date; date <= dtTo.Value.Date; date = date.AddDays(1))
                        {
                            WUPresence wuPresence = new WUPresence();
                            wuPresence.WorkingUnit = wu.Name;
                            wuPresence.TotalEmpl = 0;
                            wuPresence.Date = date.Date;
                            foreach (EmployeeTO empl in currentEmplArray)
                            {
                                if (empl.WorkingUnitID == wu.WorkingUnitID && empl.Status.Equals(Constants.statusActive))
                                {                                    
                                    List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(empl.EmployeeID.ToString(), dtFrom.Value, dtTo.Value);

                                    bool is2DayShift = false;
                                    bool is2DayShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                    //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                    //are night shift days. If day is night shift day, also take first interval of next day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                        ref is2DayShiftPrevious, ref firstIntervalNextDay, TimeSchemaList);

                                    List<IOPairTO> insertedPairs = new List<IOPairTO>();

                                    bool emplPresent = false;
                                    wuPresence.TotalEmpl++;
                                    List<IOPairTO> list = new List<IOPairTO>();
                                    foreach (IOPairTO iop in currentIOPairsList)
                                    {
                                        if (iop.EmployeeID == empl.EmployeeID && iop.IOPairDate == date)
                                        {
                                            list.Add(iop);
                                            if (isPass(iop.PassTypeID))
                                            {
                                                wuPresence.Present++;
                                                emplPresent = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!emplPresent)
                                    {
                                        foreach (IOPairTO iop in currentIOPairsList)
                                        {
                                            if (iop.EmployeeID == empl.EmployeeID && iop.IOPairDate == date)
                                            {
                                                if (iop.PassTypeID == Constants.vacation)
                                                {
                                                    wuPresence.Vacation++;
                                                    emplPresent = true;
                                                    break;
                                                }
                                                else if (iop.PassTypeID == Constants.sickLeave)
                                                {
                                                    wuPresence.SickLeave++;
                                                    emplPresent = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (!emplPresent)
                                    {
                                        IOPairTO pair = new IOPairTO();
                                        pair.EmployeeLastName = empl.LastName;
                                        pair.EmployeeName = "";
                                        pair.StartTime = date;
                                        pair.WUName = wu.Name;
                                        //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                                        if (edi != null)
                                        {
                                            List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                            if (intervals.Count > 0)
                                            {
                                                pair.PassType = dayOff;
                                                wuPresence.DayOff++;
                                            }
                                            else
                                            {
                                                pair.PassType = absent;
                                            }
                                        }
                                        else
                                        {
                                            pair.PassType = absent;
                                        }

                                        if (!ht.ContainsKey(date))
                                        {
                                            List<int> l = new List<int>();
                                            ht.Add(date, l);
                                        }
                                        if (ht[date].Count == 0 || !ht[date].Contains(empl.EmployeeID))
                                            currentIOPairsList.Add(pair);
                                        
                                        ht[date].Add(empl.EmployeeID);
                                    }
                                }
                            }
                            wuPresence.Absent = wuPresence.TotalEmpl - wuPresence.Vacation - wuPresence.Present - wuPresence.SickLeave - wuPresence.DayOff;
                            wuPresenceList.Add(wuPresence);
                        }                      
                    }

                    foreach (IOPairTO ioPair in currentIOPairsList)
                    {
                        DataRow row = tableCR.NewRow();

                        row["name"] = ioPair.EmployeeName + " " + ioPair.EmployeeLastName;
                        row["type"] = ioPair.PassType;
                        row["wu"] = ioPair.WUName;

                        if(ioPair.StartTime != new DateTime(0))
                        row["date"] = ioPair.StartTime.Date;
                        else
                        row["date"] = ioPair.EndTime.Date; 

                        if (ioPair.PassType.Equals(absent) || ioPair.PassType.Equals(dayOff))
                        {
                            row["date1"] = ioPair.StartTime.ToString("dd.MM.yyyy");
                            row["date2"] = ioPair.StartTime.ToString("dd.MM.yyyy");
                            row["time1"] = noData;
                            row["direction1"] = noData;
                            row["time2"] = noData;
                            row["direction2"] = noData;
                            row["hours"] = 0;
                        }
                        else
                        {
                            if (ioPair.StartTime == new DateTime(0))
                            {
                                row["date1"] = noData;
                                row["time1"] = noData;
                                row["direction1"] = noData;
                            }
                            else
                            {
                                row["date1"] = ioPair.StartTime.ToString("dd.MM.yyyy");
                                row["time1"] = ioPair.StartTime.ToString("HH:mm");
                                row["direction1"] = Constants.DirectionIn;
                            }
                            if (ioPair.EndTime == new DateTime(0))
                            {
                                row["date2"] = noData;
                                row["time2"] = noData;
                                row["direction2"] = noData;
                            }
                            else
                            {
                                row["date2"] = ioPair.EndTime.ToString("dd.MM.yyyy");
                                row["time2"] = ioPair.EndTime.ToString("HH:mm");
                                row["direction2"] = Constants.DirectionOut;
                            }
                            TimeSpan ts = new TimeSpan();
                            if (ioPair.StartTime != new DateTime() && ioPair.EndTime != new DateTime() && ioPair.EndTime > ioPair.StartTime)
                            {
                                ts = ioPair.EndTime - ioPair.StartTime;
                            }
                            row["hours"] = ts.TotalMinutes;
                        }
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                   
                        int tPresent = 0;
                        int tDayOff = 0;
                        int tAbsent = 0;
                        int tSickLeave = 0;
                        int tVacation = 0;

                        foreach (WUPresence wup in wuPresenceList)
                        {
                            DataRow row = tableCR1.NewRow();

                            row["wu"] = wup.WorkingUnit;
                            row["present"] = wup.Present;
                            row["sickLeave"] = wup.SickLeave;
                            row["vacation"] = wup.Vacation;
                            row["dayOff"] = wup.DayOff;
                            row["absent"] = wup.Absent;
                            row["date"] = wup.Date;
                            tAbsent += wup.Absent;
                            tPresent += wup.Present;
                            tDayOff += wup.DayOff;
                            tVacation += wup.Vacation;
                            tSickLeave += wup.SickLeave;

                            tableCR1.Rows.Add(row);
                            tableCR1.AcceptChanges();
                        }

                        totalPresente = tPresent.ToString();
                        totalAbsent = tAbsent.ToString();
                        totalDayOff = tDayOff.ToString();
                        if(tSickLeave>0)
                        totalSickLeave = tSickLeave.ToString();
                        if(tVacation>0)
                        totalVacation = tVacation.ToString();
                    
                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selShowPairs = "*";
                    string selWorkingUnit = "*";
                    string selEmplName = "*";
                    string selLocation = "*";                    

                    if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnit.Text;
                    if (cbEmplName.SelectedIndex >= 0 && (int)cbEmplName.SelectedValue >= 0)
                        selEmplName = cbEmplName.Text;
                    if (cbShowPairs.SelectedIndex >= 0 )
                        selShowPairs = cbShowPairs.Text;
                    if (cbLocation.SelectedIndex >= 0 && (int)cbLocation.SelectedValue >= 0)
                        selLocation = cbLocation.Text;
                    
                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.UNIPROM.UNIPROM_sr.UNIPROMDailyPreviewCRView view = new Reports.UNIPROM.UNIPROM_sr.UNIPROMDailyPreviewCRView(dataSetCR,
                             selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value,dtTo.Value,totalPresente,totalDayOff,totalAbsent,totalSickLeave,totalVacation);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.UNIPROM.UNIPROM_en.UNIPROMDailyPreviewCRView_en view = new Reports.UNIPROM.UNIPROM_en.UNIPROMDailyPreviewCRView_en(dataSetCR,
                               selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value, dtTo.Value, totalPresente, totalDayOff, totalAbsent, totalSickLeave, totalVacation);
                        view.ShowDialog(this);
                    }
                    //else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    //{
                    //    Reports.Reports_fi.IOPairsCRView_fi view = new Reports.Reports_fi.IOPairsCRView_fi(dataSetCR,
                    //         selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value, dtTo.Value);
                    //    view.ShowDialog(this);
                    //}
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool isPass(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				
                currentWUList = new List<WorkingUnitTO>();
				int count = 0;
				
				DateTime startDate = new DateTime();
				DateTime endDate = new DateTime();

				int emplID = -1;
				if (cbEmplName.SelectedIndex >= 0 && (int) cbEmplName.SelectedValue >= 0)
				{
					emplID = (int) cbEmplName.SelectedValue;
				}

                string selectedWU = wuString;
                if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        selectedWU = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                            currentWUList.Add(wunit);
                        }

                        if (selectedWU.Length > 0)
                        {
                            selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                        }
                    }
                    else
                    {
                        if (cbWorkingUnit.SelectedIndex == 0)
                        {
                            currentWUList = wUnits;
                        }
                        else
                        {
                            currentWUList.Add(((List<WorkingUnitTO>)cbWorkingUnit.DataSource)[cbWorkingUnit.SelectedIndex]);
                        }
                        selectedWU = cbWorkingUnit.SelectedValue.ToString();
                    }
                }
                else
                {
                    currentWUList = wUnits;
                }

                if (!wuString.Equals(""))
                {
                    if (cbEmplName.Items.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                        clearListView();
                        return;
                    }

                    // ALL
                    if (cbShowPairs.SelectedIndex.Equals(0))
                    {
                        startDate = DateTime.Now;
                        endDate = DateTime.Now;
                    }
                    // COMPLETE
                    else if (cbShowPairs.SelectedIndex.Equals(1))
                    {
                        startDate = new DateTime(1, 1, 1, 0, 0, 0);
                        endDate = new DateTime(1, 1, 1, 0, 0, 0);
                    }
                    // INCOMPLETE
                    else if (cbShowPairs.SelectedIndex.Equals(2))
                    {
                        startDate = new DateTime(1, 1, 1, 0, 0, 1);
                        endDate = new DateTime(1, 1, 1, 0, 0, 1);
                    }
                    int checkedWUID = -1;
                    if (!chbHierarhicly.Checked)
                        checkedWUID = Int32.Parse(cbWorkingUnit.SelectedValue.ToString());

                    IOPair ioPair = new IOPair();
                    ioPair.PairTO.StartTime = startDate;
                    ioPair.PairTO.EndTime = endDate;
                    ioPair.PairTO.EmployeeID = emplID;
                    ioPair.PairTO.LocationID = (int)cbLocation.SelectedValue;
                    ioPair.PairTO.PassTypeID = (int)cbPassType.SelectedValue;
                    count = ioPair.SearchCount(dtFrom.Value.Date, dtTo.Value.Date, selectedWU, checkedWUID);

                    if (count > Constants.maxRecords)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("IOPairsGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            currentIOPairsList = ioPair.SearchWithType(dtFrom.Value.Date, dtTo.Value.AddDays(1).Date, selectedWU, checkedWUID);
                        }
                        else
                        {
                            currentIOPairsList.Clear();
                            clearListView();
                        }
                    }
                    else
                    {
                        if (count > 0)
                        {
                            currentIOPairsList = ioPair.SearchWithType(dtFrom.Value.Date, dtTo.Value.AddDays(1).Date, selectedWU, checkedWUID);
                        }
                        else
                        {
                            currentIOPairsList.Clear();
                            clearListView();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                    return;
                }
			
				//ioPairsList.ToString();
				if (currentIOPairsList.Count > 0)
				{
                    List<IOPairTO> tempList = new List<IOPairTO>();
                    List<IOPairTO> controlList = new List<IOPairTO>();
                    foreach (IOPairTO pair in currentIOPairsList)
                    {
                        if (pair.PassTypeID == Constants.regularWork && pair.EndTime.TimeOfDay.Hours == 23 && pair.EndTime.TimeOfDay.Minutes == 59)
                        {
                            foreach (IOPairTO pairNextDay in currentIOPairsList)
                            {
                                if (pairNextDay.PassTypeID == Constants.regularWork && pairNextDay.StartTime.Equals(new DateTime(pair.StartTime.Year,pair.StartTime.Month,pair.StartTime.AddDays(1).Day,0,0,0))
                                    && pair.EmployeeID == pairNextDay.EmployeeID)
                                {
                                    if (tempList.Contains(pairNextDay))
                                        tempList.Remove(pairNextDay);
                                    pair.EndTime = pairNextDay.EndTime;
                                    tempList.Add(pair);
                                    controlList.Add(pairNextDay);
                                    break;
                                }
                            }
                        }
                        else if (!tempList.Contains(pair)&&!controlList.Contains(pair))
                        {
                            if(!pair.StartTime.Equals(new DateTime(dtFrom.Value.Year,dtFrom.Value.Month,dtFrom.Value.Day,0,0,0))
                                && pair.StartTime.Date != dtTo.Value.AddDays(1).Date && pair.EndTime.Date!=dtTo.Value.AddDays(1).Date)
                            tempList.Add(pair);
                        }
                    }
                    currentIOPairsList = tempList;
					startIndex = 0;
					currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
					populateListView(currentIOPairsList, startIndex);
					this.lblTotal.Visible = true;
					this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentIOPairsList.Count.ToString().Trim();
				}
				else //else if (count == 0)
				{
					//MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                    clearListView();
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void populateListView(List<IOPairTO> ioPairList, int startIndex)
		{
			try
			{
				if (ioPairList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvResults.BeginUpdate();
				lvResults.Items.Clear();

				if (ioPairList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < ioPairList.Count))
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
						if (lastIndex >= ioPairList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = ioPairList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							IOPairTO ioPair = ioPairList[i];
							ListViewItem item = new ListViewItem();

							item.Text = ioPair.EmployeeName +" "+ioPair.EmployeeLastName;
							item.SubItems.Add(ioPair.PassType);
                            if (ioPair.StartTime != new DateTime())
                            {
                                item.SubItems.Add(ioPair.StartTime.ToString("dd.MM.yyyy"));
                                item.SubItems.Add(ioPair.StartTime.ToString("HH:mm"));
                                item.SubItems.Add(Constants.DirectionIn);
                            }
                            else
                            { 
                                item.SubItems.Add(noData);
                                item.SubItems.Add(noData);
                                item.SubItems.Add(noData);                            
                            }
                            if (ioPair.EndTime != new DateTime())
                            {
                                item.SubItems.Add(ioPair.EndTime.ToString("dd.MM.yyyy"));
                                item.SubItems.Add(ioPair.EndTime.ToString("HH:mm"));
                                item.SubItems.Add(Constants.DirectionOut);
                            }
                            else
                            {
                                item.SubItems.Add(noData);
                                item.SubItems.Add(noData);
                                item.SubItems.Add(noData);
                            }
                            
                            TimeSpan ts = new TimeSpan();
                            if (ioPair.StartTime != new DateTime() && ioPair.EndTime != new DateTime() && ioPair.StartTime < ioPair.EndTime)
                            {
                                ts = ioPair.EndTime - ioPair.StartTime;
                            }
                            string time = "";
                            if (ts.Hours < 10)
                                time += "0";
                            time += ts.Hours + "h ";
                            if (ts.Minutes < 10)
                                time += "0";
                            time += ts.Minutes + "min";
                            item.SubItems.Add(time);
							
							item.Tag = ioPair;
							lvResults.Items.Add(item);
						}
					}
				}

				lvResults.EndUpdate();
				lvResults.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvResults_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				/*if (lvResults.SelectedItems.Count > 0)
				{
					IOPair ioPair = (IOPair) lvResults.SelectedItems[0].Tag;

					this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(ioPair.WUName);
					this.cbEmplName.SelectedValue = ioPair.EmployeeID;
					this.cbLocation.SelectedValue = ioPair.LocationID;
					this.dtFrom.Value = ioPair.IOPairDate.Date;
					this.dtTo.Value = ioPair.IOPairDate.Date;
				}*/
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.lvResults_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvResults_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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
				currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateListView(currentIOPairsList, startIndex);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.lvResults_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
        		
		private void setVisibility()
		{
			try
			{
				int permission;

				foreach (ApplRoleTO role in currentRoles)
				{
					permission = (((int[]) menuItemsPermissions[menuItemID])[role.ApplRoleID]);
					readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
					addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
					updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
					deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
				}

				btnSearch.Enabled = readPermission;

                //btnMapMaintenance.Visible = false;
                //btnMapObjects.Visible = false;
                //btnMapView.Visible = false;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void clearForm()
		{
			this.cbEmplName.SelectedIndex = 0;
			this.cbLocation.SelectedIndex = 0;
			this.cbShowPairs.SelectedIndex = 0;
			this.cbWorkingUnit.SelectedIndex = 0;
			dtFrom.Value = DateTime.Now;
			dtTo.Value = DateTime.Now;
			this.lblTotal.Visible=false;
		}

		private void clearListView()
		{
			lvResults.BeginUpdate();
			lvResults.Items.Clear();
			lvResults.EndUpdate();

			lvResults.Invalidate();

			btnPrev.Visible = false;
			btnNext.Visible = false;
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
				populateListView(currentIOPairsList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnPrev_Click(): " + ex.Message + "\n");
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
				populateListView(currentIOPairsList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        
        private void IOPairs_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " IOPairsUNIPROM.IOPairs_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        //private void btnWUTree_Click(object sender, EventArgs e)
        //{
        //    this.Cursor = Cursors.WaitCursor;
        //    try
        //    {
        //        System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
        //        WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
        //        workingUnitsTreeView.ShowDialog();
        //        if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
        //        {
        //            cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnWUTreeView_Click(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Arrow;
        //    }
        //}

        //private void btnLocationTree_Click(object sender, EventArgs e)
        //{
        //    this.Cursor = Cursors.WaitCursor;
        //    try
        //    {
        //        LocationsTreeView locationsTreeView = new LocationsTreeView(locArray);
        //        locationsTreeView.ShowDialog();
        //        if (!locationsTreeView.selectedLocation.Equals(""))
        //        {
        //            this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnWUTreeView_Click(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Arrow;
        //    }
        //}

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       
        private void IOPairs_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateLocationCb();
                populatePairStatusCombo();
                populateWorkigUnitCombo();
                populateEmployeeCombo(-1);
                populatePassTypeCombo();

                dtFrom.Value = DateTime.Now;
                dtTo.Value = DateTime.Now;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                sortOrder = Constants.sortAsc;
                sortField = IOPairsUNIPROM.StartTime;
                startIndex = 0;

                clearListView();
                this.lblTotal.Visible = false;

                if (!wuString.Equals(""))
                {
                    /*currentIOPairsList = new IOPair().Search(new DateTime(0), new DateTime(0), -1, -1, -1, 
                        new DateTime(0), new DateTime(0), wuString);
                    currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentIOPairsList, startIndex);*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                }

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairsUNIPROM.Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairsUNIPROM.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairsUNIPROM.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }


        public class WUPresence
        {
            private string _workingUnit = "";

            public string WorkingUnit
            {
                get { return _workingUnit; }
                set { _workingUnit = value; }
            }
            private int _present = 0;

            public int Present
            {
                get { return _present; }
                set { _present = value; }
            }
            private int _sickLeave = 0;

            public int SickLeave
            {
                get { return _sickLeave; }
                set { _sickLeave = value; }
            }
            private int _vacation = 0;

            public int Vacation
            {
                get { return _vacation; }
                set { _vacation = value; }
            }
            private int _dayOff = 0;

            public int DayOff
            {
                get { return _dayOff; }
                set { _dayOff = value; }
            }
            private int _absent = 0;

            public int Absent
            {
                get { return _absent; }
                set { _absent = value; }
            }
            private int _totalEmpl = 0;

            public int TotalEmpl
            {
                get { return _totalEmpl; }
                set { _totalEmpl = value; }
            }
            private DateTime _date = new DateTime(0);

            public DateTime Date
            {
                get { return _date; }
                set { _date = value; }
            }

            public WUPresence()
            { }
        }      
	}
}
