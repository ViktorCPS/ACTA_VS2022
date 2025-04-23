using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using System.IO;

using Common;
using TransferObjects;
using Util;

namespace Reports
{
	/// <summary>
	/// Summary description for EmpoyeeAnaliticReport.
	/// </summary>
	public class EmpoyeeAnaliticReport : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.ListView lvEmployees;
		private System.Windows.Forms.CheckBox chbCSV;
		private System.Windows.Forms.CheckBox chbPdf;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// List View indexes
		const int FirstNameIndex = 0;
		const int LastNameIndex = 1;
		const int WorkingUnitIDIndex = 2;

		List<EmployeeTO> currentEmployeesList;
		private int sortOrder;
		private int sortField;
		private int startIndex;

		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.DateTimePicker dtpFromDate;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		private System.Windows.Forms.GroupBox gbEmpolyee;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblDocFormat;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblLastName;

		DebugLog log;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
        private CheckBox chbShowRetired;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;

        Filter filter;

		public EmpoyeeAnaliticReport()
		{	
			InitializeComponent();

			this.CenterToScreen();
            
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();
			currentEmployeesList = new List<EmployeeTO>();

			wUnits = new List<WorkingUnitTO>();

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(EmployeesReports).Assembly);

			DateTime date = DateTime.Now;
			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;

