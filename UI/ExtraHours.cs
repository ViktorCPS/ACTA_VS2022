using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Text;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for ExtraHours.
	/// </summary>
	public class ExtraHours : System.Windows.Forms.Form
	{																
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageEarnedHours;
		private System.Windows.Forms.TabPage tabPageUsedHours;
		private System.Windows.Forms.TabPage tabPageUsingExtraHours;	
		private System.Windows.Forms.GroupBox gbEarnedHours;
		private System.Windows.Forms.GroupBox gbUsedHours;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitU;
		private System.Windows.Forms.Label lblWorkingUnitE;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.ComboBox cbWorkingUnitU;
		private System.Windows.Forms.ComboBox cbWorkingUnitE;
		private System.Windows.Forms.Label lblEmployeeU;
		private System.Windows.Forms.Label lblEmployeeE;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ComboBox cbEmployee;		
		private System.Windows.Forms.ComboBox cbEmployeeU;
		private System.Windows.Forms.ComboBox cbEmployeeE;		
		private System.Windows.Forms.DateTimePicker dtpToE;
		private System.Windows.Forms.Label lblToE;
		private System.Windows.Forms.DateTimePicker dtpFromE;
		private System.Windows.Forms.Label lblFromE;		
		private System.Windows.Forms.Label lblFromU;
		private System.Windows.Forms.Label lblToU;
		private System.Windows.Forms.DateTimePicker dtpFromU;
		private System.Windows.Forms.DateTimePicker dtpToU;
		private System.Windows.Forms.Label lblEmplUsedHours;
		private System.Windows.Forms.Label lblEmplEarnedHours;
		private System.Windows.Forms.ListView lvExtraHours;
		private System.Windows.Forms.ListView lvUsedHours;
		private System.Windows.Forms.ListView lvEarnedHours;
		private System.Windows.Forms.Label lblTotalE;
		private System.Windows.Forms.Label lblTotalU;
		private System.Windows.Forms.Label lblLastReaderTimeE;
		private System.Windows.Forms.Label lblAvailableHours;
		private System.Windows.Forms.TextBox tbAvailableHours;
		private System.Windows.Forms.Panel panelTime;
		private System.Windows.Forms.Label lblUsingHours;
		private System.Windows.Forms.TextBox tbUsingHours;
		private System.Windows.Forms.Label lblHrs;
		private System.Windows.Forms.ComboBox cbUsingMinutes;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblUsingDate;
		private System.Windows.Forms.DateTimePicker dtpUsingDate;
		private System.Windows.Forms.Label lblLastReaderTime;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSearchE;
		private System.Windows.Forms.Button btnSearchU;	
		private System.Windows.Forms.Button btnCalculate;
		private System.Windows.Forms.Button btnCalculateE;
		private System.Windows.Forms.Button btnDelete;
        private GroupBox gbType;
        private RadioButton rbOvertime;
        private RadioButton rbExtraHours;
        private RadioButton rbDiscard;
        private Label lblType;
        private ComboBox cbType;
        private CheckBox cbTakeWholeInterval;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        		
		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;	

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;
		private ListViewItemComparer _comp3;

		// List View indexes
		const int DateIndex = 0;		
		const int TimeIndex = 1;
		const int StartTime = 2;
		const int EndTime = 3;
		const int DateEarnedIndex = 4;
        const int TypeIndex = 5;
        const int CreatedByIndex = 6;

		private List<string> statuses = new List<string>();
		private List<WorkingUnitTO> wUnits;
		private string wuString = "";
        private Color panelColor = new Color();
        private bool selectRegularWork = true;
        private Button btnWUTree;
        private Button btnWUTreeUsed;
        private Button btnWUTreeUsing;
        private CheckBox chbUsingAhead;
        private Label lblHrs1;
        private NumericUpDown numUsingAhead;
        private Label lblEarnedHours;
        private TextBox tbEarnedHours;
        private TextBox tbUsedAhead;
        private Label lblUsedAhead;
        private GroupBox gbLastMonthData;
        private GroupBox gbUsingAheadLimit;
        private CheckBox chbHierarhicly;
        private CheckBox chbHierarchyU;
        private CheckBox chbHierarchyUE;
        private bool visibleIntervalCheckBox = false;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        //MDI chiled properties 
        List<EmployeeTO> currnetEmplArray = new List<EmployeeTO>();
        private Button btnEarnedReport;
        private Button btnUsingReport;
        private Button btnReportUsed;

        private Filter filter;

		public ExtraHours()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);
                				
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(ExtraHours).Assembly);
				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.ExtraHours(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        #region MDI child method's
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWorkingUnit.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currnetEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmployee.SelectedValue = selectedEmployeeID;
                }
                if (selectedEmployeeID > 0)
                {
                    btnSearchE_Click(this, new EventArgs());
                    btnSearchU_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationEvidence.changeSelectedEmployee(): " + ex.Message + "\n");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtraHours));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEarnedHours = new System.Windows.Forms.TabPage();
            this.btnEarnedReport = new System.Windows.Forms.Button();
            this.btnCalculateE = new System.Windows.Forms.Button();
            this.lblLastReaderTimeE = new System.Windows.Forms.Label();
            this.gbEarnedHours = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWorkingUnitE = new System.Windows.Forms.ComboBox();
            this.cbEmployeeE = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitE = new System.Windows.Forms.Label();
            this.lblEmployeeE = new System.Windows.Forms.Label();
            this.dtpFromE = new System.Windows.Forms.DateTimePicker();
            this.lblFromE = new System.Windows.Forms.Label();
            this.btnSearchE = new System.Windows.Forms.Button();
            this.dtpToE = new System.Windows.Forms.DateTimePicker();
            this.lblToE = new System.Windows.Forms.Label();
            this.lblTotalE = new System.Windows.Forms.Label();
            this.lblEmplEarnedHours = new System.Windows.Forms.Label();
            this.lvEarnedHours = new System.Windows.Forms.ListView();
            this.tabPageUsedHours = new System.Windows.Forms.TabPage();
            this.btnReportUsed = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.gbUsedHours = new System.Windows.Forms.GroupBox();
            this.chbHierarchyU = new System.Windows.Forms.CheckBox();
            this.btnWUTreeUsed = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.btnSearchU = new System.Windows.Forms.Button();
            this.dtpFromU = new System.Windows.Forms.DateTimePicker();
            this.lblFromU = new System.Windows.Forms.Label();
            this.dtpToU = new System.Windows.Forms.DateTimePicker();
            this.lblToU = new System.Windows.Forms.Label();
            this.cbWorkingUnitU = new System.Windows.Forms.ComboBox();
            this.cbEmployeeU = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitU = new System.Windows.Forms.Label();
            this.lblEmployeeU = new System.Windows.Forms.Label();
            this.lblTotalU = new System.Windows.Forms.Label();
            this.lblEmplUsedHours = new System.Windows.Forms.Label();
            this.lvUsedHours = new System.Windows.Forms.ListView();
            this.tabPageUsingExtraHours = new System.Windows.Forms.TabPage();
            this.btnUsingReport = new System.Windows.Forms.Button();
            this.chbHierarchyUE = new System.Windows.Forms.CheckBox();
            this.gbUsingAheadLimit = new System.Windows.Forms.GroupBox();
            this.numUsingAhead = new System.Windows.Forms.NumericUpDown();
            this.lblHrs1 = new System.Windows.Forms.Label();
            this.gbLastMonthData = new System.Windows.Forms.GroupBox();
            this.lblEarnedHours = new System.Windows.Forms.Label();
            this.tbUsedAhead = new System.Windows.Forms.TextBox();
            this.tbEarnedHours = new System.Windows.Forms.TextBox();
            this.lblUsedAhead = new System.Windows.Forms.Label();
            this.chbUsingAhead = new System.Windows.Forms.CheckBox();
            this.btnWUTreeUsing = new System.Windows.Forms.Button();
            this.cbTakeWholeInterval = new System.Windows.Forms.CheckBox();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbDiscard = new System.Windows.Forms.RadioButton();
            this.rbOvertime = new System.Windows.Forms.RadioButton();
            this.rbExtraHours = new System.Windows.Forms.RadioButton();
            this.lblLastReaderTime = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.panelTime = new System.Windows.Forms.Panel();
            this.lblMin = new System.Windows.Forms.Label();
            this.cbUsingMinutes = new System.Windows.Forms.ComboBox();
            this.lblHrs = new System.Windows.Forms.Label();
            this.tbUsingHours = new System.Windows.Forms.TextBox();
            this.dtpUsingDate = new System.Windows.Forms.DateTimePicker();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblUsingDate = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.tbAvailableHours = new System.Windows.Forms.TextBox();
            this.lblUsingHours = new System.Windows.Forms.Label();
            this.lblAvailableHours = new System.Windows.Forms.Label();
            this.lvExtraHours = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPageEarnedHours.SuspendLayout();
            this.gbEarnedHours.SuspendLayout();
            this.tabPageUsedHours.SuspendLayout();
            this.gbUsedHours.SuspendLayout();
            this.tabPageUsingExtraHours.SuspendLayout();
            this.gbUsingAheadLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUsingAhead)).BeginInit();
            this.gbLastMonthData.SuspendLayout();
            this.gbType.SuspendLayout();
            this.panelTime.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEarnedHours);
            this.tabControl1.Controls.Add(this.tabPageUsedHours);
            this.tabControl1.Controls.Add(this.tabPageUsingExtraHours);
            this.tabControl1.Location = new System.Drawing.Point(10, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(805, 619);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageEarnedHours
            // 
            this.tabPageEarnedHours.Controls.Add(this.btnEarnedReport);
            this.tabPageEarnedHours.Controls.Add(this.btnCalculateE);
            this.tabPageEarnedHours.Controls.Add(this.lblLastReaderTimeE);
            this.tabPageEarnedHours.Controls.Add(this.gbEarnedHours);
            this.tabPageEarnedHours.Controls.Add(this.lblTotalE);
            this.tabPageEarnedHours.Controls.Add(this.lblEmplEarnedHours);
            this.tabPageEarnedHours.Controls.Add(this.lvEarnedHours);
            this.tabPageEarnedHours.Location = new System.Drawing.Point(4, 22);
            this.tabPageEarnedHours.Name = "tabPageEarnedHours";
            this.tabPageEarnedHours.Size = new System.Drawing.Size(797, 593);
            this.tabPageEarnedHours.TabIndex = 0;
            this.tabPageEarnedHours.Text = "Earned hours";
            // 
            // btnEarnedReport
            // 
            this.btnEarnedReport.Location = new System.Drawing.Point(361, 530);
            this.btnEarnedReport.Name = "btnEarnedReport";
            this.btnEarnedReport.Size = new System.Drawing.Size(75, 23);
            this.btnEarnedReport.TabIndex = 15;
            this.btnEarnedReport.Text = "Report";
            this.btnEarnedReport.Click += new System.EventHandler(this.btnEarnedReport_Click);
            // 
            // btnCalculateE
            // 
            this.btnCalculateE.Location = new System.Drawing.Point(16, 530);
            this.btnCalculateE.Name = "btnCalculateE";
            this.btnCalculateE.Size = new System.Drawing.Size(175, 23);
            this.btnCalculateE.TabIndex = 14;
            this.btnCalculateE.Text = "Calculate extra hours";
            this.btnCalculateE.Click += new System.EventHandler(this.btnCalculateE_Click);
            // 
            // lblLastReaderTimeE
            // 
            this.lblLastReaderTimeE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReaderTimeE.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLastReaderTimeE.Location = new System.Drawing.Point(16, 498);
            this.lblLastReaderTimeE.Name = "lblLastReaderTimeE";
            this.lblLastReaderTimeE.Size = new System.Drawing.Size(457, 24);
            this.lblLastReaderTimeE.TabIndex = 12;
            this.lblLastReaderTimeE.Text = "Last reading time for all readers:";
            this.lblLastReaderTimeE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbEarnedHours
            // 
            this.gbEarnedHours.Controls.Add(this.chbHierarhicly);
            this.gbEarnedHours.Controls.Add(this.btnWUTree);
            this.gbEarnedHours.Controls.Add(this.cbWorkingUnitE);
            this.gbEarnedHours.Controls.Add(this.cbEmployeeE);
            this.gbEarnedHours.Controls.Add(this.lblWorkingUnitE);
            this.gbEarnedHours.Controls.Add(this.lblEmployeeE);
            this.gbEarnedHours.Controls.Add(this.dtpFromE);
            this.gbEarnedHours.Controls.Add(this.lblFromE);
            this.gbEarnedHours.Controls.Add(this.btnSearchE);
            this.gbEarnedHours.Controls.Add(this.dtpToE);
            this.gbEarnedHours.Controls.Add(this.lblToE);
            this.gbEarnedHours.Location = new System.Drawing.Point(16, 8);
            this.gbEarnedHours.Name = "gbEarnedHours";
            this.gbEarnedHours.Size = new System.Drawing.Size(610, 144);
            this.gbEarnedHours.TabIndex = 0;
            this.gbEarnedHours.TabStop = false;
            this.gbEarnedHours.Tag = "FILTERABLE";
            this.gbEarnedHours.Text = "Earned hours";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(389, 16);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 46;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(358, 14);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 40;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWorkingUnitE
            // 
            this.cbWorkingUnitE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitE.Location = new System.Drawing.Point(128, 16);
            this.cbWorkingUnitE.Name = "cbWorkingUnitE";
            this.cbWorkingUnitE.Size = new System.Drawing.Size(224, 21);
            this.cbWorkingUnitE.TabIndex = 2;
            this.cbWorkingUnitE.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitE_SelectedIndexChanged);
            // 
            // cbEmployeeE
            // 
            this.cbEmployeeE.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployeeE.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployeeE.Location = new System.Drawing.Point(128, 48);
            this.cbEmployeeE.Name = "cbEmployeeE";
            this.cbEmployeeE.Size = new System.Drawing.Size(224, 21);
            this.cbEmployeeE.TabIndex = 4;
            this.cbEmployeeE.SelectedIndexChanged += new System.EventHandler(this.cbEmployeeE_SelectedIndexChanged);
            this.cbEmployeeE.Leave += new System.EventHandler(this.cbEmployeeE_Leave);
            // 
            // lblWorkingUnitE
            // 
            this.lblWorkingUnitE.Location = new System.Drawing.Point(8, 16);
            this.lblWorkingUnitE.Name = "lblWorkingUnitE";
            this.lblWorkingUnitE.Size = new System.Drawing.Size(112, 23);
            this.lblWorkingUnitE.TabIndex = 1;
            this.lblWorkingUnitE.Text = "Working unit:";
            this.lblWorkingUnitE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployeeE
            // 
            this.lblEmployeeE.Location = new System.Drawing.Point(8, 48);
            this.lblEmployeeE.Name = "lblEmployeeE";
            this.lblEmployeeE.Size = new System.Drawing.Size(112, 23);
            this.lblEmployeeE.TabIndex = 3;
            this.lblEmployeeE.Text = "Employee:";
            this.lblEmployeeE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromE
            // 
            this.dtpFromE.CustomFormat = "dd.MM.yyy";
            this.dtpFromE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromE.Location = new System.Drawing.Point(128, 80);
            this.dtpFromE.Name = "dtpFromE";
            this.dtpFromE.Size = new System.Drawing.Size(100, 20);
            this.dtpFromE.TabIndex = 6;
            this.dtpFromE.ValueChanged += new System.EventHandler(this.dtpFromE_ValueChanged);
            // 
            // lblFromE
            // 
            this.lblFromE.Location = new System.Drawing.Point(8, 80);
            this.lblFromE.Name = "lblFromE";
            this.lblFromE.Size = new System.Drawing.Size(112, 23);
            this.lblFromE.TabIndex = 5;
            this.lblFromE.Text = "From:";
            this.lblFromE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearchE
            // 
            this.btnSearchE.Location = new System.Drawing.Point(522, 112);
            this.btnSearchE.Name = "btnSearchE";
            this.btnSearchE.Size = new System.Drawing.Size(75, 23);
            this.btnSearchE.TabIndex = 9;
            this.btnSearchE.Text = "Search";
            this.btnSearchE.Click += new System.EventHandler(this.btnSearchE_Click);
            // 
            // dtpToE
            // 
            this.dtpToE.CustomFormat = "dd.MM.yyy";
            this.dtpToE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToE.Location = new System.Drawing.Point(128, 112);
            this.dtpToE.Name = "dtpToE";
            this.dtpToE.Size = new System.Drawing.Size(100, 20);
            this.dtpToE.TabIndex = 8;
            this.dtpToE.ValueChanged += new System.EventHandler(this.dtpToE_ValueChanged);
            // 
            // lblToE
            // 
            this.lblToE.Location = new System.Drawing.Point(8, 112);
            this.lblToE.Name = "lblToE";
            this.lblToE.Size = new System.Drawing.Size(112, 23);
            this.lblToE.TabIndex = 7;
            this.lblToE.Text = "To:";
            this.lblToE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalE
            // 
            this.lblTotalE.Location = new System.Drawing.Point(651, 502);
            this.lblTotalE.Name = "lblTotalE";
            this.lblTotalE.Size = new System.Drawing.Size(130, 16);
            this.lblTotalE.TabIndex = 13;
            this.lblTotalE.Text = "Total:";
            this.lblTotalE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmplEarnedHours
            // 
            this.lblEmplEarnedHours.Location = new System.Drawing.Point(16, 168);
            this.lblEmplEarnedHours.Name = "lblEmplEarnedHours";
            this.lblEmplEarnedHours.Size = new System.Drawing.Size(344, 23);
            this.lblEmplEarnedHours.TabIndex = 10;
            this.lblEmplEarnedHours.Text = "List of all earned hours for employee:";
            this.lblEmplEarnedHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvEarnedHours
            // 
            this.lvEarnedHours.FullRowSelect = true;
            this.lvEarnedHours.GridLines = true;
            this.lvEarnedHours.HideSelection = false;
            this.lvEarnedHours.Location = new System.Drawing.Point(16, 200);
            this.lvEarnedHours.MultiSelect = false;
            this.lvEarnedHours.Name = "lvEarnedHours";
            this.lvEarnedHours.Size = new System.Drawing.Size(765, 285);
            this.lvEarnedHours.TabIndex = 11;
            this.lvEarnedHours.UseCompatibleStateImageBehavior = false;
            this.lvEarnedHours.View = System.Windows.Forms.View.Details;
            this.lvEarnedHours.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEarnedHours_ColumnClick);
            // 
            // tabPageUsedHours
            // 
            this.tabPageUsedHours.Controls.Add(this.btnReportUsed);
            this.tabPageUsedHours.Controls.Add(this.btnDelete);
            this.tabPageUsedHours.Controls.Add(this.gbUsedHours);
            this.tabPageUsedHours.Controls.Add(this.lblTotalU);
            this.tabPageUsedHours.Controls.Add(this.lblEmplUsedHours);
            this.tabPageUsedHours.Controls.Add(this.lvUsedHours);
            this.tabPageUsedHours.Location = new System.Drawing.Point(4, 22);
            this.tabPageUsedHours.Name = "tabPageUsedHours";
            this.tabPageUsedHours.Size = new System.Drawing.Size(797, 593);
            this.tabPageUsedHours.TabIndex = 1;
            this.tabPageUsedHours.Text = "Used hours";
            // 
            // btnReportUsed
            // 
            this.btnReportUsed.Location = new System.Drawing.Point(361, 551);
            this.btnReportUsed.Name = "btnReportUsed";
            this.btnReportUsed.Size = new System.Drawing.Size(75, 23);
            this.btnReportUsed.TabIndex = 22;
            this.btnReportUsed.Text = "Report";
            this.btnReportUsed.Click += new System.EventHandler(this.btnReportUsed_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(16, 551);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 20;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbUsedHours
            // 
            this.gbUsedHours.Controls.Add(this.chbHierarchyU);
            this.gbUsedHours.Controls.Add(this.btnWUTreeUsed);
            this.gbUsedHours.Controls.Add(this.cbType);
            this.gbUsedHours.Controls.Add(this.lblType);
            this.gbUsedHours.Controls.Add(this.btnSearchU);
            this.gbUsedHours.Controls.Add(this.dtpFromU);
            this.gbUsedHours.Controls.Add(this.lblFromU);
            this.gbUsedHours.Controls.Add(this.dtpToU);
            this.gbUsedHours.Controls.Add(this.lblToU);
            this.gbUsedHours.Controls.Add(this.cbWorkingUnitU);
            this.gbUsedHours.Controls.Add(this.cbEmployeeU);
            this.gbUsedHours.Controls.Add(this.lblWorkingUnitU);
            this.gbUsedHours.Controls.Add(this.lblEmployeeU);
            this.gbUsedHours.Location = new System.Drawing.Point(16, 8);
            this.gbUsedHours.Name = "gbUsedHours";
            this.gbUsedHours.Size = new System.Drawing.Size(610, 194);
            this.gbUsedHours.TabIndex = 0;
            this.gbUsedHours.TabStop = false;
            this.gbUsedHours.Tag = "FILTERABLE";
            this.gbUsedHours.Text = "Used hours";
            // 
            // chbHierarchyU
            // 
            this.chbHierarchyU.Location = new System.Drawing.Point(389, 16);
            this.chbHierarchyU.Name = "chbHierarchyU";
            this.chbHierarchyU.Size = new System.Drawing.Size(104, 24);
            this.chbHierarchyU.TabIndex = 47;
            this.chbHierarchyU.Text = "Hierarchy ";
            this.chbHierarchyU.CheckedChanged += new System.EventHandler(this.chbHierarchyU_CheckedChanged);
            // 
            // btnWUTreeUsed
            // 
            this.btnWUTreeUsed.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTreeUsed.Image")));
            this.btnWUTreeUsed.Location = new System.Drawing.Point(358, 16);
            this.btnWUTreeUsed.Name = "btnWUTreeUsed";
            this.btnWUTreeUsed.Size = new System.Drawing.Size(25, 23);
            this.btnWUTreeUsed.TabIndex = 41;
            this.btnWUTreeUsed.Click += new System.EventHandler(this.btnWUTreeUsed_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Location = new System.Drawing.Point(128, 144);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(224, 21);
            this.cbType.TabIndex = 16;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(8, 144);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(112, 23);
            this.lblType.TabIndex = 15;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearchU
            // 
            this.btnSearchU.Location = new System.Drawing.Point(522, 144);
            this.btnSearchU.Name = "btnSearchU";
            this.btnSearchU.Size = new System.Drawing.Size(75, 23);
            this.btnSearchU.TabIndex = 17;
            this.btnSearchU.Text = "Search";
            this.btnSearchU.Click += new System.EventHandler(this.btnSearchU_Click);
            // 
            // dtpFromU
            // 
            this.dtpFromU.CustomFormat = "dd.MM.yyy";
            this.dtpFromU.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromU.Location = new System.Drawing.Point(128, 80);
            this.dtpFromU.Name = "dtpFromU";
            this.dtpFromU.Size = new System.Drawing.Size(100, 20);
            this.dtpFromU.TabIndex = 12;
            this.dtpFromU.ValueChanged += new System.EventHandler(this.dtpFromU_ValueChanged);
            // 
            // lblFromU
            // 
            this.lblFromU.Location = new System.Drawing.Point(8, 80);
            this.lblFromU.Name = "lblFromU";
            this.lblFromU.Size = new System.Drawing.Size(112, 23);
            this.lblFromU.TabIndex = 11;
            this.lblFromU.Text = "From:";
            this.lblFromU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToU
            // 
            this.dtpToU.CustomFormat = "dd.MM.yyy";
            this.dtpToU.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToU.Location = new System.Drawing.Point(128, 112);
            this.dtpToU.Name = "dtpToU";
            this.dtpToU.Size = new System.Drawing.Size(100, 20);
            this.dtpToU.TabIndex = 14;
            this.dtpToU.ValueChanged += new System.EventHandler(this.dtpToU_ValueChanged);
            // 
            // lblToU
            // 
            this.lblToU.Location = new System.Drawing.Point(8, 112);
            this.lblToU.Name = "lblToU";
            this.lblToU.Size = new System.Drawing.Size(112, 23);
            this.lblToU.TabIndex = 13;
            this.lblToU.Text = "To:";
            this.lblToU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnitU
            // 
            this.cbWorkingUnitU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitU.Location = new System.Drawing.Point(128, 16);
            this.cbWorkingUnitU.Name = "cbWorkingUnitU";
            this.cbWorkingUnitU.Size = new System.Drawing.Size(224, 21);
            this.cbWorkingUnitU.TabIndex = 8;
            this.cbWorkingUnitU.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitU_SelectedIndexChanged);
            // 
            // cbEmployeeU
            // 
            this.cbEmployeeU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployeeU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployeeU.Location = new System.Drawing.Point(128, 48);
            this.cbEmployeeU.Name = "cbEmployeeU";
            this.cbEmployeeU.Size = new System.Drawing.Size(224, 21);
            this.cbEmployeeU.TabIndex = 10;
            this.cbEmployeeU.SelectedIndexChanged += new System.EventHandler(this.cbEmployeeU_SelectedIndexChanged);
            this.cbEmployeeU.Leave += new System.EventHandler(this.cbEmployeeU_Leave);
            // 
            // lblWorkingUnitU
            // 
            this.lblWorkingUnitU.Location = new System.Drawing.Point(8, 16);
            this.lblWorkingUnitU.Name = "lblWorkingUnitU";
            this.lblWorkingUnitU.Size = new System.Drawing.Size(112, 23);
            this.lblWorkingUnitU.TabIndex = 7;
            this.lblWorkingUnitU.Text = "Working unit:";
            this.lblWorkingUnitU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployeeU
            // 
            this.lblEmployeeU.Location = new System.Drawing.Point(8, 48);
            this.lblEmployeeU.Name = "lblEmployeeU";
            this.lblEmployeeU.Size = new System.Drawing.Size(112, 23);
            this.lblEmployeeU.TabIndex = 9;
            this.lblEmployeeU.Text = "Employee:";
            this.lblEmployeeU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalU
            // 
            this.lblTotalU.Location = new System.Drawing.Point(651, 506);
            this.lblTotalU.Name = "lblTotalU";
            this.lblTotalU.Size = new System.Drawing.Size(130, 16);
            this.lblTotalU.TabIndex = 21;
            this.lblTotalU.Text = "Total:";
            this.lblTotalU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmplUsedHours
            // 
            this.lblEmplUsedHours.Location = new System.Drawing.Point(16, 218);
            this.lblEmplUsedHours.Name = "lblEmplUsedHours";
            this.lblEmplUsedHours.Size = new System.Drawing.Size(344, 23);
            this.lblEmplUsedHours.TabIndex = 18;
            this.lblEmplUsedHours.Text = "List of all used hours for employee:";
            this.lblEmplUsedHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvUsedHours
            // 
            this.lvUsedHours.FullRowSelect = true;
            this.lvUsedHours.GridLines = true;
            this.lvUsedHours.HideSelection = false;
            this.lvUsedHours.Location = new System.Drawing.Point(16, 250);
            this.lvUsedHours.Name = "lvUsedHours";
            this.lvUsedHours.Size = new System.Drawing.Size(765, 235);
            this.lvUsedHours.TabIndex = 19;
            this.lvUsedHours.UseCompatibleStateImageBehavior = false;
            this.lvUsedHours.View = System.Windows.Forms.View.Details;
            this.lvUsedHours.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsedHours_ColumnClick);
            // 
            // tabPageUsingExtraHours
            // 
            this.tabPageUsingExtraHours.Controls.Add(this.btnUsingReport);
            this.tabPageUsingExtraHours.Controls.Add(this.chbHierarchyUE);
            this.tabPageUsingExtraHours.Controls.Add(this.gbUsingAheadLimit);
            this.tabPageUsingExtraHours.Controls.Add(this.gbLastMonthData);
            this.tabPageUsingExtraHours.Controls.Add(this.chbUsingAhead);
            this.tabPageUsingExtraHours.Controls.Add(this.btnWUTreeUsing);
            this.tabPageUsingExtraHours.Controls.Add(this.cbTakeWholeInterval);
            this.tabPageUsingExtraHours.Controls.Add(this.gbType);
            this.tabPageUsingExtraHours.Controls.Add(this.lblLastReaderTime);
            this.tabPageUsingExtraHours.Controls.Add(this.btnCalculate);
            this.tabPageUsingExtraHours.Controls.Add(this.panelTime);
            this.tabPageUsingExtraHours.Controls.Add(this.dtpUsingDate);
            this.tabPageUsingExtraHours.Controls.Add(this.cbWorkingUnit);
            this.tabPageUsingExtraHours.Controls.Add(this.lblWorkingUnit);
            this.tabPageUsingExtraHours.Controls.Add(this.lblUsingDate);
            this.tabPageUsingExtraHours.Controls.Add(this.btnCancel);
            this.tabPageUsingExtraHours.Controls.Add(this.btnSave);
            this.tabPageUsingExtraHours.Controls.Add(this.cbEmployee);
            this.tabPageUsingExtraHours.Controls.Add(this.lblEmployee);
            this.tabPageUsingExtraHours.Controls.Add(this.tbAvailableHours);
            this.tabPageUsingExtraHours.Controls.Add(this.lblUsingHours);
            this.tabPageUsingExtraHours.Controls.Add(this.lblAvailableHours);
            this.tabPageUsingExtraHours.Controls.Add(this.lvExtraHours);
            this.tabPageUsingExtraHours.Location = new System.Drawing.Point(4, 22);
            this.tabPageUsingExtraHours.Name = "tabPageUsingExtraHours";
            this.tabPageUsingExtraHours.Size = new System.Drawing.Size(797, 593);
            this.tabPageUsingExtraHours.TabIndex = 2;
            this.tabPageUsingExtraHours.Text = "Using extra hours";
            // 
            // btnUsingReport
            // 
            this.btnUsingReport.Location = new System.Drawing.Point(361, 557);
            this.btnUsingReport.Name = "btnUsingReport";
            this.btnUsingReport.Size = new System.Drawing.Size(75, 23);
            this.btnUsingReport.TabIndex = 53;
            this.btnUsingReport.Text = "Report";
            this.btnUsingReport.Click += new System.EventHandler(this.btnUsingReport_Click);
            // 
            // chbHierarchyUE
            // 
            this.chbHierarchyUE.Location = new System.Drawing.Point(485, 14);
            this.chbHierarchyUE.Name = "chbHierarchyUE";
            this.chbHierarchyUE.Size = new System.Drawing.Size(104, 24);
            this.chbHierarchyUE.TabIndex = 52;
            this.chbHierarchyUE.Tag = "FILTERABLE";
            this.chbHierarchyUE.Text = "Hierarchy ";
            this.chbHierarchyUE.CheckedChanged += new System.EventHandler(this.chbHierarchyUE_CheckedChanged);
            // 
            // gbUsingAheadLimit
            // 
            this.gbUsingAheadLimit.Controls.Add(this.numUsingAhead);
            this.gbUsingAheadLimit.Controls.Add(this.lblHrs1);
            this.gbUsingAheadLimit.Location = new System.Drawing.Point(610, 127);
            this.gbUsingAheadLimit.Name = "gbUsingAheadLimit";
            this.gbUsingAheadLimit.Size = new System.Drawing.Size(171, 50);
            this.gbUsingAheadLimit.TabIndex = 51;
            this.gbUsingAheadLimit.TabStop = false;
            this.gbUsingAheadLimit.Text = "Using ahead limit";
            // 
            // numUsingAhead
            // 
            this.numUsingAhead.Location = new System.Drawing.Point(33, 20);
            this.numUsingAhead.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numUsingAhead.Name = "numUsingAhead";
            this.numUsingAhead.Size = new System.Drawing.Size(65, 20);
            this.numUsingAhead.TabIndex = 44;
            this.numUsingAhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUsingAhead.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // lblHrs1
            // 
            this.lblHrs1.Location = new System.Drawing.Point(104, 15);
            this.lblHrs1.Name = "lblHrs1";
            this.lblHrs1.Size = new System.Drawing.Size(30, 23);
            this.lblHrs1.TabIndex = 45;
            this.lblHrs1.Text = "hrs";
            this.lblHrs1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbLastMonthData
            // 
            this.gbLastMonthData.Controls.Add(this.lblEarnedHours);
            this.gbLastMonthData.Controls.Add(this.tbUsedAhead);
            this.gbLastMonthData.Controls.Add(this.tbEarnedHours);
            this.gbLastMonthData.Controls.Add(this.lblUsedAhead);
            this.gbLastMonthData.Location = new System.Drawing.Point(376, 73);
            this.gbLastMonthData.Name = "gbLastMonthData";
            this.gbLastMonthData.Size = new System.Drawing.Size(405, 48);
            this.gbLastMonthData.TabIndex = 50;
            this.gbLastMonthData.TabStop = false;
            this.gbLastMonthData.Text = "Earned and used time for current  month";
            // 
            // lblEarnedHours
            // 
            this.lblEarnedHours.Location = new System.Drawing.Point(6, 16);
            this.lblEarnedHours.Name = "lblEarnedHours";
            this.lblEarnedHours.Size = new System.Drawing.Size(88, 23);
            this.lblEarnedHours.TabIndex = 46;
            this.lblEarnedHours.Text = "Earned hours:";
            this.lblEarnedHours.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUsedAhead
            // 
            this.tbUsedAhead.Enabled = false;
            this.tbUsedAhead.Location = new System.Drawing.Point(314, 18);
            this.tbUsedAhead.MaxLength = 50;
            this.tbUsedAhead.Name = "tbUsedAhead";
            this.tbUsedAhead.Size = new System.Drawing.Size(85, 20);
            this.tbUsedAhead.TabIndex = 49;
            this.tbUsedAhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbEarnedHours
            // 
            this.tbEarnedHours.Enabled = false;
            this.tbEarnedHours.Location = new System.Drawing.Point(100, 18);
            this.tbEarnedHours.MaxLength = 50;
            this.tbEarnedHours.Name = "tbEarnedHours";
            this.tbEarnedHours.Size = new System.Drawing.Size(82, 20);
            this.tbEarnedHours.TabIndex = 47;
            this.tbEarnedHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblUsedAhead
            // 
            this.lblUsedAhead.Location = new System.Drawing.Point(188, 16);
            this.lblUsedAhead.Name = "lblUsedAhead";
            this.lblUsedAhead.Size = new System.Drawing.Size(120, 23);
            this.lblUsedAhead.TabIndex = 48;
            this.lblUsedAhead.Text = "Used ahead:";
            this.lblUsedAhead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbUsingAhead
            // 
            this.chbUsingAhead.Location = new System.Drawing.Point(415, 140);
            this.chbUsingAhead.Name = "chbUsingAhead";
            this.chbUsingAhead.Size = new System.Drawing.Size(116, 24);
            this.chbUsingAhead.TabIndex = 43;
            this.chbUsingAhead.Tag = "FILTERABLE";
            this.chbUsingAhead.Text = "Using ahead";
            this.chbUsingAhead.CheckedChanged += new System.EventHandler(this.chbUsingAhead_CheckedChanged);
            // 
            // btnWUTreeUsing
            // 
            this.btnWUTreeUsing.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTreeUsing.Image")));
            this.btnWUTreeUsing.Location = new System.Drawing.Point(454, 14);
            this.btnWUTreeUsing.Name = "btnWUTreeUsing";
            this.btnWUTreeUsing.Size = new System.Drawing.Size(25, 23);
            this.btnWUTreeUsing.TabIndex = 42;
            this.btnWUTreeUsing.Click += new System.EventHandler(this.btnWUTreeUsing_Click);
            // 
            // cbTakeWholeInterval
            // 
            this.cbTakeWholeInterval.Location = new System.Drawing.Point(219, 174);
            this.cbTakeWholeInterval.Name = "cbTakeWholeInterval";
            this.cbTakeWholeInterval.Size = new System.Drawing.Size(194, 24);
            this.cbTakeWholeInterval.TabIndex = 25;
            this.cbTakeWholeInterval.Tag = "FILTERABLE";
            this.cbTakeWholeInterval.Text = "Take Whole Interval";
            this.cbTakeWholeInterval.CheckedChanged += new System.EventHandler(this.cbTakeWholeInterval_CheckedChanged);
            // 
            // gbType
            // 
            this.gbType.Controls.Add(this.rbDiscard);
            this.gbType.Controls.Add(this.rbOvertime);
            this.gbType.Controls.Add(this.rbExtraHours);
            this.gbType.Location = new System.Drawing.Point(125, 198);
            this.gbType.Name = "gbType";
            this.gbType.Size = new System.Drawing.Size(402, 45);
            this.gbType.TabIndex = 26;
            this.gbType.TabStop = false;
            this.gbType.Tag = "FILTERABLE";
            this.gbType.Text = "Type";
            // 
            // rbDiscard
            // 
            this.rbDiscard.Location = new System.Drawing.Point(283, 15);
            this.rbDiscard.Name = "rbDiscard";
            this.rbDiscard.Size = new System.Drawing.Size(110, 25);
            this.rbDiscard.TabIndex = 29;
            this.rbDiscard.Text = "Rejected hours";
            this.rbDiscard.CheckedChanged += new System.EventHandler(this.rbDiscard_CheckedChanged);
            // 
            // rbOvertime
            // 
            this.rbOvertime.Location = new System.Drawing.Point(138, 15);
            this.rbOvertime.Name = "rbOvertime";
            this.rbOvertime.Size = new System.Drawing.Size(120, 25);
            this.rbOvertime.TabIndex = 28;
            this.rbOvertime.Text = "Overtime_work";
            this.rbOvertime.CheckedChanged += new System.EventHandler(this.rbOvertime_CheckedChanged);
            // 
            // rbExtraHours
            // 
            this.rbExtraHours.Checked = true;
            this.rbExtraHours.Location = new System.Drawing.Point(16, 15);
            this.rbExtraHours.Name = "rbExtraHours";
            this.rbExtraHours.Size = new System.Drawing.Size(110, 25);
            this.rbExtraHours.TabIndex = 27;
            this.rbExtraHours.TabStop = true;
            this.rbExtraHours.Text = "Regular_work";
            this.rbExtraHours.CheckedChanged += new System.EventHandler(this.rbExtraHours_CheckedChanged);
            // 
            // lblLastReaderTime
            // 
            this.lblLastReaderTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReaderTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLastReaderTime.Location = new System.Drawing.Point(13, 528);
            this.lblLastReaderTime.Name = "lblLastReaderTime";
            this.lblLastReaderTime.Size = new System.Drawing.Size(320, 23);
            this.lblLastReaderTime.TabIndex = 33;
            this.lblLastReaderTime.Text = "Last reading time for all readers:";
            this.lblLastReaderTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(606, 528);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(175, 23);
            this.btnCalculate.TabIndex = 34;
            this.btnCalculate.Text = "Calculate extra hours";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // panelTime
            // 
            this.panelTime.BackColor = System.Drawing.Color.Red;
            this.panelTime.Controls.Add(this.lblMin);
            this.panelTime.Controls.Add(this.cbUsingMinutes);
            this.panelTime.Controls.Add(this.lblHrs);
            this.panelTime.Controls.Add(this.tbUsingHours);
            this.panelTime.Location = new System.Drawing.Point(219, 133);
            this.panelTime.Name = "panelTime";
            this.panelTime.Size = new System.Drawing.Size(192, 35);
            this.panelTime.TabIndex = 24;
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(166, 8);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(30, 23);
            this.lblMin.TabIndex = 23;
            this.lblMin.Text = "min";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbUsingMinutes
            // 
            this.cbUsingMinutes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbUsingMinutes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbUsingMinutes.BackColor = System.Drawing.SystemColors.Window;
            this.cbUsingMinutes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbUsingMinutes.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.cbUsingMinutes.Location = new System.Drawing.Point(96, 8);
            this.cbUsingMinutes.Name = "cbUsingMinutes";
            this.cbUsingMinutes.Size = new System.Drawing.Size(68, 21);
            this.cbUsingMinutes.TabIndex = 22;
            this.cbUsingMinutes.Tag = "FILTERABLE";
            this.cbUsingMinutes.SelectedIndexChanged += new System.EventHandler(this.cbUsingMinutes_SelectedIndexChanged);
            this.cbUsingMinutes.Leave += new System.EventHandler(this.cbUsingMinutes_Leave);
            this.cbUsingMinutes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbUsingMinutes_KeyPress);
            // 
            // lblHrs
            // 
            this.lblHrs.Location = new System.Drawing.Point(56, 8);
            this.lblHrs.Name = "lblHrs";
            this.lblHrs.Size = new System.Drawing.Size(30, 23);
            this.lblHrs.TabIndex = 21;
            this.lblHrs.Text = "hrs";
            this.lblHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbUsingHours
            // 
            this.tbUsingHours.BackColor = System.Drawing.SystemColors.Window;
            this.tbUsingHours.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbUsingHours.Location = new System.Drawing.Point(8, 8);
            this.tbUsingHours.MaxLength = 3;
            this.tbUsingHours.Name = "tbUsingHours";
            this.tbUsingHours.Size = new System.Drawing.Size(40, 20);
            this.tbUsingHours.TabIndex = 20;
            this.tbUsingHours.Tag = "FILTERABLE";
            this.tbUsingHours.Text = "0";
            this.tbUsingHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbUsingHours.TextChanged += new System.EventHandler(this.tbUsingHours_TextChanged);
            this.tbUsingHours.Leave += new System.EventHandler(this.tbUsingHours_Leave);
            // 
            // dtpUsingDate
            // 
            this.dtpUsingDate.CustomFormat = "dd.MM.yyy HH:mm";
            this.dtpUsingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUsingDate.Location = new System.Drawing.Point(219, 490);
            this.dtpUsingDate.Name = "dtpUsingDate";
            this.dtpUsingDate.Size = new System.Drawing.Size(200, 20);
            this.dtpUsingDate.TabIndex = 32;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(224, 16);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(224, 21);
            this.cbWorkingUnit.TabIndex = 14;
            this.cbWorkingUnit.Tag = "FILTERABLE";
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 16);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(200, 23);
            this.lblWorkingUnit.TabIndex = 13;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsingDate
            // 
            this.lblUsingDate.Location = new System.Drawing.Point(13, 490);
            this.lblUsingDate.Name = "lblUsingDate";
            this.lblUsingDate.Size = new System.Drawing.Size(200, 23);
            this.lblUsingDate.TabIndex = 31;
            this.lblUsingDate.Text = "Date of use:";
            this.lblUsingDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(706, 557);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 557);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(224, 46);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(224, 21);
            this.cbEmployee.TabIndex = 16;
            this.cbEmployee.Tag = "FILTERABLE";
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            this.cbEmployee.Leave += new System.EventHandler(this.cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(16, 46);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(200, 23);
            this.lblEmployee.TabIndex = 15;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAvailableHours
            // 
            this.tbAvailableHours.Enabled = false;
            this.tbAvailableHours.Location = new System.Drawing.Point(224, 83);
            this.tbAvailableHours.MaxLength = 50;
            this.tbAvailableHours.Name = "tbAvailableHours";
            this.tbAvailableHours.Size = new System.Drawing.Size(130, 20);
            this.tbAvailableHours.TabIndex = 18;
            this.tbAvailableHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblUsingHours
            // 
            this.lblUsingHours.Location = new System.Drawing.Point(16, 133);
            this.lblUsingHours.Name = "lblUsingHours";
            this.lblUsingHours.Size = new System.Drawing.Size(200, 35);
            this.lblUsingHours.TabIndex = 19;
            this.lblUsingHours.Text = "Time employee wants to use:";
            this.lblUsingHours.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvailableHours
            // 
            this.lblAvailableHours.Location = new System.Drawing.Point(18, 80);
            this.lblAvailableHours.Name = "lblAvailableHours";
            this.lblAvailableHours.Size = new System.Drawing.Size(200, 23);
            this.lblAvailableHours.TabIndex = 17;
            this.lblAvailableHours.Text = "Time employee can use:";
            this.lblAvailableHours.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvExtraHours
            // 
            this.lvExtraHours.FullRowSelect = true;
            this.lvExtraHours.GridLines = true;
            this.lvExtraHours.HideSelection = false;
            this.lvExtraHours.Location = new System.Drawing.Point(16, 249);
            this.lvExtraHours.Name = "lvExtraHours";
            this.lvExtraHours.Size = new System.Drawing.Size(765, 235);
            this.lvExtraHours.TabIndex = 30;
            this.lvExtraHours.UseCompatibleStateImageBehavior = false;
            this.lvExtraHours.View = System.Windows.Forms.View.Details;
            this.lvExtraHours.SelectedIndexChanged += new System.EventHandler(this.lvExtraHours_SelectedIndexChanged);
            this.lvExtraHours.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvExtraHours_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(738, 655);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(541, 1);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(272, 45);
            this.gbFilter.TabIndex = 25;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(175, 14);
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
            this.cbFilter.Location = new System.Drawing.Point(6, 16);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // ExtraHours
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(825, 687);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtraHours";
            this.ShowInTaskbar = false;
            this.Text = "Extra hours";
            this.Load += new System.EventHandler(this.ExtraHours_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExtraHours_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEarnedHours.ResumeLayout(false);
            this.gbEarnedHours.ResumeLayout(false);
            this.tabPageUsedHours.ResumeLayout(false);
            this.gbUsedHours.ResumeLayout(false);
            this.tabPageUsingExtraHours.ResumeLayout(false);
            this.tabPageUsingExtraHours.PerformLayout();
            this.gbUsingAheadLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUsingAhead)).EndInit();
            this.gbLastMonthData.ResumeLayout(false);
            this.gbLastMonthData.PerformLayout();
            this.gbType.ResumeLayout(false);
            this.panelTime.ResumeLayout(false);
            this.panelTime.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        void cbEmployeeE_Leave(object sender, EventArgs e)
        {
            if (cbEmployeeE.SelectedIndex == -1)
            {
                cbEmployeeE.SelectedIndex = 0;
            }
        }
        void cbEmployeeU_Leave(object sender, EventArgs e)
        {
            if (cbEmployeeU.SelectedIndex == -1)
            {
                cbEmployeeU.SelectedIndex = 0;
            }
        }

        void cbEmployee_Leave(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
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
					case ExtraHours.DateIndex:
					case ExtraHours.DateEarnedIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy", null);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
					}
					case ExtraHours.TimeIndex:
					{
						int firstID = -1;
						int secondID = -1;

						//For used hours, tag contains extraTimeAmtUsed needed for sorting, 
						//and record ID needed for deletion. Separator is ","
						//For extra and available hours, tag contains only extraTimeAmt
						if (!item1.Tag.ToString().Trim().Equals("")) 
						{
							string first = item1.Tag.ToString().Trim();
							int index = first.LastIndexOf(",");
							if (index > 0)
								first = first.Substring(0, index).Trim();
							firstID = Int32.Parse(first);
						}

						if (!item2.Tag.ToString().Trim().Equals(""))
						{
							string second = item2.Tag.ToString().Trim();
							int index = second.LastIndexOf(",");
							if (index > 0)
								second = second.Substring(0, index).Trim();
							secondID = Int32.Parse(second);
						}
						
						return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
					}
					case ExtraHours.StartTime:
					case ExtraHours.EndTime:
					{
						int firstID = -1;
						int secondID = -1;

						if (!sub1.Text.Trim().Equals("")) 
						{
							firstID = Util.Misc.transformStringTimeToMin(sub1.Text.Trim());
						}

						if (!sub2.Text.Trim().Equals(""))
						{
							secondID = Util.Misc.transformStringTimeToMin(sub2.Text.Trim());
						}
						
						return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
					}
                    case ExtraHours.TypeIndex:
                    case ExtraHours.CreatedByIndex:
                    {
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text.Trim(), sub2.Text.Trim());
                    }
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");																
				}
			}
		}

		#endregion

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("extraHoursForm", culture);
				
				// TabPage text
				tabPageEarnedHours.Text     = rm.GetString("tabPageEarnedHours", culture);
				tabPageUsedHours.Text       = rm.GetString("tabPageUsedHours", culture);
				tabPageUsingExtraHours.Text = rm.GetString("tabPageUsingExtraHours", culture);

                //check box's text
                chbHierarchyUE.Text = rm.GetString("chbHierarhicly", culture);
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbHierarchyU.Text = rm.GetString("chbHierarhicly", culture);

				// group box text
				gbEarnedHours.Text = rm.GetString("tabPageEarnedHours", culture);
				gbUsedHours.Text   = rm.GetString("tabPageUsedHours", culture);
                gbType.Text        = rm.GetString("hdrTypeOfUse", culture);
                gbLastMonthData.Text = rm.GetString("gbLastMonthData", culture);
                gbUsingAheadLimit.Text = rm.GetString("gbUsingAheadLimit", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);

				// button's text
				btnSave.Text       = rm.GetString("btnSave", culture);
				btnCancel.Text     = rm.GetString("btnCancel", culture);
				btnClose.Text      = rm.GetString("btnClose", culture);
				btnSearchE.Text    = rm.GetString("btnSearch", culture);
				btnSearchU.Text    = rm.GetString("btnSearch", culture);
				btnCalculate.Text  = rm.GetString("extraHoursCalculationForm", culture);
				btnCalculateE.Text = rm.GetString("extraHoursCalculationForm", culture);
				btnDelete.Text     = rm.GetString("btnDelete", culture);
                btnEarnedReport.Text = rm.GetString("btnReport", culture);
                btnUsingReport.Text = rm.GetString("btnReport", culture);
                btnReportUsed.Text = rm.GetString("btnReport", culture);

                // radio button's text
                rbOvertime.Text   = rm.GetString("rbOvertime", culture);
                rbExtraHours.Text = rm.GetString("rbExtraHours", culture);
                rbDiscard.Text    = rm.GetString("rbDiscard", culture);

                // check box's text
                cbTakeWholeInterval.Text = rm.GetString("cbTakeWholeInterval", culture);
                chbUsingAhead.Text = rm.GetString("chbUsingAhead", culture);

				// label's text				
				lblWorkingUnitE.Text    = rm.GetString("lblWorkingUnit", culture);
				lblEmployeeE.Text       = rm.GetString("lblEmployee", culture);
				lblFromE.Text           = rm.GetString("lblFrom", culture);
				lblToE.Text             = rm.GetString("lblTo", culture);
				lblEmplEarnedHours.Text = rm.GetString("lblEmplEarnedHours", culture);
				lblWorkingUnitU.Text    = rm.GetString("lblWorkingUnit", culture);
				lblEmployeeU.Text       = rm.GetString("lblEmployee", culture);
				lblFromU.Text           = rm.GetString("lblFrom", culture);
				lblToU.Text             = rm.GetString("lblTo", culture);
				lblEmplUsedHours.Text   = rm.GetString("lblEmplUsedHours", culture);				
				lblWorkingUnit.Text     = rm.GetString("lblWorkingUnit", culture);
				lblEmployee.Text        = rm.GetString("lblEmployee", culture);
				lblAvailableHours.Text  = rm.GetString("lblAvailableHours", culture);
				lblUsingHours.Text      = rm.GetString("lblUsingHours", culture);
				lblHrs.Text             = rm.GetString("lblHrs", culture);
				lblMin.Text             = rm.GetString("lblMin", culture);
				lblUsingDate.Text       = rm.GetString("lblUsingDate", culture);
				lblLastReaderTime.Text  = rm.GetString("lblLastReaderTime", culture);
				lblLastReaderTimeE.Text = rm.GetString("lblLastReaderTime", culture);
                lblType.Text            = rm.GetString("lblTypeOfUse", culture);
                lblHrs1.Text = rm.GetString("lblHrs", culture);
                lblEarnedHours.Text = rm.GetString("lblEarnedHours", culture);
                lblUsedAhead.Text = rm.GetString("lblUsedAhead", culture);
                	
				// list view
				lvEarnedHours.BeginUpdate();
				lvEarnedHours.Columns.Add(rm.GetString("lblEarnedDate", culture), (5 * (lvEarnedHours.Width - 4)) / 10, HorizontalAlignment.Left);
				lvEarnedHours.Columns.Add(rm.GetString("lblEarnedTime", culture), (5 * (lvEarnedHours.Width - 4)) / 10, HorizontalAlignment.Left);
				lvEarnedHours.EndUpdate();
				
				lvUsedHours.BeginUpdate();
				lvUsedHours.Columns.Add(rm.GetString("lblUsedDate", culture), (2 * (lvUsedHours.Width - 4)) / 10 - 40, HorizontalAlignment.Left);
				lvUsedHours.Columns.Add(rm.GetString("lblUsedTime", culture), ((1 * (lvUsedHours.Width - 4)) / 10) + 25, HorizontalAlignment.Left);
				lvUsedHours.Columns.Add(rm.GetString("lblStartTime", culture), ((2 * (lvUsedHours.Width - 4)) / 10) - 55, HorizontalAlignment.Left);
				lvUsedHours.Columns.Add(rm.GetString("lblEndTime", culture), ((2 * (lvUsedHours.Width - 4)) / 10) -55, HorizontalAlignment.Left);
				lvUsedHours.Columns.Add(rm.GetString("lblEarnedDate", culture), (2 * (lvUsedHours.Width - 4)) / 10 - 60, HorizontalAlignment.Left);
                lvUsedHours.Columns.Add(rm.GetString("lblTypeOfUse", culture), (1 * (lvUsedHours.Width - 4)) / 10 + 60, HorizontalAlignment.Left);
                lvUsedHours.Columns.Add(rm.GetString("lblCreatedBy", culture),  120, HorizontalAlignment.Left);
				lvUsedHours.EndUpdate();
																
				lvExtraHours.BeginUpdate();
				lvExtraHours.Columns.Add(rm.GetString("lblEarnedDate", culture), (5 * (lvExtraHours.Width - 4)) / 10, HorizontalAlignment.Left);
				lvExtraHours.Columns.Add(rm.GetString("lblAvailableTime", culture), (5 * (lvExtraHours.Width - 4)) / 10, HorizontalAlignment.Left);
				lvExtraHours.EndUpdate();				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateWorkingUnitCombo(ComboBox cbName, List<WorkingUnitTO> workingUnitArray)
		{
			try
			{
				cbName.DataSource    = workingUnitArray;
				cbName.DisplayMember = "Name";
				cbName.ValueMember   = "WorkingUnitID";

				if (workingUnitArray.Count > 0)
					cbName.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateEmployeeCombo(ComboBox cbName, List<EmployeeTO> employeeArray)
		{
			try
			{
                currnetEmplArray = employeeArray;
                cbName.DataSource    = employeeArray;
				cbName.DisplayMember = "LastName";
				cbName.ValueMember   = "EmployeeID";

				if (employeeArray.Count > 0)
					cbName.SelectedIndex = 0;

				//cbName.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

        private void populateTypeCombo()
        {
            try
            {
                ArrayList typeList = new ArrayList();
                typeList.Add(rm.GetString("all", culture));
                typeList.Add(rm.GetString("regular", culture));
                typeList.Add(rm.GetString("overtime", culture));
                typeList.Add(rm.GetString("rejected", culture));
                typeList.Add(rm.GetString("regularAdvanced",culture));

                cbType.DataSource = typeList;

                if (typeList.Count > 0)
                    cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.populateTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEarnedHours(int emplID)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime beginOfMounth = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                List<ExtraHourTO> earnedList = new ExtraHour().Search(emplID, beginOfMounth, now);
                CultureInfo ci = CultureInfo.InvariantCulture;                                

                int totalE = 0;

                if (earnedList.Count > 0)
                {
                    foreach (ExtraHourTO extraHour in earnedList)
                    {
                        if (extraHour.ExtraTimeAmt > 0)
                        {                            
                            totalE += extraHour.ExtraTimeAmt;
                        }
                    }
                }

                tbEarnedHours.Text = Util.Misc.transformMinToStringTime(totalE);			
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.populateEarnedHours(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateUsedAheadHours(int emplID)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime beginOfMounth = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                List<ExtraHourTO> earnedList = new ExtraHour().Search(emplID, beginOfMounth, new DateTime());
                CultureInfo ci = CultureInfo.InvariantCulture;

                int totalE = 0;

                if (earnedList.Count > 0)
                {
                    foreach (ExtraHourTO extraHour in earnedList)
                    {
                        if (extraHour.ExtraTimeAmt < 0)
                        {
                            totalE += Math.Abs(extraHour.ExtraTimeAmt);
                        }
                    }
                }

                tbUsedAhead.Text = Util.Misc.transformMinToStringTime(totalE);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.populateUsedAheadHours(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		/// <summary>
		/// Populate List View with earned hours found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateEarnedListView()
		{
			try
			{			
				List<ExtraHourTO> earnedList = new ExtraHour().Search((int)cbEmployeeE.SelectedValue, dtpFromE.Value, dtpToE.Value);
				CultureInfo ci = CultureInfo.InvariantCulture;

				lvEarnedHours.BeginUpdate();
				lvEarnedHours.Items.Clear();

				int totalE = 0;

				if (earnedList.Count > 0)
				{
					foreach(ExtraHourTO extraHour in earnedList)
					{
                        if (extraHour.ExtraTimeAmt > 0)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = extraHour.DateEarned.ToString("dd.MM.yyyy", ci);
                            item.SubItems.Add(extraHour.CalculatedTimeAmt);
                            item.Tag = extraHour.ExtraTimeAmt;

                            lvEarnedHours.Items.Add(item);

                            totalE += extraHour.ExtraTimeAmt;
                        }
					}
				}

				lvEarnedHours.EndUpdate();
				lvEarnedHours.Invalidate();

				lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(totalE);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.populateEarnedListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with used hours found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateUsedListView()
		{
			try
			{
                string type = "";
                if (cbType.SelectedItem.ToString().Equals(rm.GetString("regular", culture)))
                {
                    type = Constants.extraHoursUsedRegular;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("overtime", culture)))
                {
                    type = Constants.extraHoursUsedOvertime;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("rejected", culture)))
                {
                    type = Constants.extraHoursUsedRejected;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("regularAdvanced", culture)))
                {
                    type = Constants.extraHoursUsedRegularAdvanced;
                }

                List<ExtraHourUsedTO> usedList = new ExtraHourUsed().Search((int)cbEmployeeU.SelectedValue, dtpFromU.Value, dtpToU.Value, type);
				CultureInfo ci = CultureInfo.InvariantCulture;

				lvUsedHours.BeginUpdate();
				lvUsedHours.Items.Clear();

				int totalU = 0;

				if (usedList.Count > 0)
				{
					foreach(ExtraHourUsedTO extraHourUsed in usedList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = extraHourUsed.DateUsed.ToString("dd.MM.yyyy", ci);
                        item.SubItems.Add(extraHourUsed.CalculatedTimeAmtUsed);
                        //item.SubItems.Add(Util.Misc.transformTimeToStringTime(extraHourUsed.StartTime.Hour, extraHourUsed.StartTime.Minute));
                        item.SubItems.Add(extraHourUsed.StartTime.ToString("HH:mm", ci));
                        //item.SubItems.Add(Util.Misc.transformTimeToStringTime(extraHourUsed.EndTime.Hour, extraHourUsed.EndTime.Minute));
                        item.SubItems.Add(extraHourUsed.EndTime.ToString("HH:mm", ci));
                        item.SubItems.Add(extraHourUsed.DateEarned.ToString("dd.MM.yyyy", ci));

                        if (extraHourUsed.Type == Constants.extraHoursUsedRegular)
                            item.SubItems.Add(rm.GetString("regular", culture));
                        else if (extraHourUsed.Type == Constants.extraHoursUsedOvertime)
                            item.SubItems.Add(rm.GetString("overtime", culture));
                        else if (extraHourUsed.Type == Constants.extraHoursUsedRejected)
                            item.SubItems.Add(rm.GetString("rejected", culture));
                        else if (extraHourUsed.Type == Constants.extraHoursUsedRegularAdvanced)
                            item.SubItems.Add(rm.GetString("regularAdvanced", culture));
                        else
                            item.SubItems.Add("");

                        item.SubItems.Add(extraHourUsed.CreatedByName);

                        //Tag contains extraTimeAmtUsed needed for sorting, and record ID needed for deletion
                        //separator is ","
                        item.Tag = extraHourUsed.ExtraTimeAmtUsed + "," + extraHourUsed.RecordID;

                        lvUsedHours.Items.Add(item);
                        if (extraHourUsed.ExtraTimeAmtUsed > 0)
                        {
                            totalU += extraHourUsed.ExtraTimeAmtUsed;
                        }
                        else
                        {
                            totalU -= extraHourUsed.ExtraTimeAmtUsed;
                        }
                    }
				}

				lvUsedHours.EndUpdate();
				lvUsedHours.Invalidate();

				lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(totalU);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.populateUsedListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with available hours found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateExtraListView()
		{
			try
			{
				List<ExtraHourTO> availableList = new ExtraHour().SearchEmployeeAvailableDates((int)cbEmployee.SelectedValue);
				CultureInfo ci = CultureInfo.InvariantCulture;

                List<ExtraHourUsedTO> usedAheadList = new ExtraHourUsed().SearchByType((int)cbEmployee.SelectedValue,Constants.extraHoursUsedRegularAdvanced);

				lvExtraHours.BeginUpdate();
				lvExtraHours.Items.Clear();

				int total = 0;

				if (availableList.Count > 0)
				{
					foreach(ExtraHourTO extraHour in availableList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = extraHour.DateEarned.ToString("dd.MM.yyyy", ci);
						item.SubItems.Add(extraHour.CalculatedTimeAmt);
						item.Tag = extraHour.ExtraTimeAmt;
						
						lvExtraHours.Items.Add(item);

						total += extraHour.ExtraTimeAmt;
					}
				}
                if (usedAheadList.Count > 0)
                {
                    foreach (ExtraHourUsedTO extraHourUsed in usedAheadList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = extraHourUsed.DateEarned.ToString("dd/MM/yyyy", ci);
                        item.SubItems.Add(extraHourUsed.CalculatedTimeAmtUsed);
                        item.Tag = extraHourUsed.ExtraTimeAmtUsed;

                        //lvExtraHours.Items.Add(item);

                        total += extraHourUsed.ExtraTimeAmtUsed;
                    }
                }

				lvExtraHours.EndUpdate();
				lvExtraHours.Invalidate();

				tbAvailableHours.Text = Util.Misc.transformMinToStringTime(total);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.populateExtraListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void clearListView(ListView lvName)
		{
			lvName.BeginUpdate();
			lvName.Items.Clear();
			lvName.EndUpdate();
			lvName.Invalidate();
		}
        private void clearEarnedAndUsedHours()
        {
            tbEarnedHours.Text = "";
            tbUsedAhead.Text = "";
        }

		private void ExtraHours_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

                // Initialize comparer object
                _comp1 = new ListViewItemComparer(lvEarnedHours);
                lvEarnedHours.ListViewItemSorter = _comp1;
                lvEarnedHours.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvUsedHours);
                lvUsedHours.ListViewItemSorter = _comp2;
                lvUsedHours.Sorting = SortOrder.Ascending;

                _comp3 = new ListViewItemComparer(lvExtraHours);
                lvExtraHours.ListViewItemSorter = _comp3;
                lvExtraHours.Sorting = SortOrder.Ascending;

                wUnits = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ExtraHoursPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                List<WorkingUnitTO> workingUnitArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    workingUnitArray.Add(wuTO);
                }

                workingUnitArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                //With clone, each of this 3 working units is separate. Without it, they are all the same,
                //which means that selecting something in one of them will automaticly
                //be selected in the next two

                //ArrayList workingUnitEArray = (ArrayList) workingUnitArray.Clone();
                //ArrayList workingUnitUArray = (ArrayList) workingUnitArray.Clone();

                //populateWorkingUnitCombo(cbWorkingUnitE, workingUnitEArray);
                populateWorkingUnitCombo(cbWorkingUnitE, workingUnitArray);
                //populateWorkingUnitCombo(cbWorkingUnitU, workingUnitUArray);
                populateWorkingUnitCombo(cbWorkingUnitU, workingUnitArray);
                populateWorkingUnitCombo(cbWorkingUnit, workingUnitArray);

                dtpFromE.Value = new DateTime(DateTime.Now.Year, 1, 1);
                dtpFromU.Value = new DateTime(DateTime.Now.Year, 1, 1);

                dtpUsingDate.Value = DateTime.Now;
                tbUsingHours.Text = "8";
                cbUsingMinutes.SelectedIndex = 0;

                DateTime allReadersDate = (new Reader()).SearchAllReadersLastReadTime();
                if (allReadersDate != new DateTime(0))
                {
                    lblLastReaderTime.Text = lblLastReaderTime.Text + " " + allReadersDate.ToString("dd.MM.yyyy  HH:mm");
                    lblLastReaderTimeE.Text = lblLastReaderTime.Text;
                }

                populateTypeCombo();

                if (logInUser.ExtraHoursAdvancedAmt <= 0)
                {
                    gbUsingAheadLimit.Visible = false;
                    gbLastMonthData.Visible = false;
                    chbUsingAhead.Visible = false;
                }
                else
                {
                    gbUsingAheadLimit.Visible = true;
                    gbLastMonthData.Visible = true;
                    chbUsingAhead.Visible = true;
                }
                numUsingAhead.Value = logInUser.ExtraHoursAdvancedAmt;
                cbTakeWholeInterval.Visible = false;
                visibleIntervalCheckBox = false;
                numUsingAhead.Enabled = false;

                tabControl1.SelectedIndex = 2;

                filter = new Filter();
                filter.TabID = this.tabControl1.SelectedTab.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.ExtraHours_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow; 
            }
		}		

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			bool trans = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ExtraHourUsed used = new ExtraHourUsed();
                int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                char s = tbAvailableHours.Text.Trim()[0];
                int j = -1;
                bool positive = int.TryParse(s.ToString(), out j);

                if (!positive)
                {
                    timeEmployeeCanUse = -1 * timeEmployeeCanUse;
                }

                int avaiableTime = 0;

                foreach (ListViewItem item in lvExtraHours.Items)
                {
                    if ((int)item.Tag > 0)
                        avaiableTime += (int)item.Tag;
                }

                int wTime = 0;
                if (cbTakeWholeInterval.Checked && (rbDiscard.Checked || rbOvertime.Checked))
                {
                    foreach (ListViewItem item in lvExtraHours.SelectedItems)
                    {
                        wTime += (int)item.Tag;
                    }
                }
                else
                {
                    wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                             + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                }

                int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                if (((timeEmployeeCanUse < 0) && ((Math.Abs(timeEmployeeCanUse) + wTime) > usingAheadLimit)) || ((timeEmployeeCanUse >= 0) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                {
                    MessageBox.Show(rm.GetString("limitBreaken", culture));
                    return;
                }
                if ((!chbUsingAhead.Checked) && (timeEmployeeCanUse < wTime))
                {
                    MessageBox.Show(rm.GetString("notEnoughTime", culture));
                    return;
                }
                //if UsingAhead is not check
                if ((!chbUsingAhead.Checked) || (timeEmployeeCanUse >= wTime) || (avaiableTime > wTime))
                {
                    //If Date of Use is in the past, display a warning that that used hours 
                    //can not be changed. It is only apply on regular work
                    if ((rbExtraHours.Checked) && (dtpUsingDate.Value.Date < DateTime.Now.Date) && (cbEmployee.SelectedIndex > 0)
                        && (lvExtraHours.SelectedItems.Count > 0))
                    {
                        CultureInfo ci = CultureInfo.InvariantCulture;
                        StringBuilder message = new StringBuilder();
                        message.Append(rm.GetString("lblEmployee", culture) + " " +
                            cbEmployee.GetItemText(cbEmployee.SelectedItem) + "\n");
                        message.Append(rm.GetString("lblUsingHours", culture) + " " + tbUsingHours.Text + " " +
                            rm.GetString("lblHrs", culture) + " " + (Int32.Parse(cbUsingMinutes.SelectedItem.ToString())).ToString() + " " +
                            rm.GetString("lblMin", culture) + "\n");
                        message.Append(rm.GetString("lblUsingDate", culture) + " " +
                            dtpUsingDate.Value.ToString("dd/MM/yyyy HH:mm", ci) + "\n");
                        message.Append(rm.GetString("usingDateForPast", culture));

                        DialogResult result = MessageBox.Show(message.ToString(), "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    string type = "";
                    if (rbExtraHours.Checked)
                    {
                        type = Constants.extraHoursUsedRegular;
                    }
                    else if (rbOvertime.Checked)
                    {
                        type = Constants.extraHoursUsedOvertime;
                    }
                    else if (rbDiscard.Checked)
                    {
                        type = Constants.extraHoursUsedRejected;
                    }

                    int wantedTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                        + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());

                    DateTime basicStartTime = new DateTime();
                    DateTime basicEndTime = new DateTime();
                    if (rbExtraHours.Checked)
                    {
                        basicStartTime = new DateTime(1, 1, 1, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                        basicEndTime = basicStartTime.AddMinutes(wantedTime);
                    }

                    int selectedTimeAmount = 0;

                    foreach (ListViewItem item in lvExtraHours.SelectedItems)
                    {
                        selectedTimeAmount += (int)item.Tag;
                    }

                    //if TakeWholeInterval is checked, than, everything that is selected is wanted time
                    if (((rbOvertime.Checked) || (rbDiscard.Checked)) && (cbTakeWholeInterval.Checked))
                        wantedTime = selectedTimeAmount;

                    if (cbEmployee.SelectedIndex <= 0)
                        MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
                    else if (lvExtraHours.SelectedItems.Count <= 0)
                        MessageBox.Show(rm.GetString("selectDatesEarned", culture));
                    else if (selectedTimeAmount < wantedTime)
                        MessageBox.Show(rm.GetString("notEnoughWantedTime", culture));
                    //Already exist some extra hours in the same date, that overlaps with this one
                    else if ((rbExtraHours.Checked) && (new ExtraHourUsed().ExistOverlap((int)cbEmployee.SelectedValue,
                        dtpUsingDate.Value.Date, basicStartTime, basicEndTime)))
                        MessageBox.Show(rm.GetString("extraHourOverlap", culture));
                    //Already exist some IO Pair in the same date and same start and end time
                    else if ((rbExtraHours.Checked) && (new IOPair().ExistEmlpoyeeDatePair((int)cbEmployee.SelectedValue,
                        dtpUsingDate.Value.Date, basicStartTime, basicEndTime)))
                        MessageBox.Show(rm.GetString("extraHourExistEmlpoyeeDatePair", culture));
                    else
                    {
                        bool continueCalculation = false;
                        int emploeeID = (int)cbEmployee.SelectedValue;

                        if (rbExtraHours.Checked)
                        {
                            // all time shemas
                            List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search();

                            // list of Time Schemas for employee and date used
                            List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(emploeeID.ToString(),
                                dtpUsingDate.Value, dtpUsingDate.Value);
                            if (timeScheduleList.Count == 0)
                                MessageBox.Show(rm.GetString("noEmployeeTimeScheduleDateUsed", culture));
                            else
                            {
                                bool is2DaysShift = false;
                                bool is2DaysShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(timeScheduleList, dtpUsingDate.Value.Date, ref is2DaysShift,
                                    ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                                if (dayIntervals == null)
                                    MessageBox.Show(rm.GetString("noEmployeeTimeScheduleDateUsed", culture));
                                else
                                {
                                    //Check if using date is holiday
                                    HolidayTO holidayTO = (new Holiday()).Find(dtpUsingDate.Value.Date);
                                    //Find schema for using date, to see the type
                                    WorkTimeSchemaTO dateUsedSchema = Common.Misc.getTimeSchemaForDay(timeScheduleList, dtpUsingDate.Value.Date, timeSchemas);

                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[0]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        && (currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                        //not working day is displayed with only one interval, from (0, 0, 0) to (0, 0, 0)
                                        MessageBox.Show(rm.GetString("notWorkingDay", culture));
                                    //Selected date of use is Holiday, so, it is not a working day
                                    //It is checked only for non industrial schemas
                                    else if ((dateUsedSchema.Type.Trim() != Constants.schemaTypeIndustrial)
                                        && (holidayTO.HolidayDate != (new DateTime(0))))
                                        MessageBox.Show(rm.GetString("dateUsedIsHoliday", culture));
                                    else
                                    {
                                        int index = -1;
                                        DateTime timeOfUse = new DateTime(0);
                                        for (int i = 0; i < dayIntervals.Count; i++)
                                        {
                                            currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                            if ((currentTimeSchemaInterval.StartTime.TimeOfDay >= timeOfUse.TimeOfDay)
                                                && (currentTimeSchemaInterval.StartTime.TimeOfDay <= basicStartTime.TimeOfDay))
                                            {
                                                timeOfUse = currentTimeSchemaInterval.StartTime;
                                                index = i;
                                            }
                                        } //for

                                        if (index == -1)
                                            MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                        else
                                        {
                                            currentTimeSchemaInterval = dayIntervals[index]; //key is interval_num which is 0, 1...
                                            if (currentTimeSchemaInterval.EndTime.TimeOfDay <= basicStartTime.TimeOfDay)
                                                MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                            else if (currentTimeSchemaInterval.EndTime.TimeOfDay < basicEndTime.TimeOfDay)
                                                MessageBox.Show(rm.GetString("workingIntervalOverflow", culture));
                                            else
                                                continueCalculation = true;
                                        } //(index != -1)
                                    } //working day
                                } //day intervals
                            } //time schedule
                        }//regular work
                        else
                        {
                            //overtime or rejected
                            continueCalculation = true;
                        }

                        if (continueCalculation)
                        {                            
                            trans = used.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    IOPair ioPair = new IOPair();
                                    if (rbExtraHours.Checked)
                                        ioPair.SetTransaction(used.GetTransaction());

                                    //Get original sorting Column and Order
                                    SortOrder prevOrder = lvExtraHours.Sorting;
                                    int prevSortColumn = _comp3.SortColumn;

                                    //Set sort column to available time, and order to ascending
                                    //in this way, wanted time will be taken from the smallest parts first
                                    lvExtraHours.Sorting = SortOrder.None;
                                    _comp3.SortColumn = ExtraHours.TimeIndex;
                                    lvExtraHours.Sorting = SortOrder.Ascending;

                                    bool isInserted = true;
                                    selectedTimeAmount = 0;
                                    foreach (ListViewItem item in lvExtraHours.SelectedItems)
                                    {
                                        DateTime startTime = new DateTime();
                                        DateTime startIOPairTime = new DateTime();
                                        if (rbExtraHours.Checked)
                                        {
                                            startTime = new DateTime(1, 1, 1, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                                            startTime = startTime.AddMinutes(selectedTimeAmount);

                                            startIOPairTime = new DateTime(dtpUsingDate.Value.Year, dtpUsingDate.Value.Month, dtpUsingDate.Value.Day, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                                            startIOPairTime = startIOPairTime.AddMinutes(selectedTimeAmount);
                                        }

                                        int currentTime = (int)item.Tag;
                                        int currentUseTime = 0;
                                        if ((selectedTimeAmount + currentTime) <= wantedTime)
                                        {
                                            selectedTimeAmount += currentTime;
                                            currentUseTime = currentTime;
                                        }
                                        else
                                        {
                                            currentUseTime = wantedTime - selectedTimeAmount;
                                            selectedTimeAmount = wantedTime;
                                        }

                                        if (isInserted)
                                        {
                                            string[] dateElements = item.SubItems[0].Text.Split('.');
                                            if (dateElements.Length == 3)
                                            {
                                                int day = Int32.Parse(dateElements[0]);
                                                int month = Int32.Parse(dateElements[1]);
                                                int year = Int32.Parse(dateElements[2]);
                                                DateTime earnedDate = new DateTime(year, month, day, 0, 0, 0);

                                                DateTime endTime = new DateTime();
                                                DateTime endIOPairTime = new DateTime();
                                                if (rbExtraHours.Checked)
                                                {
                                                    endTime = startTime.AddMinutes(currentUseTime);
                                                    endIOPairTime = startIOPairTime.AddMinutes(currentUseTime);
                                                }

                                                int ioPairID = 0;
                                                if (rbExtraHours.Checked)
                                                {
                                                    ioPairID = ioPair.SaveExtraHourPair(dtpUsingDate.Value.Date, emploeeID, 0, 1, Constants.extraHoursIOPairInsertPassType,
                                                        startIOPairTime, endIOPairTime, 0, false);
                                                }
                                                if (ioPairID >= 0) //>= because here Identity is returned, for error is -1
                                                {
                                                    DateTime usingDate = earnedDate;
                                                    if (rbExtraHours.Checked)
                                                    {
                                                        usingDate = dtpUsingDate.Value.Date;
                                                    }
                                                    else
                                                    {
                                                        ioPairID = -1;
                                                    }

                                                    int inserted = used.Save(emploeeID,
                                                        earnedDate, usingDate, currentUseTime, startTime, endTime, ioPairID, type, false);
                                                    //earnedDate, dtpUsingDate.Value.Date, currentUseTime, startTime, endTime, ioPairID, type, false);
                                                    isInserted = (inserted > 0 ? true : false) && isInserted;
                                                }
                                                else
                                                    isInserted = false;
                                            }
                                            else
                                            {
                                                isInserted = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            isInserted = false;
                                            break;
                                        }

                                        if (selectedTimeAmount == wantedTime)
                                            break;
                                    } //foreach

                                    //Return original sorting Column and Order
                                    lvExtraHours.Sorting = SortOrder.None;
                                    _comp3.SortColumn = prevSortColumn;
                                    lvExtraHours.Sorting = prevOrder;

                                    if (isInserted)
                                    {
                                        used.CommitTransaction();

                                        //to refresh lvExtraHours list, tbAvailableHours and tbUsingHours text box, 
                                        //cbUsingMinutes and dtpUsingDate combo boxes
                                        //but do not change what is checked in radio buttons
                                        selectRegularWork = false;
                                        cbEmployee_SelectedIndexChanged(sender, e);
                                        selectRegularWork = true;

                                        //to refresh lvUsedHours list
                                        //cbEmployeeU_SelectedIndexChanged(sender, e);
                                        if (cbEmployeeU.SelectedIndex > 0)
                                        {
                                            TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployeeU.SelectedValue.ToString());
                                            if (employeeTO.EmployeeID != -1)
                                            {
                                                populateUsedListView();
                                            }
                                            else
                                            {
                                                clearListView(lvUsedHours);
                                                lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
                                            }
                                        }

                                        MessageBox.Show(rm.GetString("EmployeeXAccessGroupSaved", culture));
                                    }
                                    else
                                    {
                                        used.RollbackTransaction();
                                        MessageBox.Show(rm.GetString("EmployeeXAccessGroupNotSaved", culture));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (used.GetTransaction() != null)
                                        used.RollbackTransaction();
                                    MessageBox.Show(ex.Message);
                                }
                            } //if (trans)
                            else
                            {
                                MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                return;
                            }
                        } //selectedTimeAmount >= wantedTime
                    } //else	
                }//chbUsingAhead is not checked

                //if Using ahead is selected and time that empoyee can use is less then wanted time add REGULAR_WORK_ADVANCED to extra_hours_used and extra_hours
                //and add IOPairs for specified date and time
                else //((chbUsingAhead.Checked) && (Util.Misc.transformStringTimeToMin(tbAvailableHours.Text) < wTime))
                {
                    //select all positive items in list
                    foreach (ListViewItem item in lvExtraHours.Items)
                    {
                        if (int.Parse(item.Tag.ToString()) > 0)
                        {
                            item.Selected = true;
                        }
                    }
                    bool usingAhead = false;

                    int emploeeID = Int32.Parse(cbEmployee.SelectedValue.ToString());
                    int wantedTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                        + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                    avaiableTime = 0;

                    foreach (ListViewItem item in lvExtraHours.Items)
                    {
                        if ((int)item.Tag > 0)
                            avaiableTime += (int)item.Tag;
                    }

                    int hoursUsingAhead = 0;
                    if (avaiableTime > 0)
                    {
                        hoursUsingAhead = wantedTime - avaiableTime;
                    }
                    else
                    {
                        hoursUsingAhead = wantedTime;
                        usingAhead = true;
                    }

                    DateTime basicStartTime = new DateTime();
                    DateTime basicEndTime = new DateTime();

                    DateTime basicStartTimeAhead = new DateTime();
                    DateTime basicEndTimeAhead = new DateTime();

                    // if avaiable time exist than add this time as regular work and add IOPair's
                    basicStartTime = new DateTime(1, 1, 1, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                    if (avaiableTime > 0)
                    {
                        basicEndTime = basicStartTime.AddMinutes(avaiableTime);
                    }
                    else basicEndTime = basicStartTime;

                    basicStartTimeAhead = basicEndTime;
                    basicEndTimeAhead = basicStartTimeAhead.AddMinutes(hoursUsingAhead);

                    CultureInfo ci = CultureInfo.InvariantCulture;
                    StringBuilder message = new StringBuilder();
                    message.Append(rm.GetString("lblEmployee", culture) + " " +
                        cbEmployee.GetItemText(cbEmployee.SelectedItem) + "\n");
                    message.Append(rm.GetString("lblUsingHours", culture) + " " + wantedTime / 60 + " " +
                        rm.GetString("lblHrs", culture) + " " + wantedTime % 60 + " " +
                        rm.GetString("lblMin", culture) + "\n");
                    message.Append(rm.GetString("lblUsingTimeAhead", culture) + " " + hoursUsingAhead / 60 + " " +
                            rm.GetString("lblHrs", culture) + " " + hoursUsingAhead % 60 + " " +
                            rm.GetString("lblMin", culture) + "\n");
                    message.Append(rm.GetString("lblUsingDate", culture) + " " +
                        dtpUsingDate.Value.ToString("dd/MM/yyyy HH:mm", ci) + "\n");
                    message.Append(rm.GetString("usingHoursAhead", culture));

                    DialogResult result = MessageBox.Show(message.ToString(), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return;
                    }

                    if (cbEmployee.SelectedIndex <= 0)
                        MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
                    //Already exist some extra hours in the same date, that overlaps with this one
                    else if ((rbExtraHours.Checked) && (new ExtraHourUsed().ExistOverlap((int)cbEmployee.SelectedValue,
                        dtpUsingDate.Value.Date, basicStartTime, basicEndTime)))
                        MessageBox.Show(rm.GetString("extraHourOverlap", culture));
                    //Already exist some IO Pair in the same date and same start and end time
                    else if ((new IOPair().ExistEmlpoyeeDatePair((int)cbEmployee.SelectedValue,
                   dtpUsingDate.Value.Date, basicStartTime, basicEndTimeAhead)))
                        MessageBox.Show(rm.GetString("extraHourExistEmlpoyeeDatePair", culture));
                    else
                    {
                        int selectedTimeAmount = 0;

                        foreach (ListViewItem item in lvExtraHours.Items)
                        {
                            if ((int)item.Tag > 0)
                                selectedTimeAmount += (int)item.Tag;
                        }

                        // all time shemas
                        List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search();

                        // list of Time Schemas for employee and date used
                        List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(emploeeID.ToString(),
                            dtpUsingDate.Value, dtpUsingDate.Value);
                        if (timeScheduleList.Count == 0)
                            MessageBox.Show(rm.GetString("noEmployeeTimeScheduleDateUsed", culture));
                        else
                        {
                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(timeScheduleList, dtpUsingDate.Value.Date, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                            if (dayIntervals == null)
                                MessageBox.Show(rm.GetString("noEmployeeTimeScheduleDateUsed", culture));
                            else
                            {
                                //Check if using date is holiday
                                TransferObjects.HolidayTO holidayTO = (new Holiday()).Find(dtpUsingDate.Value.Date);
                                //Find schema for using date, to see the type
                                WorkTimeSchemaTO dateUsedSchema = Common.Misc.getTimeSchemaForDay(timeScheduleList, dtpUsingDate.Value.Date, timeSchemas);

                                WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[0]; //key is interval_num which is 0, 1...
                                if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                    && (currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                    //not working day is displayed with only one interval, from (0, 0, 0) to (0, 0, 0)
                                    MessageBox.Show(rm.GetString("notWorkingDay", culture));
                                //Selected date of use is Holiday, so, it is not a working day
                                //It is checked only for non industrial schemas
                                else if ((dateUsedSchema.Type.Trim() != Constants.schemaTypeIndustrial)
                                    && (holidayTO.HolidayDate != (new DateTime(0))))
                                    MessageBox.Show(rm.GetString("dateUsedIsHoliday", culture));
                                else
                                {
                                    bool valid = false;
                                    int index = -1;
                                    DateTime timeOfUse = new DateTime(0);
                                    for (int i = 0; i < dayIntervals.Count; i++)
                                    {
                                        currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                        if ((currentTimeSchemaInterval.StartTime.TimeOfDay >= timeOfUse.TimeOfDay)
                                            && (currentTimeSchemaInterval.StartTime.TimeOfDay <= basicStartTime.TimeOfDay))
                                        {
                                            timeOfUse = currentTimeSchemaInterval.StartTime;
                                            index = i;
                                        }
                                    } //for

                                    if (index == -1)
                                    {
                                        MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                        return;
                                    }
                                    else
                                    {
                                        currentTimeSchemaInterval = dayIntervals[index]; //key is interval_num which is 0, 1...
                                        if (currentTimeSchemaInterval.EndTime.TimeOfDay <= basicStartTime.TimeOfDay)
                                            MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                        else if (currentTimeSchemaInterval.EndTime.TimeOfDay < basicEndTimeAhead.TimeOfDay)
                                            MessageBox.Show(rm.GetString("workingIntervalOverflow", culture));
                                        else
                                        {
                                            trans = used.BeginTransaction();
                                            valid = true;
                                        }
                                    }
                                    if (valid)
                                    {
                                        if (trans)
                                        {
                                            IOPair ioPair = new IOPair();
                                            if (rbExtraHours.Checked)
                                                ioPair.SetTransaction(used.GetTransaction());

                                            //Get original sorting Column and Order
                                            SortOrder prevOrder = lvExtraHours.Sorting;
                                            int prevSortColumn = _comp3.SortColumn;

                                            //Set sort column to available time, and order to ascending
                                            //in this way, wanted time will be taken from the smallest parts first
                                            lvExtraHours.Sorting = SortOrder.None;
                                            _comp3.SortColumn = ExtraHours.TimeIndex;
                                            lvExtraHours.Sorting = SortOrder.Ascending;

                                            bool isInserted = true;
                                            selectedTimeAmount = 0;
                                            foreach (ListViewItem item in lvExtraHours.SelectedItems)
                                            {
                                                DateTime startTime = new DateTime();
                                                DateTime startIOPairTime = new DateTime();
                                                if (rbExtraHours.Checked)
                                                {
                                                    startTime = new DateTime(1, 1, 1, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                                                    startTime = startTime.AddMinutes(selectedTimeAmount);

                                                    startIOPairTime = new DateTime(dtpUsingDate.Value.Year, dtpUsingDate.Value.Month, dtpUsingDate.Value.Day, dtpUsingDate.Value.Hour, dtpUsingDate.Value.Minute, 0);
                                                    startIOPairTime = startIOPairTime.AddMinutes(selectedTimeAmount);
                                                }

                                                int currentTime = (int)item.Tag;
                                                int currentUseTime = 0;
                                                if ((selectedTimeAmount + currentTime) <= avaiableTime)
                                                {
                                                    selectedTimeAmount += currentTime;
                                                    currentUseTime = currentTime;
                                                }
                                                else
                                                {
                                                    currentUseTime = avaiableTime - selectedTimeAmount;
                                                    selectedTimeAmount = avaiableTime;
                                                }

                                                if (isInserted)
                                                {
                                                    string[] dateElements = item.SubItems[0].Text.Split('/');
                                                    if (dateElements.Length == 3)
                                                    {
                                                        int day = Int32.Parse(dateElements[0]);
                                                        int month = Int32.Parse(dateElements[1]);
                                                        int year = Int32.Parse(dateElements[2]);
                                                        DateTime earnedDate = new DateTime(year, month, day, 0, 0, 0);

                                                        DateTime endTime = new DateTime();
                                                        DateTime endIOPairTime = new DateTime();
                                                        if (rbExtraHours.Checked)
                                                        {
                                                            endTime = startTime.AddMinutes(currentUseTime);
                                                            endIOPairTime = startIOPairTime.AddMinutes(currentUseTime);
                                                        }

                                                        int ioPairID = 0;
                                                        if (rbExtraHours.Checked)
                                                        {
                                                            ioPairID = ioPair.SaveExtraHourPair(dtpUsingDate.Value.Date, emploeeID, 0, 1, Constants.extraHoursIOPairInsertPassType,
                                                                startIOPairTime, endIOPairTime, 0, false);
                                                        }
                                                        if (ioPairID >= 0) //>= because here Identity is returned, for error is -1
                                                        {
                                                            DateTime usingDate = earnedDate;
                                                            if (rbExtraHours.Checked)
                                                            {
                                                                usingDate = dtpUsingDate.Value.Date;
                                                            }
                                                            else
                                                            {
                                                                ioPairID = -1;
                                                            }
                                                            try
                                                            {
                                                                int inserted = used.Save(emploeeID,
                                                                    earnedDate, usingDate, currentUseTime, startTime, endTime, ioPairID, Constants.extraHoursUsedRegular, false);
                                                                //earnedDate, dtpUsingDate.Value.Date, currentUseTime, startTime, endTime, ioPairID, type, false);
                                                                isInserted = (inserted > 0 ? true : false) && isInserted;
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                used.RollbackTransaction();
                                                                MessageBox.Show(ex.Message);
                                                            }
                                                        }
                                                        else
                                                            isInserted = false;
                                                    }
                                                    else
                                                        break;
                                                }
                                                else
                                                    break;

                                                if (selectedTimeAmount == avaiableTime)
                                                    break;
                                            } //foreach

                                            //Return original sorting Column and Order
                                            lvExtraHours.Sorting = SortOrder.None;
                                            _comp3.SortColumn = prevSortColumn;
                                            lvExtraHours.Sorting = prevOrder;

                                            if (isInserted)
                                            {
                                                used.CommitTransaction();
                                                //MessageBox.Show(rm.GetString("EmployeeXAccessGroupSaved", culture));
                                                usingAhead = true;
                                            }
                                            else
                                            {
                                                used.RollbackTransaction();
                                                usingAhead = false;
                                                MessageBox.Show(rm.GetString("EmployeeXAccessGroupNotSaved", culture));
                                            }
                                        } //if (trans)
                                        else
                                        {
                                            MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                            return;
                                        }

                                    }
                                    //if all condition's are satisfied than add IOPair, extraHour and ectraHourUsed
                                    if (usingAhead)
                                    {
                                        int ind = -1;
                                        DateTime time = new DateTime(0);
                                        for (int i = 0; i < dayIntervals.Count; i++)
                                        {
                                            currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                            if ((currentTimeSchemaInterval.StartTime.TimeOfDay >= time.TimeOfDay)
                                                && (currentTimeSchemaInterval.StartTime.TimeOfDay <= basicStartTimeAhead.TimeOfDay))
                                            {
                                                time = currentTimeSchemaInterval.StartTime;
                                                ind = i;
                                            }
                                        } //for

                                        if (ind == -1)
                                            MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                        else
                                        {
                                            currentTimeSchemaInterval = dayIntervals[ind]; //key is interval_num which is 0, 1...
                                            if (currentTimeSchemaInterval.EndTime.TimeOfDay <= basicStartTimeAhead.TimeOfDay)
                                                MessageBox.Show(rm.GetString("notWorkingInterval", culture));
                                            else if (currentTimeSchemaInterval.EndTime.TimeOfDay < basicEndTimeAhead.TimeOfDay)
                                                MessageBox.Show(rm.GetString("workingIntervalOverflow", culture));
                                            else
                                            {
                                                ExtraHourUsed extraHourUsed = new ExtraHourUsed();
                                                trans = extraHourUsed.BeginTransaction();
                                                bool inserted = false;
                                                if (trans)
                                                {
                                                    IOPair ioPair = new IOPair();
                                                    ioPair.SetTransaction(extraHourUsed.GetTransaction());
                                                    ExtraHour extraHour = new ExtraHour();
                                                    extraHour.SetTransaction(extraHourUsed.GetTransaction());

                                                    int extraHourInsert = extraHour.Save(emploeeID, dtpUsingDate.Value.Date, -1 * hoursUsingAhead, false);

                                                    if (extraHourInsert > 0)
                                                    {
                                                        DateTime startIOPairTime = new DateTime(dtpUsingDate.Value.Year, dtpUsingDate.Value.Month, dtpUsingDate.Value.Day, basicStartTimeAhead.Hour, basicStartTimeAhead.Minute, 0);
                                                        DateTime endIOPairTime = startIOPairTime.AddMinutes(hoursUsingAhead);

                                                        int PairID = ioPair.SaveExtraHourPair(dtpUsingDate.Value.Date, emploeeID, 0, 1, Constants.extraHoursAheadIOPairInsertPassType,
                                                                      startIOPairTime, endIOPairTime, 0, false);
                                                        if (PairID > 0)
                                                        {
                                                            try
                                                            {
                                                                int extraHourUsedInsert = extraHourUsed.Save(emploeeID, dtpUsingDate.Value.Date, dtpUsingDate.Value.Date, -1 * hoursUsingAhead, basicStartTimeAhead, basicEndTimeAhead, PairID, Constants.extraHoursUsedRegularAdvanced, false);

                                                                if (extraHourUsedInsert >= 0)
                                                                {
                                                                    inserted = true;
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                if (extraHourUsed.GetTransaction() != null)
                                                                    extraHourUsed.RollbackTransaction();
                                                                MessageBox.Show(ex.Message);
                                                            }
                                                        }
                                                    }
                                                    if (inserted)
                                                    {
                                                        extraHourUsed.CommitTransaction();
                                                        //to refresh lvExtraHours list, tbAvailableHours and tbUsingHours text box, 
                                                        //cbUsingMinutes and dtpUsingDate combo boxes
                                                        //but do not change what is checked in radio buttons
                                                        selectRegularWork = false;
                                                        cbEmployee_SelectedIndexChanged(sender, e);
                                                        selectRegularWork = true;
                                                        //to refresh lvUsedHours list
                                                        //cbEmployeeU_SelectedIndexChanged(sender, e);
                                                        if (cbEmployeeU.SelectedIndex > 0)
                                                        {
                                                            TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployeeU.SelectedValue.ToString());
                                                            if (employeeTO.EmployeeID != -1)
                                                            {
                                                                populateUsedListView();
                                                            }
                                                            else
                                                            {
                                                                clearListView(lvUsedHours);
                                                                lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
                                                            }
                                                        }
                                                        MessageBox.Show(rm.GetString("EmployeeXAccessGroupSaved", culture));
                                                    }
                                                    else
                                                    {
                                                        extraHourUsed.RollbackTransaction();
                                                        MessageBox.Show(rm.GetString("EmployeeXAccessGroupNotSaved", culture));
                                                    }

                                                } //if (trans)
                                                else
                                                {
                                                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                    return;
                                                }
                                            }
                                        }
                                        int timeAvaiable = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                                        int wT = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                                                + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                                        int usingAheadL = int.Parse(numUsingAhead.Value.ToString()) * 60;

                                        if (((!positive) && ((timeAvaiable + wT) > usingAheadL)) || ((positive) && ((Math.Abs(timeAvaiable - wT)) > usingAheadL)))
                                        {
                                            panelColor = Color.Red;
                                        }
                                        else
                                        {
                                            panelColor = Color.Green;
                                        }
                                        panelTime.BackColor = panelColor;

                                    }//if(usingAhead)
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //if (currentExtraHourUsed.GetTransaction().)
                //if (trans)
                //currentExtraHourUsed.RollbackTransaction();
                log.writeLog(DateTime.Now + " ExtraHours.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
		}        

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbWorkingUnit.SelectedIndex == 0)
				{
					if (cbEmployee.SelectedIndex == 0)
					{
						clearListView(lvExtraHours);
						tbAvailableHours.Text = "";
                        //tbUsingHours must be before cbUsingMinutes, otherwise, on text changed can be 0, 0
                        tbUsingHours.Text = "8";
                        cbUsingMinutes.SelectedIndex = 0;
						dtpUsingDate.Value = DateTime.Now;
					}
					else
						cbEmployee.SelectedIndex = 0;
				}
				else
					cbWorkingUnit.SelectedIndex = 0;

                rbExtraHours.Checked = true;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnCancel_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ExtraHours.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbWorkingUnitE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				string workingUnitID = "";
				
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWorkingUnitE.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWorkingUnitE.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (!check)
                {
                    if (cbWorkingUnitE.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnitE.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnitE.SelectedValue != -1 && chbHierarhicly.Checked)
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitE.SelectedValue)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }

                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployeeE, employeeArray);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbWorkingUnitE_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbWorkingUnitU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string workingUnitID = "";

                bool check = false;
                if (!chbHierarchyU.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWorkingUnitU.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWorkingUnitU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarchyU.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (!check)
                {
                    if (cbWorkingUnitU.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnitU.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnitU.SelectedValue != -1 && chbHierarchyU.Checked)
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitU.SelectedValue)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }

                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployeeU, employeeArray);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.cbWorkingUnitU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

                string workingUnitID = "";
				 bool check = false;
                 if (!chbHierarchyUE.Checked)
                 {
                     foreach (WorkingUnitTO wu in wUnits)
                     {
                         if (cbWorkingUnit.SelectedIndex != 0)
                         {
                             if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                             {
                                 chbHierarchyUE.Checked = true;
                                 check = true;
                             }
                         }
                     }
                 }
                if (!check)
                {
                    if (cbWorkingUnit.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnit.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnit.SelectedValue != -1 && chbHierarchyUE.Checked)
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
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }
                    
                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployee, employeeArray);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbEmployeeE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				clearListView(lvEarnedHours);
				lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);

				if (cbEmployeeU.Items.Count > 0)
					cbEmployeeU.SelectedIndex = cbEmployeeE.SelectedIndex;
				if (cbEmployee.Items.Count > 0)
					cbEmployee.SelectedIndex = cbEmployeeE.SelectedIndex;
                if (chbUsingAhead.Checked && !tbAvailableHours.Text.Equals(""))
                {
                    int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                    int wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                            + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                    int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                    char s = tbAvailableHours.Text.Trim()[0];
                    int i = -1;
                    bool positive = int.TryParse(s.ToString(), out i);

                    if (((!positive) && ((timeEmployeeCanUse + wTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                    {
                        panelColor = Color.Red;
                    }
                    else
                    {
                        panelColor = Color.Green;
                    }
                    panelTime.BackColor = panelColor;
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbEmployeeE_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbEmployeeU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				clearListView(lvUsedHours);
				lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);

				if (cbEmployeeE.Items.Count > 0)
					cbEmployeeE.SelectedIndex = cbEmployeeU.SelectedIndex;
				if (cbEmployee.Items.Count > 0)
					cbEmployee.SelectedIndex = cbEmployeeU.SelectedIndex;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbEmployeeU_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
		
				if (cbEmployee.SelectedIndex > 0)
				{
					TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployee.SelectedValue.ToString());
					if (employeeTO.EmployeeID != -1)
					{
						populateExtraListView();
                        populateEarnedHours(employeeTO.EmployeeID);
                        populateUsedAheadHours(employeeTO.EmployeeID);
					}
					else
					{
						clearListView(lvExtraHours);
                        clearEarnedAndUsedHours();
						tbAvailableHours.Text = "";
					}
				}
				else
				{
					clearListView(lvExtraHours);
					tbAvailableHours.Text = "";
				}	

                //tbUsingHours must be before cbUsingMinutes, otherwise, on text changed can be 0, 0
                tbUsingHours.Text = "8";
                cbUsingMinutes.SelectedIndex = 0;
				dtpUsingDate.Value = DateTime.Now;

                panelColor = Color.Red;
                if (panelTime.BackColor != Control.DefaultBackColor)
                    panelTime.BackColor = panelColor;
                if (chbUsingAhead.Checked && !tbAvailableHours.Text.Equals(""))
                {
                    int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                    int wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                            + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                    int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                    char s = tbAvailableHours.Text.Trim()[0];
                    int i = -1;
                    bool positive = int.TryParse(s.ToString(), out i);

                    if (((!positive) && ((timeEmployeeCanUse + wTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                    {
                        panelColor = Color.Red;
                    }
                    else
                    {
                        panelColor = Color.Green;
                    }
                    panelTime.BackColor = panelColor;
                }
                //this is false only if we are comming from btnSave_click, because, than, do not change radio buttons
                if (selectRegularWork)
                    rbExtraHours.Checked = true;

				if (cbEmployeeE.Items.Count > 0)
					cbEmployeeE.SelectedIndex = cbEmployee.SelectedIndex;
				if (cbEmployeeU.Items.Count > 0)
					cbEmployeeU.SelectedIndex = cbEmployee.SelectedIndex;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbUsingHours_TextChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!chbUsingAhead.Checked)
                {
                    if (!tbUsingHours.Text.Trim().Equals(""))
                    {
                        if (!checkNumber())
                        {
                            MessageBox.Show(rm.GetString("wantedTimeNotNum", culture));
                            tbUsingHours.Text = "8";
                        }
                        else if (Int32.Parse(tbUsingHours.Text.Trim()) == 0 && cbUsingMinutes.SelectedIndex == 0)
                        {
                            MessageBox.Show(rm.GetString("wantedTimeNotNum", culture));
                            //tbUsingHours must be before cbUsingMinutes, otherwise, on text changed can be 0, 0
                            tbUsingHours.Text = "8";
                            cbUsingMinutes.SelectedIndex = 0;
                        }
                    }

                    panelColor = Color.Red;
                    if (panelTime.BackColor != Control.DefaultBackColor)
                        panelTime.BackColor = panelColor;

                    lvExtraHours_SelectedIndexChanged(this, new EventArgs());
                }
                else
                {
                    int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                    int wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                            + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                    int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                    char s = tbAvailableHours.Text.Trim()[0];
                    int i = -1;
                    bool positive = int.TryParse(s.ToString(), out i);

                    if (((!positive) && ((timeEmployeeCanUse + wTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                    {
                        panelColor = Color.Red;
                    }
                    else
                    {
                        panelColor = Color.Green;
                    }
                    panelTime.BackColor = panelColor;
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.tbUsingHours_TextChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbUsingHours_Leave(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (tbUsingHours.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("enterWantedTime", culture));
                    tbUsingHours.Text = "8";
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.tbUsingHours_Leave(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private bool checkNumber()
		{
			bool isNumber = true;
			try
			{
				int num = Int32.Parse(tbUsingHours.Text.Trim());
				if (num < 0)
					isNumber = false;
			}
			catch
			{
				isNumber = false;
			}

			return isNumber;
		}

		private void cbUsingMinutes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!chbUsingAhead.Checked)
                {
                    if (Int32.Parse(tbUsingHours.Text.Trim()) == 0 && cbUsingMinutes.SelectedIndex == 0)
                    {
                        MessageBox.Show(rm.GetString("wantedTimeNotNum", culture));
                        //tbUsingHours must be before cbUsingMinutes, otherwise, on text changed can be 0, 0
                        tbUsingHours.Text = "8";
                        cbUsingMinutes.SelectedIndex = 0;
                    }

                    panelColor = Color.Red;
                    if (panelTime.BackColor != Control.DefaultBackColor)
                        panelTime.BackColor = panelColor;

                    lvExtraHours_SelectedIndexChanged(this, new EventArgs());
                }
                else
                {
                    int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                    int wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                            + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                    int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                    char s = tbAvailableHours.Text.Trim()[0];
                    int i = -1;
                    bool positive = int.TryParse(s.ToString(), out i);
                       
                    if (((!positive) && ((timeEmployeeCanUse + wTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                    {
                        panelColor = Color.Red;
                    }
                    else
                    {
                        panelColor = Color.Green;
                    }
                    panelTime.BackColor = panelColor;
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.cbUsingMinutes_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvExtraHours_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //tbUsingHours can not be "" because of textLeave event, but if you delete tbUsingHours
                //and with the mouse click into the list, index changed event will happened before textLeave event, 
                //and in that case tbUsingHours is "", but than, do not do anything, let textLeave event do the job
                if (!tbUsingHours.Text.Trim().Equals("") && !chbUsingAhead.Checked)
                {
                    if (lvExtraHours.SelectedItems.Count > 0)
                    {
                        int selectedTimeAmount = 0;

                        foreach (ListViewItem item in lvExtraHours.SelectedItems)
                        {
                            if ((int)item.Tag > 0)
                            {
                                selectedTimeAmount += (int)item.Tag;
                            }
                            else
                            {
                                //MessageBox.Show(rm.GetString("CanNotSelNegative", culture));
                                item.Selected = false;
                            }
                        }


                        int wantedTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                            + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                        int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                        int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                        char s = tbAvailableHours.Text.Trim()[0];
                        int i = -1;
                        bool positive = int.TryParse(s.ToString(), out i);

                        if (((selectedTimeAmount >= wantedTime) && (timeEmployeeCanUse >= wantedTime))
                            && !(((!positive) && ((timeEmployeeCanUse + wantedTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wantedTime)) < usingAheadLimit))))
                        {
                            panelColor = Color.Green;
                        }
                        else
                        {
                            panelColor = Color.Red;
                        }
                    }//something is selected
                    else
                    {
                        //nothing is selected
                        panelColor = Color.Red;
                    }
                    if (panelTime.BackColor != Control.DefaultBackColor)
                        panelTime.BackColor = panelColor;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.lvExtraHours_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                
                this.Cursor = Cursors.Arrow; 
            }
		}

		private void lvEarnedHours_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvEarnedHours.Sorting;
				lvEarnedHours.Sorting = SortOrder.None;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvEarnedHours.Sorting = SortOrder.Descending;
					}
					else
					{
						lvEarnedHours.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvEarnedHours.Sorting = SortOrder.Ascending;
                } 
                lvEarnedHours.ListViewItemSorter = _comp1;

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.lvEarnedHours_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvUsedHours_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvUsedHours.Sorting;
				lvUsedHours.Sorting = SortOrder.None;

				if (e.Column == _comp2.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvUsedHours.Sorting = SortOrder.Descending;
					}
					else
					{
						lvUsedHours.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp2.SortColumn = e.Column;
					lvUsedHours.Sorting = SortOrder.Ascending;
				}
                lvUsedHours.ListViewItemSorter = _comp2;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.lvUsedHours_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvExtraHours_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvExtraHours.Sorting;
				lvExtraHours.Sorting = SortOrder.None;

				if (e.Column == _comp3.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvExtraHours.Sorting = SortOrder.Descending;
					}
					else
					{
						lvExtraHours.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp3.SortColumn = e.Column;
					lvExtraHours.Sorting = SortOrder.Ascending;
				}
                lvExtraHours.ListViewItemSorter = _comp3;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.lvExtraHours_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSearchE_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (cbEmployeeE.SelectedIndex > 0)
				{
					TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployeeE.SelectedValue.ToString());
					if (employeeTO.EmployeeID != -1)
					{
						populateEarnedListView();
					}
					else
					{
						clearListView(lvEarnedHours);
						lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
				}				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnSearchE_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnSearchU_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (cbEmployeeU.SelectedIndex > 0)
				{
					TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployeeU.SelectedValue.ToString());
					if (employeeTO.EmployeeID != -1)
					{
						populateUsedListView();
					}
					else
					{
						clearListView(lvUsedHours);
						lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnSearchU_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void dtpFromE_ValueChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				clearListView(lvEarnedHours);
				lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.dtpFromE_ValueChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void dtpToE_ValueChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				clearListView(lvEarnedHours);
				lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.dtpToE_ValueChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void dtpFromU_ValueChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				clearListView(lvUsedHours);
				lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.dtpFromU_ValueChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void dtpToU_ValueChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				clearListView(lvUsedHours);
				lblTotalU.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.dtpToU_ValueChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCalculate_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbEmployee.SelectedIndex <= 0)
					MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
				else
				{
					ExtraHoursCalculation extraHoursCalculationForm = new ExtraHoursCalculation((int)cbEmployee.SelectedValue); 
					extraHoursCalculationForm.ShowDialog(this);

					refreshExtraHours(sender, e);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnCalculate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCalculateE_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbEmployeeE.SelectedIndex <= 0)
					MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
				else
				{
					ExtraHoursCalculation extraHoursCalculationForm = new ExtraHoursCalculation((int)cbEmployeeE.SelectedValue);
					extraHoursCalculationForm.ShowDialog(this);

					refreshExtraHours(sender, e);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnCalculateE_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}
	
		private void refreshExtraHours(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //to refresh lvExtraHours list, tbAvailableHours and tbUsingHours text box, 
                //cbUsingMinutes and dtpUsingDate combo boxes
                cbEmployee_SelectedIndexChanged(sender, e);
                //to refresh lvEarnedHours list
                if (cbEmployeeE.SelectedIndex > 0)
                {
                    TransferObjects.EmployeeTO employeeTO = new Employee().Find(cbEmployeeE.SelectedValue.ToString());
                    if (employeeTO.EmployeeID != -1)
                    {
                        populateEarnedListView();
                    }
                    else
                    {
                        clearListView(lvEarnedHours);
                        lblTotalE.Text = rm.GetString("lblTotal", culture) + Util.Misc.transformMinToStringTime(0);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.refreshExtraHours(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (lvUsedHours.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelUsedHoursDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteUsedHours", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						int selected = lvUsedHours.SelectedItems.Count;
						foreach(ListViewItem item in lvUsedHours.SelectedItems)
						{
							DateTime usedDate = new DateTime();

                            bool isRegularWork = (rm.GetString("regular", culture) == item.SubItems[5].Text);
                            bool isRegularWorkAdvanced = (rm.GetString("regularAdvanced", culture) == item.SubItems[5].Text);
							string[] dateElements = item.SubItems[0].Text.Split('.');
							if (dateElements.Length == 3)
							{
								int day = Int32.Parse(dateElements[0]);
								int month = Int32.Parse(dateElements[1]);
								int year = Int32.Parse(dateElements[2]);

								int hour = 0;
								int min = 0;
								string[] timeElements = item.SubItems[2].Text.Split(':');
								if (timeElements.Length == 2)
								{
									hour = Int32.Parse(timeElements[0]);
									min = Int32.Parse(timeElements[1]);
								}
								usedDate = new DateTime(year, month, day, hour, min, 0);
							}
							if ((usedDate != new DateTime())
                                && (usedDate.Date < DateTime.Now.Date) && isRegularWork)
							{
								MessageBox.Show(item.Text + ": " + rm.GetString("usedHoursAlreadyUsed", culture));
								selected--;
							}
                            if (isRegularWorkAdvanced)
                            {
                                MessageBox.Show( rm.GetString("usedHoursAhead", culture));
                                selected--;
                            }
							else
							{
								string recordID = item.Tag.ToString();
								int index = recordID.LastIndexOf(",");
								if (index > 0)
									recordID = recordID.Substring(index + 1).Trim();

                                isDeleted = new ExtraHourUsed().Delete(Int32.Parse(recordID), isRegularWork, usedDate) && isDeleted;
							}
						}

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("usedHoursDel", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("noUsedHoursDel", culture));
						}

						//to refresh lvUsedHours list
						populateUsedListView();	
					
						//to refresh lvExtraHours list, tbAvailableHours and tbUsingHours text box, 
						//cbUsingMinutes and dtpUsingDate combo boxes
						cbEmployee_SelectedIndexChanged(sender, e);

						this.Invalidate();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHours.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void rbExtraHours_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbExtraHours.Checked)
                {
                    dtpUsingDate.Enabled = true;
                    //if (cbTakeWholeInterval.Visible == true)
                    if (visibleIntervalCheckBox)
                    {
                        cbTakeWholeInterval.Visible = false;
                        visibleIntervalCheckBox = false;
                        cbTakeWholeInterval.Checked = false;
                        cbTakeWholeInterval.CheckState = CheckState.Unchecked;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.rbExtraHours_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbOvertime_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbOvertime.Checked)
                {
                    dtpUsingDate.Enabled = false;
                    //if (cbTakeWholeInterval.Visible == false)
                    if (!visibleIntervalCheckBox)
                    {
                        cbTakeWholeInterval.Visible = true;
                        visibleIntervalCheckBox = true;
                        cbTakeWholeInterval.Checked = true;
                        cbTakeWholeInterval.CheckState = CheckState.Checked;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.rbOvertime_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbDiscard_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbDiscard.Checked)
                {
                    dtpUsingDate.Enabled = false;
                    //if (cbTakeWholeInterval.Visible == false)
                    if (!visibleIntervalCheckBox)
                    {
                        cbTakeWholeInterval.Visible = true;
                        visibleIntervalCheckBox = true;
                        cbTakeWholeInterval.Checked = true;
                        cbTakeWholeInterval.CheckState = CheckState.Checked;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.rbDiscard_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbTakeWholeInterval_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbTakeWholeInterval.Checked)
                {
                    panelTime.BackColor = Control.DefaultBackColor;
                    tbUsingHours.Enabled = false;
                    cbUsingMinutes.Enabled = false;
                }
                else
                {
                    panelTime.BackColor = panelColor;
                    tbUsingHours.Enabled = true;
                    cbUsingMinutes.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.cbTakeWholeInterval_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ExtraHours_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExtraHours.ExtraHours_KeyUp(): " + ex.Message + "\n");
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
                    this.cbWorkingUnitE.SelectedIndex = cbWorkingUnitE.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTreeUsed_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWorkingUnitU.SelectedIndex = cbWorkingUnitU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTreeUsing_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbUsingAhead_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (chbUsingAhead.Checked)
                {
                    if (!tbAvailableHours.Text.Equals(""))
                    {
                        gbType.Enabled = false;
                        rbExtraHours.Checked = true;

                        int timeEmployeeCanUse = Util.Misc.transformStringTimeToMin(tbAvailableHours.Text);
                        int wTime = Int32.Parse(tbUsingHours.Text.Trim()) * 60
                                + Int32.Parse(cbUsingMinutes.SelectedItem.ToString());
                        int usingAheadLimit = int.Parse(numUsingAhead.Value.ToString()) * 60;
                        char s = tbAvailableHours.Text.Trim()[0];
                        int i = -1;
                        bool positive = int.TryParse(s.ToString(), out i);

                        if (((!positive) && ((timeEmployeeCanUse + wTime) > usingAheadLimit)) || ((positive) && ((Math.Abs(timeEmployeeCanUse - wTime)) < usingAheadLimit)))
                        {
                            panelColor = Color.Red;
                        }
                        else
                        {
                            panelColor = Color.Green;
                        }
                    }
                }
                else
                {
                    gbType.Enabled = true;
                    panelColor = Color.Red;
                    foreach (ListViewItem item in lvExtraHours.SelectedItems)
                    {
                        item.Selected = false;
                    }
                }
                panelTime.BackColor = panelColor;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.chbUsingAhead_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                chbHierarchyU.Checked = chbHierarhicly.Checked;
                chbHierarchyUE.Checked = chbHierarhicly.Checked;
                string workingUnitID = "";

                if (cbWorkingUnitE.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnitE.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnitE.SelectedValue != -1 && chbHierarhicly.Checked)
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitE.SelectedValue)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }


                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployeeE, employeeArray);
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarchyU_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                chbHierarhicly.Checked = chbHierarchyU.Checked;
                chbHierarchyUE.Checked = chbHierarchyU.Checked;

                string workingUnitID = "";

               
                    if (cbWorkingUnitU.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnitU.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnitU.SelectedValue != -1 && chbHierarchyU.Checked)
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitU.SelectedValue)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }

                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployeeU, employeeArray);
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.cbWorkingUnitU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarchyUE_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                chbHierarchyU.Checked = chbHierarchyUE.Checked;
                chbHierarhicly.Checked = chbHierarchyUE.Checked;

                string workingUnitID = "";
               
                    if (cbWorkingUnit.SelectedIndex > 0)
                    {
                        workingUnitID = cbWorkingUnit.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWorkingUnit.SelectedValue != -1 && chbHierarchyUE.Checked)
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
                                wuList = wu.FindAllChildren(wuList);
                            workingUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workingUnitID.Length > 0)
                            {
                                workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                            }
                        }
                    }


                    List<EmployeeTO> employeeArray;
                    if (workingUnitID.Equals(""))
                    {
                        employeeArray = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        employeeArray = new Employee().SearchWithStatuses(statuses, workingUnitID);
                    }

                    foreach (EmployeeTO employee in employeeArray)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }
                    employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

                    populateEmployeeCombo(cbEmployee, employeeArray);
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ExtraHours.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter, this.tabControl1.SelectedTab);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (filter != null)
                {
                    if (tabControl1.SelectedIndex == 0)
                        filter.SerachButton = btnSearchE;
                    if (tabControl1.SelectedIndex == 1)
                        filter.SerachButton = btnSearchU;
                    filter.TabID = this.tabControl1.SelectedTab.Text;
                    filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnEarnedReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CultureInfo ci = CultureInfo.InvariantCulture;

                if (lvEarnedHours.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("extra_hours");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("wu", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
                    tableCR.Columns.Add("time", typeof(System.String));
                    tableCR.Columns.Add("date_sort", typeof(System.DateTime));
                    tableCR.Columns.Add("earnedMin", typeof(System.Int32));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (ListViewItem item in lvEarnedHours.Items)
                    {
                        DataRow row = tableCR.NewRow();

                        row["wu"] = "";
                        row["employee"] = "";
                        row["date"] = item.Text.ToString().Trim();
                        string time = item.SubItems[1].Text.ToString().Trim();
                        row["time"] = time;
                        row["earnedMin"] = Util.Misc.transformStringTimeToMin(time);
                        row["date_sort"] = DateTime.ParseExact(item.Text.ToString().Trim(), "dd.MM.yyyy", ci);

                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmployee = "*";


                    if (cbWorkingUnitE.SelectedIndex >= 0 && (int)cbWorkingUnitE.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnitE.Text;
                    if (cbEmployeeE.SelectedIndex >= 0 && (int)cbEmployeeE.SelectedValue >= 0)
                        selEmployee = cbEmployeeE.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.ExtraHoursCRView_sr view = new Reports.Reports_sr.ExtraHoursCRView_sr(dataSetCR,
                             selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromE.Value, dtpToE.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.ExtraHoursCRView_en view = new Reports.Reports_en.ExtraHoursCRView_en(dataSetCR,
                              selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromE.Value, dtpToE.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.ExtraHoursCRView_fi view = new Reports.Reports_fi.ExtraHoursCRView_fi(dataSetCR,
                             selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromE.Value, dtpToE.Value);
                        view.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours. btnEarnedReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUsingReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CultureInfo ci = CultureInfo.InvariantCulture;

                if (lvExtraHours.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("extra_hours");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("wu", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
                    tableCR.Columns.Add("time", typeof(System.String));
                    tableCR.Columns.Add("date_sort", typeof(System.DateTime));
                    tableCR.Columns.Add("earnedMin", typeof(System.Int32));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (ListViewItem item in lvExtraHours.Items)
                    {
                        DataRow row = tableCR.NewRow();

                        row["wu"] = "";
                        row["employee"] = "";
                        row["date"] = item.Text.ToString().Trim();
                        string time = item.SubItems[1].Text.ToString().Trim();
                        row["time"] = time;
                        row["earnedMin"] = Util.Misc.transformStringTimeToMin(time);
                        row["date_sort"] = DateTime.ParseExact(item.Text.ToString().Trim(), "dd.MM.yyyy", ci);

                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmployee = "*";


                    if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnit.Text;
                    if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                        selEmployee = cbEmployee.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.ExtraHoursCRView_sr view = new Reports.Reports_sr.ExtraHoursCRView_sr(dataSetCR,
                             selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.ExtraHoursCRView_en view = new Reports.Reports_en.ExtraHoursCRView_en(dataSetCR,
                              selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.ExtraHoursCRView_fi view = new Reports.Reports_fi.ExtraHoursCRView_fi(dataSetCR,
                             selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                        view.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
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
                log.writeLog(DateTime.Now + " ExtraHours. btnEarnedReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReportUsed_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CultureInfo ci = CultureInfo.InvariantCulture;

                if (lvExtraHours.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("extra_hours");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("wu", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("usedDate", typeof(System.String));
                    tableCR.Columns.Add("amount", typeof(System.String));
                    tableCR.Columns.Add("startTime", typeof(System.String));
                    tableCR.Columns.Add("endTime", typeof(System.String));
                    tableCR.Columns.Add("earnedDate", typeof(System.String));
                    tableCR.Columns.Add("type", typeof(System.String));
                    tableCR.Columns.Add("createdBy", typeof(System.String));
                    tableCR.Columns.Add("usedDateSort", typeof(System.DateTime));
                    tableCR.Columns.Add("Min", typeof(System.Int16));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (ListViewItem item in lvUsedHours.Items)
                    {
                        DataRow row = tableCR.NewRow();
                        row["wu"] = "";
                        row["employee"] = "";                        
                        string time = item.SubItems[0].Text.ToString().Trim();
                        row["usedDate"] = time;
                        row["usedDateSort"] = DateTime.ParseExact(time, "dd.MM.yyyy", ci); 
                        row["amount"] = item.SubItems[1].Text.ToString().Trim();
                        row["Min"] = Util.Misc.transformStringTimeToMin(item.SubItems[1].Text.ToString().Trim());
                        row["startTime"] = item.SubItems[2].Text.ToString().Trim();
                        row["endTime"] = item.SubItems[3].Text.ToString().Trim();
                        row["earnedDate"] = item.SubItems[4].Text.ToString().Trim();
                        row["type"] = item.SubItems[5].Text.ToString().Trim();
                        row["createdBy"] = item.SubItems[6].Text.ToString().Trim();
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmployee = "*";
                    string selType = "*";


                    if (cbWorkingUnitU.SelectedIndex >= 0 && (int)cbWorkingUnitU.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnitU.Text;
                    if (cbEmployeeU.SelectedIndex >= 0 && (int)cbEmployeeU.SelectedValue >= 0)
                        selEmployee = cbEmployeeU.Text;
                    if (cbType.SelectedIndex >= 0 && !cbType.SelectedValue.ToString().Equals("*"))
                        selType = cbType.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.ExtraHoursUsedCRView_sr view = new Reports.Reports_sr.ExtraHoursUsedCRView_sr(dataSetCR,
                             selWorkingUnit, selEmployee,selType, dtpFromU.Value,dtpToU.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.ExtraHoursUsedCRView_en view = new Reports.Reports_en.ExtraHoursUsedCRView_en(dataSetCR,
                             selWorkingUnit, selEmployee, selType, dtpFromU.Value, dtpToU.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.ExtraHoursUsedCRView_fi view = new Reports.Reports_fi.ExtraHoursUsedCRView_fi(dataSetCR,
                             selWorkingUnit, selEmployee, selType, dtpFromU.Value, dtpToU.Value);
                        view.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
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
                log.writeLog(DateTime.Now + " ExtraHours. btnEarnedReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbUsingMinutes_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //e.Handled = true;
                if (!Char.IsNumber(e.KeyChar) && (e.KeyChar != '\b'))
                { e.Handled = true; }
                //{
                //    e.Handled = true;
                //    if (cbUsingMinutes.Text.Length == 1)
                //    {
                //        if (cbUsingMinutes.Text.ToString() == "0")
                //            cbUsingMinutes.SelectedItem = e.KeyChar.ToString();
                //        else if (int.Parse(cbUsingMinutes.Text.ToString() + e.KeyChar) <= 59 && int.Parse(cbUsingMinutes.Text.ToString() + e.KeyChar) >= 0)
                //            cbUsingMinutes.SelectedItem = cbUsingMinutes.Text.ToString() + e.KeyChar.ToString();
                //        else
                //        {
                //            cbUsingMinutes.SelectedItem = e.KeyChar.ToString();
                //        }
                //    }
                //    else
                //    {
                //        cbUsingMinutes.SelectedItem = e.KeyChar.ToString();
                //    }
                //}
                //else if (e.KeyChar == '\b')
                //{
                //    if (cbUsingMinutes.Text.ToString().Length == 2)
                //        cbUsingMinutes.SelectedItem = cbUsingMinutes.Text.Substring(0, 1);
                //    else
                //    {
                //        cbUsingMinutes.SelectedItem = "0";
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " CardBlocking.tbClient_KeyPress(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbUsingMinutes_Leave(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    cbUsingMinutes.SelectedItem = cbUsingMinutes.Text.ToString();
                    if(cbUsingMinutes.SelectedItem== null)
                        cbUsingMinutes.SelectedItem = "0";
                }
                catch
                {
                    cbUsingMinutes.SelectedItem = "0";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " CardBlocking.tbClient_KeyPress(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        
       
	}
}
