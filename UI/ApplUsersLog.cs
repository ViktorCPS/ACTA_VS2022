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
using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ApplUsersLog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbOperaterLog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblUsersLogTo;
        private System.Windows.Forms.Label lblUsersLogFrom;
        private System.Windows.Forms.ComboBox cmbUserID;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvUsersLog;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnSearch;
        private CheckBox cbRetired;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        private ComboBox cbType;
        private Label lblType;
        private ComboBox cbStatus;
        private Label lblStatus;
        private ComboBox cbChanel;
        private Label lblChanel;
        private Button btnNext;
        private Button btnPrev;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO userTO = new ApplUserTO();		
        		
		private CultureInfo culture;
		protected string dateTimeformat = "";
		
		ResourceManager rm;
		
		DebugLog log;

		// List View indexes
		const int UserIDIndex = 0;
		const int NameIndex = 1;
		const int HostIndex = 2;
		const int LoginTimeIndex = 3;
		const int LogoutTimeIndex = 4;
        const int DurationIndex = 5;                
        const int LoginStatus = 6;
        const int LoginChanel = 7;
        const int LoginType = 8;
        const int Changes = 9;

        private int sortOrder = 0;
        private int sortField = 0;
        private int startIndex = 0;
        
        private Filter filter;

        bool isFiatLicence = false;
        private ComboBox cbChanges;
        private Label lblChanges;

        Dictionary<string, ApplUserTO> userDict = new Dictionary<string, ApplUserTO>();
        Dictionary<string, int> changeIndexes = new Dictionary<string, int>();
        List<string> changeTableNames = new List<string>();
        List<ApplUserLogTO> currentLogList = new List<ApplUserLogTO>();
        private Button btnReport;
        private Label lblTotal;
        private Label lblNum;
        
        string userIDs = "";

		public ApplUsersLog(ApplUserTO userTO)
		{
			//
			// Required for Windows Form Designer support
			//
			
			InitializeComponent();
			
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
            
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(ApplUsersLog).Assembly);

            //check if it is Fiat licence
            string costumer = Common.Misc.getCustomer(null);
            int cost = 0;
            bool costum = int.TryParse(costumer, out cost);
            isFiatLicence = (cost == (int)Constants.Customers.FIAT);

            if (!isFiatLicence)
                lblType.Visible = cbType.Visible = false;
			
            setLanguage();

			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;

			dtpTo.Value = DateTime.Now.Date;
			dtpFrom.Value = DateTime.Now.Date.AddDays(-7);

            btnPrev.Visible = false;
            btnNext.Visible = false;

            cbRetired.Checked = false;

            this.userTO = userTO;
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
            this.gbOperaterLog = new System.Windows.Forms.GroupBox();
            this.cbChanges = new System.Windows.Forms.ComboBox();
            this.lblChanges = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbChanel = new System.Windows.Forms.ComboBox();
            this.lblChanel = new System.Windows.Forms.Label();
            this.cbRetired = new System.Windows.Forms.CheckBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblUsersLogTo = new System.Windows.Forms.Label();
            this.lblUsersLogFrom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbUserID = new System.Windows.Forms.ComboBox();
            this.lvUsersLog = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.gbOperaterLog.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOperaterLog
            // 
            this.gbOperaterLog.Controls.Add(this.cbChanges);
            this.gbOperaterLog.Controls.Add(this.lblChanges);
            this.gbOperaterLog.Controls.Add(this.cbType);
            this.gbOperaterLog.Controls.Add(this.lblType);
            this.gbOperaterLog.Controls.Add(this.cbStatus);
            this.gbOperaterLog.Controls.Add(this.lblStatus);
            this.gbOperaterLog.Controls.Add(this.cbChanel);
            this.gbOperaterLog.Controls.Add(this.lblChanel);
            this.gbOperaterLog.Controls.Add(this.cbRetired);
            this.gbOperaterLog.Controls.Add(this.dtpTo);
            this.gbOperaterLog.Controls.Add(this.dtpFrom);
            this.gbOperaterLog.Controls.Add(this.lblUsersLogTo);
            this.gbOperaterLog.Controls.Add(this.lblUsersLogFrom);
            this.gbOperaterLog.Controls.Add(this.label2);
            this.gbOperaterLog.Controls.Add(this.lblUser);
            this.gbOperaterLog.Controls.Add(this.btnSearch);
            this.gbOperaterLog.Controls.Add(this.cmbUserID);
            this.gbOperaterLog.Location = new System.Drawing.Point(12, 4);
            this.gbOperaterLog.Name = "gbOperaterLog";
            this.gbOperaterLog.Size = new System.Drawing.Size(502, 169);
            this.gbOperaterLog.TabIndex = 0;
            this.gbOperaterLog.TabStop = false;
            this.gbOperaterLog.Tag = "FILTERABLE";
            this.gbOperaterLog.Text = "Operators";
            // 
            // cbChanges
            // 
            this.cbChanges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChanges.Location = new System.Drawing.Point(112, 105);
            this.cbChanges.Name = "cbChanges";
            this.cbChanges.Size = new System.Drawing.Size(126, 21);
            this.cbChanges.TabIndex = 9;
            // 
            // lblChanges
            // 
            this.lblChanges.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblChanges.Location = new System.Drawing.Point(41, 103);
            this.lblChanges.Name = "lblChanges";
            this.lblChanges.Size = new System.Drawing.Size(65, 23);
            this.lblChanges.TabIndex = 8;
            this.lblChanges.Text = "Changes:";
            this.lblChanges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Location = new System.Drawing.Point(358, 22);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(126, 21);
            this.cbType.TabIndex = 3;
            // 
            // lblType
            // 
            this.lblType.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblType.Location = new System.Drawing.Point(313, 20);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(39, 23);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Location = new System.Drawing.Point(112, 76);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(126, 21);
            this.cbStatus.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatus.Location = new System.Drawing.Point(41, 74);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(65, 23);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbChanel
            // 
            this.cbChanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChanel.Location = new System.Drawing.Point(112, 49);
            this.cbChanel.Name = "cbChanel";
            this.cbChanel.Size = new System.Drawing.Size(126, 21);
            this.cbChanel.TabIndex = 5;
            // 
            // lblChanel
            // 
            this.lblChanel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblChanel.Location = new System.Drawing.Point(41, 46);
            this.lblChanel.Name = "lblChanel";
            this.lblChanel.Size = new System.Drawing.Size(65, 23);
            this.lblChanel.TabIndex = 4;
            this.lblChanel.Text = "Chanel:";
            this.lblChanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRetired
            // 
            this.cbRetired.AutoSize = true;
            this.cbRetired.Location = new System.Drawing.Point(112, 132);
            this.cbRetired.Name = "cbRetired";
            this.cbRetired.Size = new System.Drawing.Size(140, 17);
            this.cbRetired.TabIndex = 10;
            this.cbRetired.Text = "Include RETIRED users";
            this.cbRetired.UseVisualStyleBackColor = true;
            this.cbRetired.CheckedChanged += new System.EventHandler(this.cbRetired_CheckedChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(375, 106);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(109, 20);
            this.dtpTo.TabIndex = 14;
            this.dtpTo.Value = new System.DateTime(2007, 3, 27, 0, 0, 0, 0);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(375, 80);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(109, 20);
            this.dtpFrom.TabIndex = 12;
            this.dtpFrom.Value = new System.DateTime(2007, 3, 27, 0, 0, 0, 0);
            // 
            // lblUsersLogTo
            // 
            this.lblUsersLogTo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUsersLogTo.Location = new System.Drawing.Point(318, 102);
            this.lblUsersLogTo.Name = "lblUsersLogTo";
            this.lblUsersLogTo.Size = new System.Drawing.Size(51, 23);
            this.lblUsersLogTo.TabIndex = 13;
            this.lblUsersLogTo.Text = "To:";
            this.lblUsersLogTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsersLogFrom
            // 
            this.lblUsersLogFrom.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUsersLogFrom.Location = new System.Drawing.Point(318, 77);
            this.lblUsersLogFrom.Name = "lblUsersLogFrom";
            this.lblUsersLogFrom.Size = new System.Drawing.Size(51, 23);
            this.lblUsersLogFrom.TabIndex = 11;
            this.lblUsersLogFrom.Text = "From:";
            this.lblUsersLogFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(86, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 0);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // lblUser
            // 
            this.lblUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUser.Location = new System.Drawing.Point(6, 19);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(100, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "User ID:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(409, 132);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbUserID
            // 
            this.cmbUserID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUserID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUserID.Location = new System.Drawing.Point(112, 22);
            this.cmbUserID.Name = "cmbUserID";
            this.cmbUserID.Size = new System.Drawing.Size(195, 21);
            this.cmbUserID.TabIndex = 1;
            // 
            // lvUsersLog
            // 
            this.lvUsersLog.FullRowSelect = true;
            this.lvUsersLog.GridLines = true;
            this.lvUsersLog.HideSelection = false;
            this.lvUsersLog.LabelWrap = false;
            this.lvUsersLog.Location = new System.Drawing.Point(12, 179);
            this.lvUsersLog.Name = "lvUsersLog";
            this.lvUsersLog.Size = new System.Drawing.Size(786, 288);
            this.lvUsersLog.TabIndex = 4;
            this.lvUsersLog.UseCompatibleStateImageBehavior = false;
            this.lvUsersLog.View = System.Windows.Forms.View.Details;
            this.lvUsersLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsersLog_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(723, 505);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(520, 4);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(162, 100);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(42, 57);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 1;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 22);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(146, 21);
            this.cbFilter.TabIndex = 0;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(766, 150);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(723, 150);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(12, 505);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(126, 23);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotal.Location = new System.Drawing.Point(598, 470);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(100, 23);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNum
            // 
            this.lblNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNum.Location = new System.Drawing.Point(704, 470);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(94, 23);
            this.lblNum.TabIndex = 6;
            this.lblNum.Text = "0";
            this.lblNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ApplUsersLog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(810, 542);
            this.ControlBox = false;
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvUsersLog);
            this.Controls.Add(this.gbOperaterLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "ApplUsersLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log View";
            this.Load += new System.EventHandler(this.ApplUsersLog_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplUsersLog_KeyUp);
            this.gbOperaterLog.ResumeLayout(false);
            this.gbOperaterLog.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List 
		*/
        private class ArrayListSort : IComparer<ApplUserLogTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(ApplUserLogTO x, ApplUserLogTO y)
            {
                ApplUserLogTO log1 = null;
                ApplUserLogTO log2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    log1 = x;
                    log2 = y;
                }
                else
                {
                    log1 = y;
                    log2 = x;
                }                

                switch (compField)
                {
                    case ApplUsersLog.UserIDIndex:
                        return log1.UserID.CompareTo(log2.UserID);
                    case ApplUsersLog.NameIndex:
                        return log1.UserName.CompareTo(log2.UserName);
                    case ApplUsersLog.HostIndex:
                        return log1.Host.CompareTo(log2.Host);
                    case ApplUsersLog.LoginTimeIndex:
                        return log1.LogInTime.CompareTo(log2.LogInTime);
                    case ApplUsersLog.LogoutTimeIndex:
                        return log1.LogOutTime.CompareTo(log2.LogOutTime);
                    case ApplUsersLog.DurationIndex:
                        return log1.Duration.CompareTo(log2.Duration);
                    case ApplUsersLog.LoginChanel:
                        return log1.LoginChanel.CompareTo(log2.LoginChanel);
                    case ApplUsersLog.LoginStatus:
                        return log1.LoginStatus.CompareTo(log2.LoginStatus);
                    case ApplUsersLog.LoginType:
                        return log1.LoginType.CompareTo(log2.LoginType);
                    case ApplUsersLog.Changes:
                        return log1.LoginChange.CompareTo(log2.LoginChange);
                    default:
                        return log1.UserName.CompareTo(log2.UserName);
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
				this.Text = rm.GetString("formUsersLog", culture);
				
				// group box text
				gbOperaterLog.Text = rm.GetString("gbOperaterLog", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

				// button's text
				btnClose.Text = rm.GetString("btnClose", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // checkbox text
                cbRetired.Text = rm.GetString("cbRetired", culture);

				// label's text
				lblUser.Text = rm.GetString("lblUserID", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblChanel.Text = rm.GetString("lblChanel", culture);
                lblType.Text = rm.GetString("lblType", culture);
				lblUsersLogFrom.Text = rm.GetString("lblUsersLogFrom", culture);
				lblUsersLogTo.Text = rm.GetString("lblUsersLogTo", culture);
                lblChanges.Text = rm.GetString("lblChanges", culture);
                lblTotal.Text = rm.GetString("lblTotal", culture);
                				
				// list view
				lvUsersLog.BeginUpdate();
				lvUsersLog.Columns.Add(rm.GetString("lblUserID_lv", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("lblName_lv", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("lblHost", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("lblLoginTime", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("lblLogoutTime", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("lblDurationLogin", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("hdrStatus", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("hdrChanel", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("hdrType", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
                lvUsersLog.Columns.Add(rm.GetString("hdrChanges", culture), (lvUsersLog.Width / 10) - 2, HorizontalAlignment.Left);
				lvUsersLog.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersLog.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateUserIDCombo(bool includeRetired)
		{
			try
			{
                List<ApplUserTO> userArray = new List<ApplUserTO>();

                foreach (string userID in userDict.Keys)
                {
                    if (userDict[userID].Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) ||
                        userDict[userID].Status.Trim().ToUpper().Equals(Constants.statusDisabled.Trim().ToUpper()) || includeRetired)
                    {
                        userArray.Add(userDict[userID]);
                        userIDs += "'" + userID.Trim() + "',";
                    }
                }

                if (userIDs.Length > 0)
                    userIDs = userIDs.Substring(0, userIDs.Length - 1);

                userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", rm.GetString("all", culture), "", -1, "", -1, ""));

				this.cmbUserID.DataSource = userArray;
				this.cmbUserID.DisplayMember = "Name";
				this.cmbUserID.ValueMember = "UserID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersLog.populateUserIDCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateTypes()
        {
            try
            {
                List<string> typeList = new List<string>();

                typeList.Add(rm.GetString("all", culture));

                if (isFiatLicence)
                    typeList.Add(Constants.UserLoginType.FIAT.ToString());

                typeList.Add(Constants.UserLoginType.TM.ToString());                

                this.cbType.DataSource = typeList;

                if (!isFiatLicence)
                    cbType.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.populateTypes(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateStatuses()
        {
            try
            {
                List<string> statusList = new List<string>();

                statusList.Add(rm.GetString("all", culture));
                statusList.Add(Constants.UserLoginStatus.FAILED.ToString());
                statusList.Add(Constants.UserLoginStatus.SUCCESSFUL.ToString());

                this.cbStatus.DataSource = statusList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.populateStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateChanels()
        {
            try
            {
                List<string> chanelList = new List<string>();

                chanelList.Add(rm.GetString("all", culture));
                chanelList.Add(Constants.UserLoginChanel.DESKTOP.ToString());
                chanelList.Add(Constants.UserLoginChanel.WEB.ToString());

                this.cbChanel.DataSource = chanelList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.populateChanels(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateChanges()
        {
            try
            {
                List<string> chList = new List<string>();

                chList.Add(rm.GetString("all", culture));
                chList.Add(rm.GetString("yes", culture));
                chList.Add(rm.GetString("no", culture));

                changeIndexes.Add(rm.GetString("yes", culture), Constants.yesInt);
                changeIndexes.Add(rm.GetString("no", culture), Constants.noInt);

                this.cbChanges.DataSource = chList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.populateTypes(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string userID = "";
                if (cmbUserID.SelectedIndex > 0)
                {
                    userID = "'" + cmbUserID.SelectedValue.ToString().Trim() + "'";
                }
                else
                    userID = userIDs;

                ApplUserLog log = new ApplUserLog();

                if (cbChanel.SelectedIndex > 0)
                    log.UserLogTO.LoginChanel = cbChanel.Text;

                if (cbType.SelectedIndex > 0)
                    log.UserLogTO.LoginType = cbType.Text;

                if (cbStatus.SelectedIndex > 0)
                    log.UserLogTO.LoginStatus = cbStatus.Text;

                if (cbChanges.SelectedIndex > 0 && changeIndexes.ContainsKey(cbChanges.Text))
                    log.UserLogTO.LoginChange = changeIndexes[cbChanges.Text];

                currentLogList = log.Search(userID, dtpFrom.Value.Date, dtpTo.Value.Date, changeTableNames, userDict);
                currentLogList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;                
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void populateListView()
		{
            try
            {
                if (currentLogList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvUsersLog.BeginUpdate();
                lvUsersLog.Items.Clear();

                if ((startIndex >= 0) && (startIndex < currentLogList.Count))
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
                    if (lastIndex >= currentLogList.Count)
                    {
                        btnNext.Enabled = false;
                        lastIndex = currentLogList.Count;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }

                    for (int i = startIndex; i < lastIndex; i++)
                    {
                        ApplUserLogTO log = currentLogList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = log.UserID.Trim();

                        if (!log.UserName.Trim().Equals(""))
                            item.SubItems.Add(userDict[log.UserID].Name.Trim());
                        else
                            item.SubItems.Add("N/A");

                        item.SubItems.Add(log.Host.Trim());
                        item.SubItems.Add(log.LogInTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));

                        if (!log.LogOutTime.Equals(new DateTime()))
                        {
                            item.SubItems.Add(log.LogOutTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                            item.SubItems.Add(((int)log.Duration.TotalHours).ToString().PadLeft(2, '0') + ":" + log.Duration.Minutes.ToString().PadLeft(2, '0')
                                + ":" + log.Duration.Seconds.ToString().PadLeft(2, '0'));
                        }
                        else
                        {
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                        }

                        item.SubItems.Add(log.LoginStatus.Trim());
                        item.SubItems.Add(log.LoginChanel.Trim());
                        item.SubItems.Add(log.LoginType.Trim());

                        if (log.LoginChange == Constants.yesInt)
                            item.SubItems.Add(rm.GetString("yes", culture));
                        else if (log.LoginChange == Constants.noInt)
                            item.SubItems.Add(rm.GetString("no", culture));
                        else
                            item.SubItems.Add("N/A");

                        item.Tag = log;

                        lvUsersLog.Items.Add(item);
                    }
                }

                lblNum.Text = currentLogList.Count.ToString().Trim();
                lblNum.Invalidate();

                lvUsersLog.EndUpdate();
                lvUsersLog.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
		}		
		
		private void lvUsersLog_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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

                currentLogList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.lvUsersLog_ColumnClick(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ApplUsersLog.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void cbRetired_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateUserIDCombo(cbRetired.Checked);
                lvUsersLog.Items.Clear();
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

        private void ApplUsersLog_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplUsersLog.ApplUsersLog_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ApplUsersLog_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                sortOrder = Constants.sortAsc;
                sortField = ApplUsersLog.NameIndex;
                startIndex = 0;

                userDict = new ApplUser().SearchDictionary();
                changeTableNames = new Common.ApplUsersLoginChangesTbl().SearchTableNames();

                populateUserIDCombo(userTO.Status.Equals(Constants.statusRetired));
                populateTypes();
                populateStatuses();
                populateChanels();
                populateChanges();

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.TabID = this.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

                if (!userTO.UserID.Trim().Equals(""))
                {
                    if (userTO.Status.Equals(Constants.statusRetired))                    
                        cbRetired.Checked = true;                    

                    cmbUserID.SelectedValue = userTO.UserID;
                    btnSearch.PerformClick();                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.ApplUsersLog_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ApplUsersLog.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter, new TabPage(this.Text));
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.btnPrev_Click(): " + ex.Message + "\n");
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
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (currentLogList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
                
                // insert header                
                ws.Cells[1, 1] = rm.GetString("lblUserID_lv", culture);
                ws.Cells[1, 2] = rm.GetString("lblName_lv", culture);
                ws.Cells[1, 3] = rm.GetString("lblHost", culture);
                ws.Cells[1, 4] = rm.GetString("lblLoginTime", culture);
                ws.Cells[1, 5] = rm.GetString("lblLogoutTime", culture);
                ws.Cells[1, 6] = rm.GetString("lblDurationLogin", culture);
                ws.Cells[1, 7] = rm.GetString("hdrStatus", culture);
                ws.Cells[1, 8] = rm.GetString("hdrChanel", culture);
                ws.Cells[1, 9] = rm.GetString("hdrType", culture);
                ws.Cells[1, 10] = rm.GetString("hdrChanges", culture);

                int colNum = 10;
                
                setRowFontWeight(ws, 1, colNum, true);

                int i = 2;
                foreach (ApplUserLogTO log in currentLogList)
                {
                    // insert log data
                    ws.Cells[i, 1] = log.UserID.Trim();

                    if (!log.UserName.Trim().Equals(""))
                        ws.Cells[i, 2] = userDict[log.UserID].Name.Trim();
                    else
                        ws.Cells[i, 2] = "N/A";

                    ws.Cells[i, 3] = log.Host.Trim();
                    ws.Cells[i, 4] = log.LogInTime.ToString(Constants.dateFormat + " " + Constants.timeFormat);

                    if (!log.LogOutTime.Equals(new DateTime()))
                    {
                        ws.Cells[i, 5] = log.LogOutTime.ToString(Constants.dateFormat + " " + Constants.timeFormat);
                        ws.Cells[i, 6] = ((int)log.Duration.TotalHours).ToString().PadLeft(2, '0') + ":" + log.Duration.Minutes.ToString().PadLeft(2, '0')
                            + ":" + log.Duration.Seconds.ToString().PadLeft(2, '0');
                    }
                    else
                    {
                        ws.Cells[i, 5] = "";
                        ws.Cells[i, 6] = "";
                    }

                    ws.Cells[i, 7] = log.LoginStatus.Trim();
                    ws.Cells[i, 8] = log.LoginChanel.Trim();
                    ws.Cells[i, 9] = log.LoginType.Trim();

                    if (log.LoginChange == Constants.yesInt)
                        ws.Cells[i, 10] = rm.GetString("yes", culture);
                    else if (log.LoginChange == Constants.noInt)
                        ws.Cells[i, 10] = rm.GetString("no", culture);
                    else
                        ws.Cells[i, 10] = "N/A";

                    i++;
                }
                
                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "ApplUserLogReport_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);                
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLog.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, int colNum, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ApplUsersLog.setRowFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ApplUsersLog.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
	}	
}		