			setLanguage();
        }
        #region MDI child method's       
        public void MDIchangeSelectedEmployee(int employeeID, DateTime from, DateTime to)
        {
            try
            {
                dtpFromDate.Value = from;
                dtpToDate.Value = to;
                if (employeeID > -1)
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if ((int)item.Tag == employeeID)
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAnaliticReport.changeSelectedEmployee(): " + ex.Message + "\n");
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
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmpoyeeAnaliticReport));
            this.gbEmpolyee = new System.Windows.Forms.GroupBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.chbCSV = new System.Windows.Forms.CheckBox();
            this.chbPdf = new System.Windows.Forms.CheckBox();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbEmpolyee.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmpolyee
            // 
            this.gbEmpolyee.Controls.Add(this.lblLastName);
            this.gbEmpolyee.Controls.Add(this.tbLastName);
            this.gbEmpolyee.Location = new System.Drawing.Point(8, 6);
            this.gbEmpolyee.Name = "gbEmpolyee";
            this.gbEmpolyee.Size = new System.Drawing.Size(264, 91);
            this.gbEmpolyee.TabIndex = 0;
            this.gbEmpolyee.TabStop = false;
            this.gbEmpolyee.Text = "Employee";
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(16, 35);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(72, 23);
            this.lblLastName.TabIndex = 1;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(96, 37);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(120, 20);
            this.tbLastName.TabIndex = 2;
            this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Location = new System.Drawing.Point(8, 369);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(456, 100);
            this.gbTimeInterval.TabIndex = 9;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(96, 56);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(32, 56);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(56, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(32, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(56, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(96, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFromDate.TabIndex = 11;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(8, 137);
            this.lvEmployees.MultiSelect = false;
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(456, 216);
            this.lvEmployees.TabIndex = 4;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // chbCSV
            // 
            this.chbCSV.Enabled = false;
            this.chbCSV.Location = new System.Drawing.Point(232, 481);
            this.chbCSV.Name = "chbCSV";
            this.chbCSV.Size = new System.Drawing.Size(56, 24);
            this.chbCSV.TabIndex = 16;
            this.chbCSV.Text = "CSV";
            // 
            // chbPdf
            // 
            this.chbPdf.Enabled = false;
            this.chbPdf.Location = new System.Drawing.Point(152, 481);
            this.chbPdf.Name = "chbPdf";
            this.chbPdf.Size = new System.Drawing.Size(56, 24);
            this.chbPdf.TabIndex = 15;
            this.chbPdf.Text = "PDF";
            this.chbPdf.Visible = false;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(8, 481);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(100, 23);
            this.lblDocFormat.TabIndex = 14;
            this.lblDocFormat.Text = "Document format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(389, 521);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(8, 521);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 19;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(432, 105);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 22;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(392, 105);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(312, 481);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(56, 24);
            this.cbTXT.TabIndex = 17;
            this.cbTXT.Text = "TXT";
            this.cbTXT.CheckedChanged += new System.EventHandler(this.cbTXT_CheckedChanged);
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(392, 481);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(48, 24);
            this.cbCR.TabIndex = 18;
            this.cbCR.Text = "CR";
            this.cbCR.CheckedChanged += new System.EventHandler(this.cbCR_CheckedChanged);
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.Location = new System.Drawing.Point(8, 112);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 23;
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            this.chbShowRetired.CheckedChanged += new System.EventHandler(this.chbShowRetired_CheckedChanged);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(278, 6);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 31;
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
            // EmpoyeeAnaliticReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(482, 554);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.chbPdf);
            this.Controls.Add(this.chbCSV);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbEmpolyee);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmpoyeeAnaliticReport";
            this.ShowInTaskbar = false;
            this.Text = "Employees Reports";
            this.Load += new System.EventHandler(this.EmployeeReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmpoyeeAnaliticReport_KeyUp);
            this.gbEmpolyee.ResumeLayout(false);
            this.gbEmpolyee.PerformLayout();
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<EmployeeTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(EmployeeTO x, EmployeeTO y)        
			{
				EmployeeTO empl1 = null;
				EmployeeTO empl2 = null;

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

				switch(compField)            
				{                
					case EmpoyeeAnaliticReport.FirstNameIndex:
						return empl1.FirstName.CompareTo(empl2.FirstName);
					case EmpoyeeAnaliticReport.LastNameIndex:
						return empl1.LastName.CompareTo(empl2.LastName);
					case EmpoyeeAnaliticReport.WorkingUnitIDIndex:
						return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
					default:                    
						return empl1.LastName.CompareTo(empl2.LastName);
				}        
			}    
		}

		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new EmployeesReports());
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void EmployeeReports_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);

                gbFilter.Text = rm.GetString("gbFilter", culture);

				// Initialize comparer object
				sortOrder = Constants.sortAsc;
				sortField = EmpoyeeAnaliticReport.LastNameIndex;
				startIndex = 0;						

				if (logInUser != null)
				{
					wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
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

				currentEmployeesList = new Employee().SearchByWU(wuString);
				currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));

				populateEmployeeListView(currentEmployeesList, startIndex);

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesReports.Load(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void populateEmployeeListView(List<EmployeeTO> employeeList, int startIndex)
		{
			try
			{
				if (employeeList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvEmployees.BeginUpdate();
				lvEmployees.Items.Clear();

				if (employeeList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < employeeList.Count))
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
						if (lastIndex >= employeeList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = employeeList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            EmployeeTO employee = employeeList[i];
                            ListViewItem item = new ListViewItem();
                            if (chbShowRetired.Checked || !employee.Status.Equals(Constants.statusRetired))
                            {
                                item.Text = employee.FirstName.Trim();
                                item.SubItems.Add(employee.LastName.Trim());

                                // Get Working Unit name for the particular user
                                //WorkingUnit wu = new WorkingUnit();
                                //if (wu.Find(employee.WorkingUnitID))
                                //{
                                //    item.SubItems.Add(wu.WUTO.Name.Trim());
                                //}
                                item.SubItems.Add(employee.WorkingUnitName.Trim());
                                item.Tag = employee.EmployeeID;

                                lvEmployees.Items.Add(item);
                            }
                        }
					}
				}

				lvEmployees.EndUpdate();
				lvEmployees.Invalidate();                
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesReports.populateEmployeeListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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

				currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateEmployeeListView(currentEmployeesList, startIndex);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.lvEmployees_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void tbLastName_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				int i=0;
				int p=0;
				
				this.Cursor = Cursors.WaitCursor;
				foreach(EmployeeTO employee in currentEmployeesList)
				{
					if (employee.LastName.ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper()))
					{
						p = i/Constants.recordsPerPage ;
						startIndex = p*Constants.recordsPerPage;
						populateEmployeeListView(currentEmployeesList, startIndex);
						break;
					}
					i++;
					
				}

				lvEmployees.SelectedItems.Clear();
				lvEmployees.Select();
				lvEmployees.Invalidate();
			
				foreach(ListViewItem item in lvEmployees.Items)
				{
					if (item.SubItems[1].Text.Trim().ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper()))
					{
						item.Selected = true;
						lvEmployees.Select();
						lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
						break;
					}
				}
			
				tbLastName.Focus();
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.tbLastName_TextChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				List<int> selectedEmployees = new List<int>();
				IOPair ioPair = new IOPair();

				// list of pairs for report
                List<IOPairTO> ioPairList = new List<IOPairTO>();

				// list of Time Schemas for selected Employee and selected Time Interval
                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

				// list of Time Schedules for one month
				//ArrayList timeSchedule = new ArrayList();

				if (lvEmployees.SelectedItems.Count == 0)
				{
					MessageBox.Show(rm.GetString("noEmplSelectedForRep", culture));
					return;
				}

				selectedEmployees.Add((int) lvEmployees.SelectedItems[0].Tag);

				//int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);
                int count = ioPair.SearchForEmployeesWrkHrsCount(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);
				if (count > Constants.maxEmplAnaliticReportRecords)
				{
					this.Cursor = Cursors.Arrow;
					MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
					return;
				}
				else if (count > Constants.warningEmplAnaliticReportRecords)
				{
					this.Cursor = Cursors.Arrow;
					DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
					if (result.Equals(DialogResult.No))
					{
						return;
					}
				}

				this.Cursor = Cursors.WaitCursor;
					
				// get all valid IO Pairs for selected employee and time interval
				//ioPairList = ioPair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);
                ioPairList = ioPair.SearchForEmployeesWrkHrs(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);

				// get all dates in selected interval which has open pairs
				//ArrayList datesList = new IOPair().SearchDatesWithOpenPairs(dtpFromDate.Value, dtpToDate.Value,
                List<DateTime> datesList = new IOPair().SearchDatesWithOpenPairsWrkHrs(dtpFromDate.Value, dtpToDate.Value,
					(int) lvEmployees.SelectedItems[0].Tag);
					
				// get Time Schemas for selected Employee and selected Time Interval
				DateTime date = dtpFromDate.Value.Date;

				/*while ((date <= dtpToDate.Value) || (date.Month == dtpToDate.Value.Month))
				{
					timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule((int) lvEmployees.SelectedItems[0].Tag, date);

					foreach (EmployeesTimeSchedule ets in timeSchedule)
					{
						timeScheduleList.Add(ets);
					}

					date = date.AddMonths(1);
				}*/
                //get time schemas for selected Employee, for selected Time Interval
                timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(selectedEmployees[0].ToString(), dtpFromDate.Value.Date, dtpToDate.Value.Date);

				// Kay is Date, Value is Time schema for that Date
				Dictionary<DateTime, WorkTimeSchemaTO> schemaForDay = new Dictionary<DateTime,WorkTimeSchemaTO>();
				TimeSchema schema = new TimeSchema();

				// Set Time Schema for every selected day
				date = dtpFromDate.Value.Date;
				while (date <= dtpToDate.Value.Date)
				{
					int timeScheduleIndex = -1;

					for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
					{
                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
						if (date >= timeScheduleList[scheduleIndex].Date)
							//&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
						{
							timeScheduleIndex = scheduleIndex;
						}
					}

					if (timeScheduleIndex >= 0)
					{
                        TimeSchema sch = new TimeSchema();
                        sch.TimeSchemaTO.TimeSchemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
						List<WorkTimeSchemaTO> schemas = sch.Search();
						if (schemas.Count > 0)
						{
							schemaForDay.Add(date, schemas[0]);
						}
					}
					
					date = date.AddDays(1);
				}

				// Key is Pass Type Id, Value is Pass Type Description
				Dictionary<int, string> passTypes = new Dictionary<int,string>();

				List<PassTypeTO> passTypesAll = new PassType().Search();
				foreach (PassTypeTO pt in passTypesAll)
				{
					passTypes.Add(pt.PassTypeID, pt.Description);
				}

				// Key is PassTypeID, Value is total time
				Hashtable passTypesTotalTime = new Hashtable();
			
				// Key is Date, Value is hours spent on job for that date
				Hashtable job = new Hashtable();
				
				// Key is Date, Value is late for that date
				Hashtable late = new Hashtable();

				// Key is Date, Value is earlier going away for that date
				Hashtable early = new Hashtable();

				// Key is Date, Value is overtime work for that date
				Hashtable overtime = new Hashtable();

				// Table Definition for Crystal Reports
				DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("employee_analytical");

				tableCR.Columns.Add("date", typeof(System.DateTime));
				tableCR.Columns.Add("time_schema", typeof(System.String));
				tableCR.Columns.Add("last_name", typeof(System.String));
				tableCR.Columns.Add("first_name", typeof(System.String));
				tableCR.Columns.Add("late", typeof(int));
				tableCR.Columns.Add("early", typeof(int));
				tableCR.Columns.Add("total_time", typeof(int));
				tableCR.Columns.Add("over_time", typeof(int));			
				tableCR.Columns.Add("need_validation", typeof(System.String));
				tableCR.Columns.Add("working_unit", typeof(System.String));
				tableCR.Columns.Add("employee_id", typeof(int));
                tableCR.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

				TimeSpan totalTime = new TimeSpan(0);

				// Job
				foreach (IOPairTO iopairTO in ioPairList)
				{
					totalTime = iopairTO.EndTime.Subtract(iopairTO.StartTime); 

					if (job.ContainsKey(iopairTO.IOPairDate.Date))
					{
						job[iopairTO.IOPairDate.Date] = ((TimeSpan) job[iopairTO.IOPairDate.Date]).Add(totalTime);	
					}
					else
					{
						job.Add(iopairTO.IOPairDate.Date, totalTime);
					}

					if (passTypesTotalTime.ContainsKey(iopairTO.PassTypeID))
					{
						passTypesTotalTime[iopairTO.PassTypeID] = ((TimeSpan) passTypesTotalTime[iopairTO.PassTypeID]).Add(totalTime);	
					}
					else
					{
						passTypesTotalTime.Add(iopairTO.PassTypeID, totalTime);
					}
				}
				
				// Late
				date = dtpFromDate.Value.Date;
				Dictionary<DateTime, IOPairTO> earlyestArrivedPair = new Dictionary<DateTime,IOPairTO>();

				while (date <= dtpToDate.Value.Date)
				{
					// Find IOPair with earliest arrival for particular date
					foreach(IOPairTO pair in ioPairList)
					{
						if (date.Equals(pair.IOPairDate))
						{
							if (earlyestArrivedPair.ContainsKey(date))
							{
								if (earlyestArrivedPair[date].StartTime.TimeOfDay > pair.StartTime.TimeOfDay)
								{
									earlyestArrivedPair[date] = pair;
								}
							}
							else
							{
								earlyestArrivedPair.Add(date, pair);
							}
						}
					}
					
					// Calculate Late
					if (earlyestArrivedPair.ContainsKey(date))
					{
                        if (!late.ContainsKey(date) && schemaForDay.ContainsKey(date))
                        {
                            WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
                            if (((TimeSpan)currentInterval.EndTime.Subtract(currentInterval.StartTime)).TotalMinutes != 0)
                            {
                                IOPairTO earlyestReal = earlyestArrivedPair[date];
                                if (earlyestReal.StartTime.TimeOfDay > currentInterval.LatestArrivaed.TimeOfDay)
                                {
                                    late.Add(date, earlyestReal.StartTime.TimeOfDay - currentInterval.LatestArrivaed.TimeOfDay);
                                }
                            }
                        }
					}
				
					date = date.AddDays(1);
				}

                // Earlier going away for that date
				date = dtpFromDate.Value.Date;
				Dictionary<DateTime, IOPairTO> latestLeftPairs = new Dictionary<DateTime,IOPairTO>();

				while (date <= dtpToDate.Value)
				{
					foreach(IOPairTO pair in ioPairList)
					{
						if (date.Equals(pair.IOPairDate))
						{
							if (latestLeftPairs.ContainsKey(date))
							{
								if (latestLeftPairs[date].EndTime.TimeOfDay < pair.EndTime.TimeOfDay)
								{
									latestLeftPairs[date] = pair;
								}
							}
							else
							{
								latestLeftPairs.Add(date, pair);
							}
						}
					}

					// Calculate earlier going away from job 
					if (latestLeftPairs.ContainsKey(date))
					{
						if (!early.ContainsKey(date) && schemaForDay.ContainsKey(date))
						{
                            WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
							IOPairTO latestReal = latestLeftPairs[date];
                            
                            WorkTimeSchemaTO currentSchema = schemaForDay[date];//ja
                            if (currentSchema.Type.Trim() == Constants.schemaTypeFlexi) //ja
                            {
                                //flexy working time
                                TimeSpan expectedDuarationL = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
                                DateTime expectedEndTime = new DateTime(earlyestArrivedPair[date].StartTime.Ticks);
                                if (latestReal.EndTime.TimeOfDay < (earlyestArrivedPair[date].StartTime.TimeOfDay + expectedDuarationL))
                                {
                                    early.Add(date, (earlyestArrivedPair[date].StartTime.TimeOfDay + expectedDuarationL) - latestReal.EndTime.TimeOfDay);
                                }
                            }
                            else
                            {
                                //not flexy working time
                                if (latestReal.EndTime.TimeOfDay < currentInterval.EarliestLeft.TimeOfDay)
                                {
                                    early.Add(date, currentInterval.EarliestLeft.TimeOfDay - latestReal.EndTime.TimeOfDay);
                                }
                            }
						}
					}

					date = date.AddDays(1);
				}

				// Overtime work for that date
				date = dtpFromDate.Value.Date;
				TimeSpan expectedDuaration = new TimeSpan(0);
				TimeSpan realTime = new TimeSpan(0);
				TimeSpan dif = new TimeSpan(0);
				 
				while (date <= dtpToDate.Value)
				{					
					if (job.ContainsKey(date))
					{
                        WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
						expectedDuaration = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
						realTime = (TimeSpan) job[date];
						dif = realTime - expectedDuaration;

						if (dif.TotalMinutes > 0)
						{
							if (overtime.ContainsKey(date))
							{
								overtime[date] = dif;
							}
							else
							{
								overtime.Add(date, dif); 
							}
						}
					}

					date = date.AddDays(1);
				}
				
				// ******** REPORT ********
				// all records for report
				date = dtpFromDate.Value.Date;
				ArrayList rowList = new ArrayList();
				WorkTimeSchemaTO tsc = new WorkTimeSchemaTO();

				// TOTALS
				TimeSpan totalLate = new TimeSpan(0);
				TimeSpan totalEarly = new TimeSpan(0);
				TimeSpan totalWorkTime = new TimeSpan(0);
				TimeSpan totalOverTime = new TimeSpan(0);

                int i = 0;
				while (date <= dtpToDate.Value.Date)
				{
					if (schemaForDay.ContainsKey(date) || late.ContainsKey(date) || early.ContainsKey(date) ||
						job.ContainsKey(date) || overtime.ContainsKey(date) || datesList.Contains(date))
					{
						DataRow rowCR = tableCR.NewRow();
						// One record in table

						ArrayList row = new ArrayList();

						rowCR["last_name"] = lvEmployees.SelectedItems[0].Text;
						rowCR["first_name"] = lvEmployees.SelectedItems[0].SubItems[1].Text;
						rowCR["working_unit"] = lvEmployees.SelectedItems[0].SubItems[2].Text;
                        rowCR["employee_id"] = (int) lvEmployees.SelectedItems[0].Tag;
						// Date
						row.Add(date.ToString("dd.MM.yyy"));
						rowCR["date"] = date;


						// Work Time Shema Description
						if (schemaForDay.ContainsKey(date))
						{
							tsc = schemaForDay[date];
						}
						else
						{
							tsc = new WorkTimeSchemaTO();
							tsc.Description = rm.GetString("noTimeSchema", culture);
						}
						
						row.Add(tsc.Description.ToString());
						rowCR["time_schema"] = tsc.Description.ToString();

						// Late
						if (late.ContainsKey(date))
						{
							totalTime = ((TimeSpan) late[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalLate = totalLate.Add(totalTime);
							rowCR["late"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["late"] = 0;
						}
						
						// Early
						if (early.ContainsKey(date))
						{
							totalTime = ((TimeSpan) early[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalEarly = totalEarly.Add(totalTime);
							rowCR["early"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["early"] = 0;
						}

						// Total working hours for a day
						if (job.ContainsKey(date))
						{
							totalTime = ((TimeSpan) job[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalWorkTime = totalWorkTime.Add(totalTime);
							rowCR["total_time"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["total_time"] = 0;
						}
					
						// Overtime work for that date
						if (overtime.ContainsKey(date))
						{
							totalTime = ((TimeSpan) overtime[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalOverTime = totalOverTime.Add(totalTime);
							rowCR["over_time"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["over_time"] = 0;
						}

						// Note
						if (datesList.Contains(date))
						{
							row.Add("X");
							rowCR["need_validation"] = "X";
						}
						else
						{
							row.Add("");
							rowCR["need_validation"] = "";
						}

                        rowCR["imageID"] = 1;
                        if (i == 0)
                        {
                            //add logo image just once
                            DataRow rowI = tableI.NewRow();
                            rowI["image"] = Constants.LogoForReport;
                            rowI["imageID"] = 1;
                            tableI.Rows.Add(rowI);
                            tableI.AcceptChanges();
                        }
						
						rowList.Add(row);
						tableCR.Rows.Add(rowCR);
                        i++;
					}
					
					date = date.AddDays(1);
				}
				dataSetCR.Tables.Add(tableCR);
                dataSetCR.Tables.Add(tableI);

				// Claculate Totals
				
				ArrayList totalsRowList = new ArrayList();

				// One record in Total's table
				ArrayList rowTotal = new ArrayList();

				rowTotal.Add("Ukupno:");
				rowTotal.Add("");

				rowTotal.Add((totalLate.Hours + (totalLate.Days * 24)).ToString() + "h " + totalLate.Minutes + "min");

				rowTotal.Add((totalEarly.Hours + (totalEarly.Days * 24)).ToString() + "h " + totalEarly.Minutes + "min");
				rowTotal.Add((totalWorkTime.Hours + (totalWorkTime.Days * 24)).ToString() + "h " + totalWorkTime.Minutes + "min");
				rowTotal.Add((totalOverTime.Hours + (totalOverTime.Days * 24)).ToString() + "h " + totalOverTime.Minutes + "min");
				rowTotal.Add("");

				totalsRowList.Add(rowTotal);
				

				// Calculate Totals by Pass Type
				ArrayList ptTotalsRowList = new ArrayList();

				foreach (int ptID in passTypes.Keys)
				{
					if (passTypesTotalTime.ContainsKey(ptID))
					{
						ptTotalsRowList.Add(passTypes[ptID].ToString().Trim() + ": " 
							+ (((TimeSpan) passTypesTotalTime[ptID]).Hours + (((TimeSpan) passTypesTotalTime[ptID]).Days * 24)).ToString() + "h " 
							+ ((TimeSpan) passTypesTotalTime[ptID]).Minutes + "min");
					}
				}

				// report
				/*
				if (this.chbPdf.Checked)
				{
					generatePDFReport(rowList, totalsRowList, ptTotalsRowList);
				}
				*/			

				if (this.chbCSV.Checked)
				{
					log.writeLog(DateTime.Now + " EmployeesAnaliticReports csv: STARTED!\n");
					generateCSVReport(rowList, totalsRowList, ptTotalsRowList);
					log.writeLog(DateTime.Now + " EmployeesAnaliticReports csv: FNISHED!\n");
				}

				if (this.cbTXT.Checked)
				{
					generateTXTReport(rowList, totalsRowList, ptTotalsRowList);
				}

				if (this.cbCR.Checked)
				{
					generateCRReport(dataSetCR);
				}

				//this.Close();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.btnGenerate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		/*
		private void pdfSetup(PDFDocument doc, string emplFirstName, string emplLastName, string employeeId)
		{
			try
			{
				// Set Document Header
				doc.SetLandscape();
				doc.Title = "Analiticki izveštaj po zaposlenom";
				doc.LeftBoxText = "Zaposleni: " + emplFirstName + " " + emplLastName + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Analiticki izvestaj-" + employeeId + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.pdfSetup(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		/// <summary>
		/// Generate PDF reports
		/// </summary>
		/// <param name="rowList"></param>
		/// <param name="totalList"></param>
		/// <param name="ptTotalsRowList"></param>
		/*
		private void generatePDFReport(ArrayList rowList, ArrayList totalList, ArrayList ptTotalsRowList)
		{
			try
			{
				PDFDocument doc = new PDFDocument();
				string emplFirstName = lvEmployees.SelectedItems[0].Text;
				string emplLastName = lvEmployees.SelectedItems[0].SubItems[1].Text;
				string employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();

				this.pdfSetup(doc, emplFirstName, emplLastName, employeeID);
				this.InsertTitle(14, doc);
			
				string[] colNames = {"Datum", "Satnica", "Kašnjenje", "Raniji odlazak", "Ukupno provedeno", "Prekovremeno", "Neophodna provera"};
				int[] colWidths = {3, 5, 3, 3, 3, 3, 3};
				double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};

				ArrayList tableData = PopulateData(rowList);

				doc.InsertTable(colNames, colWidths, colPositions, tableData, true, totalList, true, ptTotalsRowList);
				doc.InsertFooter(doc.FontSize);
				doc.Save();
				log.writeLog(DateTime.Now + " EmployeeAnaliticReport OPEN Document: Started! \n");
				doc.Open();
				log.writeLog(DateTime.Now + " EmployeeAnaliticReport OPEN Document: Finished! \n");
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.generatePDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		private void generateCSVReport(ArrayList rowList, ArrayList totalList, ArrayList ptTotalsRowList)
		{
			try
			{
				DataTable table = new DataTable();
				table.Columns.Add("datum", typeof(System.String));
				table.Columns.Add("satnica",typeof(System.String));
				table.Columns.Add("kašnjenje", typeof(System.String));
				table.Columns.Add("raniji_odlazak", typeof(System.String));
				table.Columns.Add("ukupno_provedeno", typeof(System.String));
				table.Columns.Add("prekovremeno", typeof(System.String));
				table.Columns.Add("neophodna_provera", typeof(System.String));
				
				for(int i=0; i< rowList.Count; i++) 
				{	
					ArrayList al = (ArrayList)rowList[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Count; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6};
				string[] cHeaders = {"Datum", "Satnica", "Kašnjenje", "Raniji odlazak", 
										"Ukupno provedeno", "Prekovremeno", "Neophodna provera"};						

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					//19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
					Export objExport = new Export("Win", cHeaders);
			 
					//RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
					string employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();
					string fileName = Constants.csvDocPath + "\\Analiticki izvestaj-" + employeeID + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAnaliticReport.generateCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateTXTReport(ArrayList rowList, ArrayList totalList, ArrayList ptTotalsRowList)
		{
			try
			{
				DataTable table = new DataTable();
				table.Columns.Add("datum", typeof(System.String));
				table.Columns.Add("satnica",typeof(System.String));
				table.Columns.Add("kašnjenje", typeof(System.String));
				table.Columns.Add("raniji_odlazak", typeof(System.String));
				table.Columns.Add("ukupno_provedeno", typeof(System.String));
				table.Columns.Add("prekovremeno", typeof(System.String));
				table.Columns.Add("neophodna_provera", typeof(System.String));
				
				for(int i=0; i< rowList.Count; i++) 
				{	
					ArrayList al = (ArrayList)rowList[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Count; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6};
				string[] cHeaders = {"Datum", "Satnica", "Kašnjenje", "Raniji odlazak", 
										"Ukupno provedeno", "Prekovremeno", "Neophodna provera"};						

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					//19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
					Export objExport = new Export("Win", cHeaders);
			 
					//RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
					string employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();
					string fileName = Constants.txtDocPath + "\\Analiticki izvestaj-" + employeeID + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
					objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);
					System.Diagnostics.Process.Start(fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAnaliticReport.generateTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private ArrayList PopulateData(ArrayList rowListData)
		{
			ArrayList result = new ArrayList();
			try
			{
				foreach(ArrayList row in rowListData)
				{
					string[] rowString = {row[0].ToString(), row[1].ToString(), row[2].ToString(),
											 row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString()};
					result.Add(rowString);
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeReports.populateDate(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

			return result;
		}

		private void setLanguage()
		{
			gbEmpolyee.Text = rm.GetString("employee", culture);
			lblLastName.Text = rm.GetString("lblLastName", culture);
			lblFrom.Text = rm.GetString("lblFrom", culture);
			gbTimeInterval.Text = rm.GetString("timeInterval", culture);
			lblTo.Text = rm.GetString("lblTo", culture);
			lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
			btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
			btnCancel.Text = rm.GetString("btnCancel", culture);
            chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
			this.Text = rm.GetString("employeeAnaliticReports", culture);
		}

		// Header
		/// <summary>
		/// Create header for pdf document
		/// </summary>
		/// <param name="titleFontSize"></param>
		/// <param name="doc"></param>
		/*
		public void InsertTitle(int titleFontSize, PDFDocument doc)
		{
			try
			{
				doc.TextStyle.Bold = true;
				doc.HeaderHeight = Constants.HeaderHeight;
				doc.Rect.String = "main";
				double totalWidth = doc.StandardHeight - (doc.BottomMargine + doc.TopMargine);
				doc.Rect.Height = doc.HeaderHeight;
				
				doc.Color.String = "0 0 0";

				doc.PageNumber = 1;
				doc.Rect.Position(doc.BottomMargine, doc.StandardWidth - doc.LeftMargine - doc.HeaderHeight);
				doc.Rect.Width = totalWidth / 4;
				doc.FontSize = Constants.pdfFontSize;
				doc.HPos = 0.0;
				doc.VPos = 1.0;
				doc.AddText(doc.LeftBoxText);

				doc.Rect.Position(doc.BottomMargine + totalWidth / 4, doc.StandardWidth - doc.LeftMargine - doc.HeaderHeight);
				doc.Rect.Width = 2 * (totalWidth / 4);
				doc.FontSize = titleFontSize;
				doc.HPos = 0.5;
				doc.VPos = 0.0;
				doc.AddText(doc.Title);

				doc.Rect.Position(doc.BottomMargine + 3 * (totalWidth / 4), doc.StandardWidth - doc.LeftMargine - doc.HeaderHeight);
				doc.FontSize = Constants.pdfFontSize;
				doc.Rect.Width = totalWidth / 4;
				doc.HPos = 1.0;
				doc.VPos = 1.0;
				doc.AddText(doc.RightBoxText);
				doc.TextStyle.Bold = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PDFDocument.InsertHeader(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

        public WorkTimeIntervalTO getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
		{
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

			int timeScheduleIndex = -1;

			for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
			{
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
				if (date >= timeScheduleList[scheduleIndex].Date)
					//&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
				{
					timeScheduleIndex = scheduleIndex;
				}
			}

			if (timeScheduleIndex >= 0)
			{
				int cycleDuration = 0;
				int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
				int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                TimeSchema sch = new TimeSchema();
                sch.TimeSchemaTO.TimeSchemaID = schemaID;
				List<WorkTimeSchemaTO> timeSchema = sch.Search();

                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
				if(timeSchema.Count > 0)
				{
                    schema = timeSchema[0];
                    cycleDuration = schema.CycleDuration;
				}

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
				//TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleList[timeScheduleIndex])).Date;
				//interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);
                interval = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration][0];
			}

			return interval;
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
				populateEmployeeListView(currentEmployeesList, startIndex);
				
				this.tbLastName.TextChanged -= new System.EventHandler(this.tbLastName_TextChanged);
				this.tbLastName.Clear();
				this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
				this.tbLastName.Focus();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAnaliticReports.btnPrev_Click(): " + ex.Message + "\n");
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
				populateEmployeeListView(currentEmployeesList, startIndex);

				this.tbLastName.TextChanged -= new System.EventHandler(this.tbLastName_TextChanged);
				this.tbLastName.Clear();
				this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
				this.tbLastName.Focus();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAnaliticReports.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void generateCRReport(DataSet dataCR)
        {
            try
            {
                DataTable table = dataCR.Tables["employee_analytical"];

                if (table.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports_sr.EmployeeAnalyticalCRView view = new Reports_sr.EmployeeAnalyticalCRView(
                        dataCR, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports_en.EmployeeAnalyticalCRView_en view = new Reports_en.EmployeeAnalyticalCRView_en(
                        dataCR, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAnaliticReports.generateCRReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbShowRetired_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                populateEmployeeListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAnalyticalReports.chbShowRetired_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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

        private void EmpoyeeAnaliticReport_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmpoyeeAnaliticReport.EmpoyeeAnaliticReport_KeyUp(): " + ex.Message + "\n");
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
	}
}
